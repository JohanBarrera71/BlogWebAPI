namespace DemoLinkedIn.Server.Responses.FeedResponses;

public class CommentInfoResponse
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; }
    public string UserId { get; set; } = null!; // Foreign key
    public UserInfoResponse User { get; set; } = null!; // Navigation property
    public int PostId { get; set; } // Foreign key
    public PostResponse Post { get; set; } = null!; // Navigation property
}