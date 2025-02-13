using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;

namespace Easy_Booking_BE.Repositories;

public interface IFeatureRepository
{
    public Task<BaseDataResponse<List<FeatureModel>>> GetAllFeaturesAsync();
    public Task<BaseDataResponse<FeatureModel>> GetFeatureByIdAsync(int id);
    public Task<BaseDataResponse<object>> CreateFeatureAsync(FeatureModel feature);
    public Task<BaseDataResponse<object>> UpdateFeatureAsync(int id, FeatureModel feature);
    public Task<BaseDataResponse<object>> DeleteFeatureAsync(int id);
    public Task<BaseDataResponse<List<FeatureModel>>> SearchFeatureAsync(FeatureModel feature);
    
}