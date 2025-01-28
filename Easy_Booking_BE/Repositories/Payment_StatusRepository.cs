using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Models;
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
    
    public async Task<List<Payment_StatusModel>> GetAllPayment_Status()
    {
        var ps = await _context.Payment_Status!.ToListAsync();
        return _mapper.Map<List<Payment_StatusModel>>(ps);
    }

    public async Task<Payment_StatusModel> GetPayment_StatusById(int id)
    {
        var ps = await _context.Payment_Status!.FindAsync(id);
        return _mapper.Map<Payment_StatusModel>(ps);
    }

    public async Task<int> AddPayment_Status(Payment_StatusModel payment_Status)
    {
        var newPayment_Status = _mapper.Map<Payment_Status>(payment_Status);
        _context.Payment_Status!.Add(newPayment_Status);
        await _context.SaveChangesAsync();
        return newPayment_Status.payment_id;
    }

    public async Task UpdatePayment_Status(int id, Payment_StatusModel payment_Status)
    {
        if (id == payment_Status.payment_id)
        {
            var updatePS = _mapper.Map<Payment_Status>(payment_Status);
            _context.Payment_Status!.Update(updatePS);
            await _context.SaveChangesAsync();
        }   
    }

    public async Task DeletePayment_Status(int id)
    {
        var deletePS = _context.Payment_Status!.FirstOrDefault(ps => ps.payment_id == id);
        if (deletePS != null)
        {
            _context.Payment_Status!.Remove(deletePS);
            await _context.SaveChangesAsync();
        }
    }
}