using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class Payment_StatusRepository : IPayment_StatusRepository
{
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;

    public Payment_StatusRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseDataResponse<List<Payment_StatusModel>>> GetAllPayment_Status()
    {
        var ps = await _context.Payment_Status!.ToListAsync();
        var mappedData = _mapper.Map<List<Payment_StatusModel>>(ps);
        return new BaseDataResponse<List<Payment_StatusModel>>(
            statusCode: mappedData.Any() ? 200 : 404,
            message: mappedData.Any() ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: mappedData.Any() ? mappedData : new List<Payment_StatusModel>()
        );
    }

    public async Task<BaseDataResponse<Payment_StatusModel>> GetPayment_StatusById(int id)
    {
        var ps = await _context.Payment_Status!.FindAsync(id);
        return new BaseDataResponse<Payment_StatusModel>
        (
            statusCode: ps != null ? 200 : 404,
            message: ps != null ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: ps != null ? _mapper.Map<Payment_StatusModel>(ps) : null
        );
    }

    public async Task<BaseDataResponse<object>> AddPayment_Status(Payment_StatusModel payment_Status)
    {
        try
        {
            var exist_ps = await _context.Payment_Status.FirstOrDefaultAsync(ps =>
                ps.payment_status_name == payment_Status.payment_status_name);
            if (exist_ps != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST
                );
            }

            var newPaymentStatus = _mapper.Map<Payment_Status>(payment_Status);
            _context.Payment_Status!.Add(newPaymentStatus);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: newPaymentStatus
            );
        }
        catch (DbUpdateException ex)
        {
            return new BaseDataResponse<object>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> UpdatePayment_Status(int id, Payment_StatusModel paymentStatus)
    {
        var updatePs = await _context.Payment_Status!.FindAsync(id);
        if (updatePs != null)
        {
            if (id == paymentStatus.payment_id)
            {
                var existPs = await _context.Payment_Status.FirstOrDefaultAsync(ps =>
                    ps.payment_status_name == paymentStatus.payment_status_name && ps.payment_id != id);

                if (existPs != null)
                {
                    return new BaseDataResponse<object>(
                        statusCode: 200,
                        message: Constants.ALREADY_EXSIST
                    );
                }

                updatePs.payment_status_name = paymentStatus.payment_status_name;
                _context.Payment_Status!.Update(updatePs);
                await _context.SaveChangesAsync();
                return new BaseDataResponse<object>
                (
                    statusCode: 200,
                    message: Constants.SUCCESSFUL,
                    data: updatePs
                );
            }
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.NOT_MATCH
            );
        }

        return new BaseDataResponse<object>
        (
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<object>> DeletePayment_Status(int id)
    {
        var deletePs = _context.Payment_Status!.FirstOrDefault(ps => ps.payment_id == id);
        if (deletePs != null)
        {
            _context.Payment_Status!.Remove(deletePs);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>
            (
                statusCode: 200,
                message: Constants.SUCCESSFUL
            );
        }

        return new BaseDataResponse<object>
        (
            statusCode: 404,
            message: Constants.UNSUCCESSFUL
        );
    }
}