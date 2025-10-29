using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/portfolio")]
public class PortfolioController(
    UserManager<AppUser> userManager,
    IStockRepository stockRepo,
    IPortfolioRepository portfolioRepo) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);
        var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);
        var stock = await stockRepo.GetBySymbolAsync(symbol);

        if (stock == null) return BadRequest("Stock not found");
        var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Portfolio already exists");

        var portfolio = new Portfolio
        {
            AppUserId = appUser.Id,
            StockId = stock.Id
        };

        await portfolioRepo.CreateAsync(portfolio);

        return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);

        var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
        if (filteredStock.Count == 1)
            await portfolioRepo.DeletePortfolio(appUser, symbol);
        else
            return BadRequest("Stock not in your portfolio");

        return Ok();
    }
}