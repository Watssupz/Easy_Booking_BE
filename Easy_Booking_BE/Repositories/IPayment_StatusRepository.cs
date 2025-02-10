using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;

namespace Easy_Booking_BE.Repositories;

public interface IPayment_StatusRepository
{
    public Task<BaseDataResponse<List<Payment_StatusModel>>> GetAllPayment_Status();
    public Task<BaseDataResponse<Payment_StatusModel>> GetPayment_StatusById(int id);
    public Task<BaseDataResponse<object>> AddPayment_Status(Payment_StatusModel payment_Status);
    public Task<BaseDataResponse<object>> UpdatePayment_Status(int id, Payment_StatusModel payment_Status);
    public Task<BaseDataResponse<object>> DeletePayment_Status(int id);
}