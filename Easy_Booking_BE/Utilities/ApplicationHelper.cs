using AutoMapper;
using Easy_Booking_BE.Models;
using EasyBooking.Data;

namespace Easy_Booking_BE.Utilities;

public class ApplicationHelper : Profile
{

    public ApplicationHelper()
    {
        CreateMap<Booking, BookingModel>().ReverseMap();
        CreateMap<Payment_Status, Payment_StatusModel>().ReverseMap();
    }
    
}