using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
    public async Task<List<Comment>> GetAllAsync()
    {
        return await context.Comments.Include(a => a.AppUser).ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var existingComment = await context.Comments.FindAsync(id);
        if (existingComment == null) return null;
        existingComment.Title = comment.Title;
        existingComment.Content = comment.Content;
        await context.SaveChangesAsync();
        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null) return null;
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
        return comment;
    }
}