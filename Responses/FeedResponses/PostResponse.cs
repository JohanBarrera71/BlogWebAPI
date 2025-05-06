using DemoLinkedIn.Server.Entities;

namespace DemoLinkedIn.Server.Responses.FeedResponses
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Picture { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; }

        public UserInfoResponse User { get; set; } = null!;

        // Reference to the Comments
        public ICollection<CommentInfoResponse> Comments { get; set; } = new List<CommentInfoResponse>();

        // Reference to the Likes
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        
        public int CommentsCount { get; set; } = 0;
        public int LikesCount { get; set; } = 0;

    }
}
