using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;

namespace Easy_Booking_BE.Repositories;

public interface ILocationRepository
{
    public Task<BaseDataResponse<List<LocationModel>>> GetLocationsAsync();
    public Task<BaseDataResponse<LocationModel>> GetLocationByIdAsync(int id);
    public Task<BaseDataResponse<object>> CreateLocationAsync(LocationModel location);
    public Task<BaseDataResponse<object>> UpdateLocationAsync(int id, LocationModel location);
    public Task<BaseDataResponse<object>> DeleteLocationAsync(int id);
    public Task<BaseDataResponse<List<LocationModel>>> SearchLocationAsync(LocationModel location);
    
}