using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController(
    ICommentRepository commentRepo,
    IStockRepository stockRepo,
    UserManager<AppUser> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await commentRepo.GetAllAsync();
        var commentsDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await commentRepo.GetByIdAsync(id);
        if (comment == null) return NotFound();

        return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!await stockRepo.StockExists(stockId)) return BadRequest("Stock does not exist");

        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);

        var comment = dto.ToCommentModel(stockId);
        comment.AppUserid = appUser.Id;
        await commentRepo.CreateAsync(comment);
        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var comment = await commentRepo.UpdateAsync(id, dto.ToCommentModel());
        if (comment == null) return NotFound("Comment not found");

        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var comment = await commentRepo.DeleteAsync(id);
        if (comment == null) return NotFound("Comment not found");
        return Ok(comment.ToCommentDto());
    }
}