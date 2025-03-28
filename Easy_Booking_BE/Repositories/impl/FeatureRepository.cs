using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class FeatureRepository : IFeatureRepository
{
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;

    public FeatureRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseDataResponse<List<FeatureModel>>> GetAllFeaturesAsync()
    {
        var f = await _context.Features!.ToListAsync();
        var mappedData = _mapper.Map<List<FeatureModel>>(f);
        return new BaseDataResponse<List<FeatureModel>>(
            statusCode: mappedData.Any() ? 200 : 404,
            message: mappedData.Any() ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: mappedData.Any() ? mappedData : new List<FeatureModel>()
        );
    }

    public async Task<BaseDataResponse<FeatureModel>> GetFeatureByIdAsync(int id)
    {
        var f = await _context.Features!.FindAsync(id);
        return new BaseDataResponse<FeatureModel>(
            statusCode: f != null ? 200 : 404,
            message: f != null ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: f != null ? _mapper.Map<FeatureModel>(f) : null
        );
    }

    public async Task<BaseDataResponse<List<String>>> GetFeatureByRoomId(int rid)
    {
        try
        {
            var exist = await _context.Room.FirstOrDefaultAsync(r => r.room_id == rid);
            if (exist == null)
            {
                return new BaseDataResponse<List<String>>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            var list_feature = await _context.Room_Features
                .Include(r => r.Feature)
                .Where(rf => rf.room_id == rid).Select(x => x.Feature.feature_name).ToListAsync();

            return new BaseDataResponse<List<String>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: list_feature
            );
        }
        catch (Exception ex)
        {
            return new BaseDataResponse<List<String>>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> CreateFeatureAsync(FeatureModel feature)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(feature.feature_name))
            {
                return new BaseDataResponse<object>(
                    statusCode: 400,
                    message: Constants.NOT_NULL
                );
            }
            var exist = await _context.Features!.FirstOrDefaultAsync(f =>
                f.feature_name == feature.feature_name);
            if (exist != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST
                );
            }

            var newF = _mapper.Map<Feature>(feature);
            await _context.Features.AddAsync(newF);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: newF
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<object>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> UpdateFeatureAsync(int id, FeatureModel feature)
    {
        var updateF = await _context.Features!.FindAsync(id);
        if (updateF != null)
        {
            if (id == feature.feature_id && !string.IsNullOrWhiteSpace(feature.feature_name))
            {
                var existF = await _context.Features!.FirstOrDefaultAsync(f =>
                    f.feature_name == feature.feature_name && f.feature_id != id);
                if (existF != null)
                {
                    return new BaseDataResponse<object>(
                        statusCode: 200,
                        message: Constants.ALREADY_EXSIST
                    );
                }

                updateF.feature_name = feature.feature_name;
                _context.Features.Update(updateF);
                await _context.SaveChangesAsync();
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.SUCCESSFUL,
                    data: updateF
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

    public async Task<BaseDataResponse<object>> DeleteFeatureAsync(int id)
    {
        var f = await _context.Features!.FirstOrDefaultAsync(f =>
            f.feature_id == id);
        if (f != null)
        {
            _context.Features!.Remove(f);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL
            );
        }

        return new BaseDataResponse<object>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<List<FeatureModel>>> SearchFeatureAsync(FeatureModel feature)
    {
        var searchF = await _context.Features!.Where(f => f.feature_name.Contains(feature.feature_name)).ToListAsync();
        var mappedData = _mapper.Map<List<FeatureModel>>(searchF);
        if (searchF.Any())
        {
            return new BaseDataResponse<List<FeatureModel>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: mappedData
            );
        }

        return new BaseDataResponse<List<FeatureModel>>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }
}