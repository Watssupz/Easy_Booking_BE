using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Utilities;
using Final_Project_PRN221.Models.DTO;

namespace Easy_Booking_BE.Repositories;

public class QrGenerateRepository : IQrGenerateRepository
{
    public async Task<BaseDataResponse<object>> QrGenerate(BankInfoDTO bank)
    {
        try
        {
            string qrCode = await Util.GenQR(970422, "0393994806", "Nguyen Manh Duong", bank.amount, "text", "print",
                bank.content);
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: qrCode
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<object>(
                statusCode: 400,
                message: Constants.ERROR
            );
        }
        
    }
}