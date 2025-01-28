using Easy_Booking_BE.Models;

namespace Easy_Booking_BE.Repositories;

public interface IPayment_StatusRepository
{
    public Task<List<Payment_StatusModel>> GetAllPayment_Status();
    public Task<Payment_StatusModel> GetPayment_StatusById(int id);
    public Task<int> AddPayment_Status(Payment_StatusModel payment_Status);
    public Task UpdatePayment_Status(int id, Payment_StatusModel payment_Status);
    public Task DeletePayment_Status(int id);
}