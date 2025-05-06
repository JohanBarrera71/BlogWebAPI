namespace DemoLinkedIn.Server.Responses
{
    public class UserInfoResponse
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? Headline { get; set; }
        public string? ProfilePicture { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Position { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Website { get; set; }
        public string? CoverImage { get; set; }
        public string? About { get; set; }
        public DateOnly? Birthday { get; set; }
        public bool ProfileComplete { get; set; }
    }
}
