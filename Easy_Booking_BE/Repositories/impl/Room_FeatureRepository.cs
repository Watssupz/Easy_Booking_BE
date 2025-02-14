using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class Room_FeatureRepository : IRoom_FeatureRepository
{
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;

    public Room_FeatureRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseDataResponse<object>> UpdateRoomFeatureIds(UpdateRoomFeatureModel model)
    {
        var existingRoomFeatures = await _context.Room_Features!
            .Where(rf => rf.room_id == model.room_id)
            .ToListAsync();
        if (!existingRoomFeatures.Any())
        {
            // **Tạo các FeatureIds mới cho Room**
            var featuresToAdd = model.FeatureIds
                .Select(fid => new Room_Feature
                {
                    room_id = model.room_id,
                    feature_id = fid
                })
                .ToList();

            // Thêm vào Room_Feature
            await _context.Room_Features.AddRangeAsync(featuresToAdd);
            await _context.SaveChangesAsync();

            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: model
            );
        }

        // Lấy danh sách feature_id hiện tại trong Room_Feature
        var existingFeatureIds = existingRoomFeatures.Select(rf => rf.feature_id).ToList();

        // Kiểm tra nếu FeatureIds mới không thay đổi, không cần update
        if (existingFeatureIds.OrderBy(id => id).SequenceEqual(model.FeatureIds.OrderBy(id => id)))
        {
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.NO_CHANGES,
                data: null
            );
        }

        // **Xóa các FeatureIds cũ**
        var featuresToRemove = existingRoomFeatures.Where(rf => !model.FeatureIds.Contains(rf.feature_id)).ToList();
        _context.Room_Features.RemoveRange(featuresToRemove);

        // **Thêm các FeatureIds mới**
        var featuresToAddNew = model.FeatureIds
            .Where(fid => !existingFeatureIds.Contains(fid))
            .Select(fid => new Room_Feature
            {
                room_id = model.room_id,
                feature_id = fid
            })
            .ToList();

        if (featuresToAddNew.Any())
        {
            await _context.Room_Features.AddRangeAsync(featuresToAddNew);
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        await _context.SaveChangesAsync();

        return new BaseDataResponse<object>(
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: model
        );
    }
}