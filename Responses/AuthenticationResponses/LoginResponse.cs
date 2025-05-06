namespace DemoLinkedInApi.Responses
{
    public record LoginResponse(string Token = null!, string RefreshToken = null!, bool EmailConfirmed = true);

}
