using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;

namespace Easy_Booking_BE.Repositories;

public interface IMediaRepository
{
    public Task<BaseDataResponse<string>> CreateMediaByRoomId(int room_id, IFormFileCollection uploadModels);
    public Task<BaseDataResponse<string>> SaveRoomMediaToDB(int room_id, List<byte[]> pictures);
}