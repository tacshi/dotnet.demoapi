using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stock)
    {
        return new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            LastDiv = stock.LastDiv,
            Industry = stock.Industry,
            MarketCap = stock.MarketCap,
            Comments = stock.Comments.Select(c => c.ToCommentDto()).ToList()
        };
    }

    public static Stock ToStockModel(this CreateStockDto dto)
    {
        return new Stock
        {
            Symbol = dto.Symbol,
            CompanyName = dto.CompanyName,
            Purchase = dto.Purchase,
            LastDiv = dto.LastDiv,
            Industry = dto.Industry,
            MarketCap = dto.MarketCap
        };
    }
}