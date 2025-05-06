using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DemoLinkedIn.Server.DTOs.FeedDTOs;

public class CommentDto
{
    [Required]
    [MinLength(1)]
    public required string Content { get; set; }
}