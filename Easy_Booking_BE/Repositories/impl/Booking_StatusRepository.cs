using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class Booking_StatusRepository : IBooking_StatusRepository
{
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;

    public Booking_StatusRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseDataResponse<List<Booking_StatusModel>>> GetAllBooking_Status()
    {
        var bs = await _context.Booking_Status!.ToListAsync();
        var mappedData = _mapper.Map<List<Booking_StatusModel>>(bs);
        return new BaseDataResponse<List<Booking_StatusModel>>
        (
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mappedData != null && mappedData.Any() ? mappedData : new List<Booking_StatusModel>()
        );
    }

    public async Task<BaseDataResponse<Booking_StatusModel>> GetBooking_StatusById(int id)
    {
        var bs = await _context.Booking_Status!.FindAsync(id);
        return new BaseDataResponse<Booking_StatusModel>(
            statusCode: 200,
            message: bs != null ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: bs != null ? _mapper.Map<Booking_StatusModel>(bs) : null
        );
    }

    public async Task<BaseDataResponse<object>> CreateBooking_Status(Booking_StatusModel booking_status)
    {
        try
        {
            var exist = await _context.Booking_Status!.FirstOrDefaultAsync(bs =>
                bs.booking_status_name == booking_status.booking_status_name);
            if (exist != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.UNSUCCESSFUL
                );
            }

            var newBS = _mapper.Map<Booking_Status>(booking_status);
            _context.Booking_Status.Add(newBS);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: newBS
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

    public async Task<BaseDataResponse<object>> UpdateBooking_Status(int id, Booking_StatusModel booking_status)
    {
        var updateBS = await _context.Booking_Status!.FindAsync(id);
        if (updateBS != null)
        {
            var existBS = await _context.Booking_Status.FirstOrDefaultAsync(bs =>
                bs.booking_status_name == booking_status.booking_status_name && bs.booking_status_id != id);
            if (existBS != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST
                );
            }

            updateBS.booking_status_name = booking_status.booking_status_name;
            _context.Booking_Status!.Update(updateBS);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: updateBS
            );
        }

        return new BaseDataResponse<object>(
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<object>> DeleteBooking_Status(int id)
    {
        var bs = await _context.Booking_Status!.FirstOrDefaultAsync(bs =>
            bs.booking_status_id == id);
        if (bs != null)
        {
            _context.Booking_Status!.Remove(bs);
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