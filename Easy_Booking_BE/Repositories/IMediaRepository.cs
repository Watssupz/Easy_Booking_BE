using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;

namespace Easy_Booking_BE.Repositories;

public interface IMediaRepository
{
    public Task<BaseDataResponse<List<MediaModel>>> GetMediaByRoomId(int roomId);
    public Task<BaseDataResponse<string>> CreateMediaByRoomId(int room_id, IFormFileCollection uploadModels);
    public Task<BaseDataResponse<string>> CreateThumbnailByRoomId(int room_id, IFormFile uploadModels);
    public Task<BaseDataResponse<string>> SaveRoomMediaToDB(int room_id, List<byte[]> pictures);
}