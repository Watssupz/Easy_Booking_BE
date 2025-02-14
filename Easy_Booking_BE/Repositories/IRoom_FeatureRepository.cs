using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;

namespace Easy_Booking_BE.Repositories;

public interface IRoom_FeatureRepository
{
    public Task<BaseDataResponse<object>> UpdateRoomFeatureIds(UpdateRoomFeatureModel model);
}