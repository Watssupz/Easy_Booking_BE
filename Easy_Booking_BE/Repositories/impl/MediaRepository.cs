using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class MediaRepository : IMediaRepository
{   
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;

    public MediaRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<BaseDataResponse<List<MediaModel>>> GetMediaByRoomId(int roomId)
    {
        var listMedia = await _context.Medias.Where(r => r.room_id == roomId).ToListAsync();
        if (!listMedia.Any())
        {
            return new BaseDataResponse<List<MediaModel>>(
                statusCode: 404,
                message: Constants.NOT_FOUND
            );
        }
        var mediaModels = _mapper.Map<List<MediaModel>>(listMedia);
        return new BaseDataResponse<List<MediaModel>>(
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mediaModels
        );
    }

    public async Task<BaseDataResponse<string>> CreateMediaByRoomId(int room_id, IFormFileCollection uploadModels)
    {
        try
        {
            if (!uploadModels.Any())
            {
                return new BaseDataResponse<string>(
                    statusCode: 400,
                    message: Constants.ERROR
                );
            }

            var picture = new List<byte[]>();
            foreach (var file in uploadModels)
            {
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        picture.Add(memoryStream.ToArray());
                    }
                }
            }
            return await SaveRoomMediaToDB(room_id, picture);
        }
        catch (Exception ex)
        {
            return new BaseDataResponse<string>(
                statusCode: 500,
                message: ex.Message
            );
        }
    }

    public async Task<BaseDataResponse<string>> SaveRoomMediaToDB(int room_id, List<byte[]> pictures)
    {
        try
        {
            var roomExist = await _context.Room.AnyAsync(r => r.room_id == room_id);
            if (!roomExist)
            {
                return new BaseDataResponse<string>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            // remove old room media 
            var oldMedias = await _context.Medias.Where(m => m.room_id == room_id).ToListAsync();
            _context.Medias.RemoveRange(oldMedias);
            
            var mediaList = new List<Media>();
            foreach (var pic in pictures)
            {
                if (pic != null && pic.Length > 0)
                {
                    mediaList.Add(new Media
                    {
                        room_id = room_id,
                        picture = pic
                    });
                }
            }
            _context.Medias.AddRange(mediaList);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<string>(
                statusCode: 200,
                message: Constants.SUCCESSFUL
            );
        }
        catch (Exception ex)
        {
            return new BaseDataResponse<string>(
                statusCode: 500,
                message: ex.Message
            );
        }
    }
}