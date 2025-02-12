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
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mappedData != null && mappedData.Any() ? mappedData : new List<FeatureModel>()
        );
    }

    public async Task<BaseDataResponse<FeatureModel>> GetFeatureByIdAsync(int id)
    {
        var f = await _context.Features!.FindAsync(id);
        return new BaseDataResponse<FeatureModel>(
            statusCode: 200,
            message: f != null ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: f != null ? _mapper.Map<FeatureModel>(f) : null
        );
    }

    public async Task<BaseDataResponse<object>> CreateFeatureAsync(FeatureModel feature)
    {
        try
        {
            var exist = await _context.Features!.FirstOrDefaultAsync(f =>
                f.feature_name == feature.feature_name);
            if (exist != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.UNSUCCESSFUL
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
            var existF = await _context.Features!.FirstOrDefaultAsync(f =>
                f.feature_name == feature.feature_name && f.feature_id != id);
            if (existF != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.UNSUCCESSFUL
                );
            }

            updateF.feature_name = feature.feature_name;
            _context.Features!.Update(updateF);
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
            statusCode: 200,
            message: Constants.UNSUCCESSFUL
        );
    }
}