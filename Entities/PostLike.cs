using DemoLinkedInApi.Entities;

namespace DemoLinkedIn.Server.Entities
{
    public class PostLike
    {
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
