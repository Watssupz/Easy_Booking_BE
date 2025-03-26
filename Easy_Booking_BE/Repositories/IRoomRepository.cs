using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;

namespace Easy_Booking_BE.Repositories;

public interface IRoomRepository
{
    public Task<BaseDataResponse<List<Room_FeatureIdsModel>>> GetAllRoomsAsync();
    public Task<BaseDataResponse<Room_FeatureIdsModel>> GetRoomByIdAsync(int id);
    public Task<BaseDataResponse<object>> CreateRoomAsync(Room_FeatureIdsModel room);
    public Task<BaseDataResponse<object>> UpdateRoomAsync(int id, RoomModel room);
    public Task<BaseDataResponse<object>> UpdateRoomStatusAsync(int id, RoomUpdateStatus model);
    public Task<BaseDataResponse<List<RoomModel>>> SearchRoomsByLocationAsync(RoomSearchModel model);
    public Task<BaseDataResponse<List<Room_FeatureIdsModel>>> GetRoomByStatusIdAsync(int id);
    public Task<BaseDataResponse<object>> CheckDuplicateRoomAsync(RoomModel model);
    public Task<BaseDataResponse<List<RoomModel>>> GetRoomsByUserIdAsync();
}