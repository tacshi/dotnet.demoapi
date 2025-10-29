using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository(ApplicationDbContext context) : IPortfolioRepository
{
    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        return await context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(p => new Stock
            {
                Id = p.StockId,
                Symbol = p.Stock.Symbol,
                CompanyName = p.Stock.CompanyName,
                Purchase = p.Stock.Purchase,
                LastDiv = p.Stock.LastDiv,
                Industry = p.Stock.Industry,
                MarketCap = p.Stock.MarketCap
            }).ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await context.Portfolios.AddAsync(portfolio);
        await context.SaveChangesAsync();
        return portfolio;
    }

    public async Task<Portfolio?> DeletePortfolio(AppUser appUser, string symbol)
    {
        var portfolio = await context.Portfolios.FirstOrDefaultAsync(x =>
            x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
        if (portfolio == null) return null;
        context.Portfolios.Remove(portfolio);
        await context.SaveChangesAsync();
        return portfolio;
    }
}