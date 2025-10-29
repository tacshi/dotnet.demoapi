using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            CreatedOn = comment.CreatedOn,
            CreatedBy = comment.AppUser.UserName,
            StockId = comment.StockId
        };
    }

    public static Comment ToCommentModel(this CreateCommentDto dto, int stockId)
    {
        return new Comment
        {
            Title = dto.Title,
            Content = dto.Content,
            StockId = stockId
        };
    }

    public static Comment ToCommentModel(this UpdateCommentDto dto)
    {
        return new Comment
        {
            Title = dto.Title,
            Content = dto.Content
        };
    }
}