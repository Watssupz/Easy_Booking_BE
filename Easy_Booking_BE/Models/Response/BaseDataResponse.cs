namespace Easy_Booking_BE.Models.Response;

public class BaseDataResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public BaseDataResponse(int statusCode, string message, T data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    public BaseDataResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
    
}