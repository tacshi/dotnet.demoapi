using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment;

public class CreateCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
    [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Content must be at least 5 characters long")]
    [MaxLength(255, ErrorMessage = "Content cannot exceed 255 characters")]
    public string Content { get; set; } = string.Empty;
}