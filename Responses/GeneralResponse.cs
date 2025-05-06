namespace DemoLinkedInApi.Responses
{
    public record GeneralResponse<T>(
        bool IsSuccessful, 
        string Message = null!, 
        int StatusCode = StatusCodes.Status200OK, 
        T? Data = default);
}