using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;

namespace Easy_Booking_BE.Repositories;

public interface IRegionRepository
{
    public Task<BaseDataResponse<List<ProvinceModel>>> GetProvincesAsync();
    public Task<BaseDataResponse<List<DistrictModel>>> GetDistrictAsync(int province_id);
    public Task<BaseDataResponse<List<WardModel>>> GetWardAsync(int district_id);
}