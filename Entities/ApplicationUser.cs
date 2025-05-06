using System.ComponentModel.DataAnnotations;
using DemoLinkedIn.Server.Entities;
using Microsoft.AspNetCore.Identity;

namespace DemoLinkedInApi.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(150)]
        public string? Headline { get; set; }
        [MaxLength(100)]
        public string? Company { get; set; }
        [MaxLength(100)]
        public string? Position { get; set; }
        [MaxLength(100)]
        public string? Country { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(250)]
        public string? Website { get; set; }
        public bool ProfileComplete { get; set; } = false;
        [MaxLength(500)]
        public string? ProfilePicture { get; set; }
        [MaxLength(500)]
        public string? CoverImage { get; set; }
        [MaxLength(2600)]
        public string? About { get; set; }
        public DateOnly? Birthday { get; set; }
        public bool IsActive { get; set; } = true;
        // Reference to the Posts
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        // Reference to the Comments
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Reference to the Likes
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    }
}
