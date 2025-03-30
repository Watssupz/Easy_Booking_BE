using Easy_Booking_BE.Models.Response;
using Final_Project_PRN221.Models.DTO;

namespace Easy_Booking_BE.Repositories;

public interface IQrGenerateRepository
{
    public Task<BaseDataResponse<object>> QrGenerate(BankInfoDTO bankInfo);
}