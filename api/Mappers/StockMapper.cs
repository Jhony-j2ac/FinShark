using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto()
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                LastDiv = stock.LastDiv,
                Purchase = stock.Purchase,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStock createStock)
        {
            return new Stock()
            {
                Symbol = createStock.Symbol,
                CompanyName = createStock.CompanyName,
                LastDiv = createStock.LastDiv,
                Purchase = createStock.Purchase,
                Industry = createStock.Industry,
                MarketCap = createStock.MarketCap
            };
        }
    }
}
