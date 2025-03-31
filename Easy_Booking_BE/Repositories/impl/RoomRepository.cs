using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Utilities;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;
    private readonly Util _util;

    public RoomRepository(EasyBookingBEContext context, IMapper mapper, Util util)
    {
        _context = context;
        _mapper = mapper;
        _util = util;
    }

    public async Task<BaseDataResponse<List<Room_FeatureIdsModel>>> GetAllRoomsAsync()
    {
        var r = await _context.Room!.ToListAsync();
        if (!r.Any())
        {
            return new BaseDataResponse<List<Room_FeatureIdsModel>>(
                statusCode: 404,
                message: Constants.NOT_FOUND,
                data: new List<Room_FeatureIdsModel>()
            );
        }

        // Lấy danh sách RoomId -> FeatureIds
        var roomFeatures = _context.Room_Features
            .AsEnumerable()
            .GroupBy(rf => rf.room_id)
            .ToDictionary(g => g.Key, g => g.Select(rf => rf.feature_id)
                .ToList());

        // Map Room -> RoomModel và gán FeatureIds
        var mappedData = r.Select(room => new Room_FeatureIdsModel
        {
            Room = _mapper.Map<RoomModel>(room), // Chuyển đổi Room -> RoomModel
            FeatureIds = roomFeatures.ContainsKey(room.room_id) ? roomFeatures[room.room_id] : new List<int>()
        }).ToList();
        return new BaseDataResponse<List<Room_FeatureIdsModel>>(
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mappedData
        );
    }

    public async Task<BaseDataResponse<Room_FeatureIdsModel>> GetRoomByIdAsync(int id)
    {
        var room = await _context.Room.FindAsync(id);
        if (room == null)
        {
            return new BaseDataResponse<Room_FeatureIdsModel>(
                statusCode: 404,
                message: Constants.NOT_FOUND,
                data: null
            );
        }

        // Fetch FeatureIds từ Room_Feature trước rồi xử lý trên client
        var featureIds = await _context.Room_Features
            .Where(rf => rf.room_id == id)
            .Select(rf => rf.feature_id)
            .ToListAsync();

        // Map Room -> RoomModel
        var roomModel = _mapper.Map<RoomModel>(room);

        // Trả về RoomModel + FeatureIds
        var respond = new Room_FeatureIdsModel
        {
            Room = roomModel,
            FeatureIds = featureIds
        };

        return new BaseDataResponse<Room_FeatureIdsModel>(
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: respond
        );
    }

    public async Task<BaseDataResponse<object>> CreateRoomAsync(Room_FeatureIdsModel room)
    {
        try
        {
            var userId = await _util.GetUserIdFromTokenAsync();
            var exist = await _context.Room.FirstOrDefaultAsync(r =>
                r.room_number == room.Room.room_number &&
                r.user_id == userId);
            if (exist != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST
                );
            }

            var newRoom = _mapper.Map<Room>(room.Room);
            newRoom.user_id = userId;
            
            await _context.Room.AddAsync(newRoom);
            await _context.SaveChangesAsync();

            if (room.FeatureIds.Any())
            {
                var roomFeatureList = room.FeatureIds.Select(rf => new Room_Feature
                {
                    room_id = newRoom.room_id,
                    feature_id = rf
                });
                await _context.Room_Features.AddRangeAsync(roomFeatureList);
                await _context.SaveChangesAsync();
            }

            var respond = new Room_FeatureIdsModel()
            {
                Room = _mapper.Map<RoomModel>(newRoom),
                FeatureIds = room.FeatureIds
            };

            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: respond.Room.room_id
            );
        }
        catch (Exception ex)
        {
            return new BaseDataResponse<object>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> UpdateRoomAsync(int id, RoomModel room)
    {
        var updateR = await _context.Room!.FindAsync(id);
        if (updateR != null)
        {
            if (id == room.room_id)
            {
                // Check duplicate Room
                var existR = await _context.Room!.FirstOrDefaultAsync(r =>
                    r.room_number == room.room_number &&
                    r.price_per_night == room.price_per_night &&
                    r.description == room.description &&
                    r.location == room.location &&
                    r.room_status_id == room.room_status_id &&
                    r.room_id != id);
                if (existR != null)
                {
                    return new BaseDataResponse<object>(
                        statusCode: 200,
                        message: Constants.ALREADY_EXSIST
                    );
                }

                // Update Room
                updateR.room_number = room.room_number;
                updateR.description = room.description;
                updateR.location = room.location;
                updateR.num_bathrooms = room.num_bathrooms;
                updateR.num_beds = room.num_beds;
                updateR.price_per_night = room.price_per_night;
                updateR.room_status_id = room.room_status_id;

                _context.Room.Update(updateR);
                await _context.SaveChangesAsync();

                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.SUCCESSFUL,
                    data: updateR
                );
            }

            return new BaseDataResponse<object>(
                statusCode: 404,
                message: Constants.ERROR
            );
        }

        return new BaseDataResponse<object>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<object>> CheckDuplicateRoomAsync(RoomModel model)
    {
        var exist = await _context.Room!.FirstOrDefaultAsync(r =>
            r.room_number == model.room_number &&
            r.price_per_night == model.price_per_night &&
            r.description == model.description &&
            r.location == model.location &&
            r.room_status_id == model.room_status_id);
        if (exist != null)
        {
            return new BaseDataResponse<object>(
                statusCode: 400,
                message: Constants.ALREADY_EXSIST,
                data: exist
            );
        }

        return new BaseDataResponse<object>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<List<RoomModel>>> GetRoomsByUserIdAsync()
    {
        try
        {
            var userId = await _util.GetUserIdFromTokenAsync();
            if (userId == null)
            {
                return new BaseDataResponse<List<RoomModel>>(
                    statusCode: 401,
                    message: Constants.ERROR
                );
            }

            var listRoom = await _context.Room!.Where(r => r.user_id == userId).ToListAsync();
            var mappedData = _mapper.Map<List<RoomModel>>(listRoom);
            return new BaseDataResponse<List<RoomModel>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: mappedData
            );
        }
        catch (Exception ex)
        {
            return new BaseDataResponse<List<RoomModel>>(
                statusCode: 404,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<List<RoomModel>>> SearchRoomsByLocationAsync(RoomSearchModel model)
    {
        var searchR = await _context.Room!.Where(r => r.location.Contains(model.location)).ToListAsync();
        var mappedData = _mapper.Map<List<RoomModel>>(searchR);
        if (searchR.Any())
        {
            return new BaseDataResponse<List<RoomModel>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: mappedData
            );
        }

        return new BaseDataResponse<List<RoomModel>>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<object>> UpdateRoomStatusAsync(int id, RoomUpdateStatus model)
    {
        var searchR = await _context.Room!.FindAsync(id);
        if (searchR != null)
        {
            if (id == model.room_id)
            {
                searchR.room_status_id = model.room_status_id;
                _context.Room.Update(searchR);
                await _context.SaveChangesAsync();
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.SUCCESSFUL,
                    data: searchR
                );
            }

            return new BaseDataResponse<object>(
                statusCode: 404,
                message: Constants.ERROR
            );
        }

        return new BaseDataResponse<object>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }


    public async Task<BaseDataResponse<List<Room_FeatureIdsModel>>> GetRoomByStatusIdAsync(int id)
    {
        var searchR = await _context.Room!.Where(r => r.room_status_id == id).ToListAsync();

        if (!searchR.Any())
        {
            return new BaseDataResponse<List<Room_FeatureIdsModel>>(
                statusCode: 404,
                message: Constants.NOT_FOUND,
                data: new List<Room_FeatureIdsModel>()
            );
        }

        // Lấy danh sách RoomId -> FeatureIds
        var roomFeatures = _context.Room_Features
            .AsEnumerable()
            .GroupBy(rf => rf.room_id)
            .ToDictionary(g => g.Key, g => g.Select(rf => rf.feature_id)
                .ToList());

        // Map Room -> RoomModel và gán FeatureIds
        var mappedData = searchR.Select(room => new Room_FeatureIdsModel
        {
            Room = _mapper.Map<RoomModel>(room), // Chuyển đổi Room -> RoomModel
            FeatureIds = roomFeatures.ContainsKey(room.room_id) ? roomFeatures[room.room_id] : new List<int>()
        }).ToList();
        return new BaseDataResponse<List<Room_FeatureIdsModel>>(
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mappedData
        );
    }
}