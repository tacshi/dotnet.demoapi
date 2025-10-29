using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController(IStockRepository repo) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        var stocks = await repo.GetAllAsync(query);
        var stocksDto = stocks.Select(s => s.ToStockDto()).ToList();
        return Ok(stocksDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await repo.GetByIdAsync(id);
        if (stock == null) return NotFound();
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stock = dto.ToStockModel();
        await repo.CreateAsync(stock);
        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stock = await repo.UpdateAsync(id, dto);
        if (stock == null) return NotFound();

        return Ok(stock.ToStockDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await repo.DeleteAsync(id);
        if (stock == null) return NotFound();

        return NoContent();
    }
}