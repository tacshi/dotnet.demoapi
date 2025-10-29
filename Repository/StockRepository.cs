using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository(ApplicationDbContext context) : IStockRepository
{
    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocksQuery = context.Stocks
            .Include(s => s.Comments)
            .ThenInclude(a => a.AppUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
            stocksQuery = stocksQuery.Where(s => s.Symbol.Contains(query.Symbol));

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
            stocksQuery = stocksQuery.Where(s => s.CompanyName.Contains(query.CompanyName));

        if (!string.IsNullOrWhiteSpace(query.SortBy))
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stocksQuery = query.IsDescending
                    ? stocksQuery.OrderByDescending(s => s.Symbol)
                    : stocksQuery.OrderBy(s => s.Symbol);

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stocksQuery.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await context.Stocks
            .Include(s => s.Comments)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
        await context.Stocks.AddAsync(stock);
        await context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto dto)
    {
        var existingStock = await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStock == null) return null;
        existingStock.Symbol = dto.Symbol;
        existingStock.CompanyName = dto.CompanyName;
        existingStock.Purchase = dto.Purchase;
        existingStock.LastDiv = dto.LastDiv;
        existingStock.Industry = dto.Industry;
        existingStock.MarketCap = dto.MarketCap;
        await context.SaveChangesAsync();
        return existingStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stock = await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (stock == null) return null;
        context.Stocks.Remove(stock);
        await context.SaveChangesAsync();
        return stock;
    }

    public Task<bool> StockExists(int id)
    {
        return context.Stocks.AnyAsync(s => s.Id == id);
    }
}