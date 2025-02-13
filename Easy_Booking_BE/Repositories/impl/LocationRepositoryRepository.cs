using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly IMapper _mapper;
    private readonly EasyBookingBEContext _context;

    public LocationRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseDataResponse<List<LocationModel>>> GetLocationsAsync()
    {
        var l = await _context.Locations!.ToListAsync();
        var mappedData = _mapper.Map<List<LocationModel>>(l);
        return new BaseDataResponse<List<LocationModel>>(
            statusCode: mappedData.Any() ? 200 : 404,
            message: mappedData.Any() ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: mappedData.Any() ? mappedData : new List<LocationModel>()
        );
    }

    public async Task<BaseDataResponse<LocationModel>> GetLocationByIdAsync(int id)
    {
        var f = await _context.Locations!.FindAsync(id);
        return new BaseDataResponse<LocationModel>(
            statusCode: f != null ? 200 : 404,
            message: f != null ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: f != null ? _mapper.Map<LocationModel>(f) : null
        );
    }

    public async Task<BaseDataResponse<object>> CreateLocationAsync(LocationModel location)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(location.location_name))
            {
                return new BaseDataResponse<object>(
                    statusCode: 400,
                    message: Constants.NOT_NULL
                );
            }
            var exist = await _context.Locations!.FirstOrDefaultAsync(l =>
                l.location_name == location.location_name);
            if (exist != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST
                );
            }

            var newL = _mapper.Map<Location>(location);
            await _context.Locations.AddAsync(newL);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: newL
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

    public async Task<BaseDataResponse<object>> UpdateLocationAsync(int id, LocationModel location)
    {
        var updateL = await _context.Locations!.FindAsync(id);
        if (updateL != null)
        {
            if (id == location.location_id && !string.IsNullOrWhiteSpace(location.location_name))
            {
                var existL = await _context.Locations.FirstOrDefaultAsync(l =>
                    l.location_name == location.location_name && l.location_id != location.location_id);
                if (existL != null)
                {
                    return new BaseDataResponse<object>(
                        statusCode: 200,
                        message: Constants.ALREADY_EXSIST
                    );
                }

                updateL.location_name = location.location_name;
                _context.Locations.Update(updateL);
                await _context.SaveChangesAsync();
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.SUCCESSFUL,
                    data: updateL
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

    public async Task<BaseDataResponse<object>> DeleteLocationAsync(int id)
    {
        var l = await _context.Locations!.FirstOrDefaultAsync(l =>
            l.location_id == id);
        if (l != null)
        {
            _context.Locations!.Remove(l);
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

    public async Task<BaseDataResponse<List<LocationModel>>> SearchLocationAsync(LocationModel model)
    {
        var searchL = await _context.Locations.Where(l => l.location_name.Contains(model.location_name)).ToListAsync();
        var mappedData = _mapper.Map<List<LocationModel>>(searchL);
        if (searchL.Any())
        {
            return new BaseDataResponse<List<LocationModel>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: mappedData
            );
        }

        return new BaseDataResponse<List<LocationModel>>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }
}