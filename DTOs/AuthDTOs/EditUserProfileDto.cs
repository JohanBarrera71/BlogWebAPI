namespace DemoLinkedInApi.DTOs;

public class EditUserProfileDto
{
    public IFormFile? ProfilePicture { get; set; }
    public IFormFile? CoverImage { get; set; }
    public string? FirstName { get; set; }
    public string? LastNameName { get; set; }
    public string? Headline { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Website { get; set; }
    public string? About { get; set; }
    public string? BirthDay { get; set; }
}