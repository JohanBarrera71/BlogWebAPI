using DemoLinkedInApi.Entities;

namespace DemoLinkedIn.Server.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Picture { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Reference to the User
        public string UserId { get; set; } = null!; // Foreign key
        public ApplicationUser User { get; set; } = null!; // Navigation property

        // Reference to the Comments
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Reference to the Likes
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    }
}