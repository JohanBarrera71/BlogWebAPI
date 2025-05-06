using DemoLinkedInApi.Entities;

namespace DemoLinkedIn.Server.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Refrence to the Post
        public int? PostId { get; set; } // Foreign key
        public Post? Post { get; set; } // Navigation property

        // Reference to the User
        public string? UserId { get; set; } // Foreign key
        public ApplicationUser? User { get; set; } // Navigation property
    }
}
