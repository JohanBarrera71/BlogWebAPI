using System.ComponentModel.DataAnnotations;

namespace DemoLinkedIn.Server.DTOs.FeedDTOs
{
    public class PostDTO
    {
        [Required]
        public required string Content { get; set; }
        public string? Picture { get; set; }
    }
}
