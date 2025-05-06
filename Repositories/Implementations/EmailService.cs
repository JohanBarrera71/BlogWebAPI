using System.Text;
using DemoLinkedIn.Server.Repositories.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace DemoLinkedIn.Server.Repositories.Implementations;

public class EmailService: IEmailService
{
    public string BuildEmail(string email, string emailCode)
    {
        StringBuilder emailMessage = new StringBuilder();
        emailMessage.AppendLine("<html>");
        emailMessage.AppendLine("<body>");
        emailMessage.AppendLine($"<p>Dear {email},</p>");
        emailMessage.AppendLine(
            "<p>Thank you for registering with us. To verify your email address, please use the following verification code:</p>");
        emailMessage.AppendLine($"<h2>Verification Code: {emailCode}</h2>");
        emailMessage.AppendLine("<p>Please enter this code on our website to complete your registration.</p>");
        emailMessage.AppendLine("<p>If you did not request this, please ignore this email.</p>");
        emailMessage.AppendLine("<br>");
        emailMessage.AppendLine("<p>Best regards,</p>");
        emailMessage.AppendLine("<p><strong>JohanDev</strong></p>");
        emailMessage.AppendLine("</body>");
        emailMessage.AppendLine("</html>");

        var result= SendEmail(emailMessage.ToString());

        return result ? "Thank you for your registration, kindly check your email for confirmation code." : "There was an error.";
    }

    private bool SendEmail(string message)
    {
        try
        {
            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse("deshaun.cole@ethereal.email"));
            email.From.Add(MailboxAddress.Parse("deshaun.cole@ethereal.email"));
            email.Subject = "Email Confirmation";
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("deshaun.cole@ethereal.email", "3T6xQMPufreBw2arD7");
            smtp.Send(email);
            smtp.Disconnect(true);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}