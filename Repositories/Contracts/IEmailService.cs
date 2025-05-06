namespace DemoLinkedIn.Server.Repositories.Contracts;

public interface IEmailService
{
    string BuildEmail(string email, string emailCode);
}