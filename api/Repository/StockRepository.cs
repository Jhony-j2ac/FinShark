using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock?> AddAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FindAsync(id);
            if (stockModel == null) return null;

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            var result = _context.Stocks.AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObject.CompanyName))
                result = result.Where(x => x.CompanyName == queryObject.CompanyName);

            if(!string.IsNullOrWhiteSpace(queryObject.Symbol))
                result = result.Where(x => x.Symbol== queryObject.Symbol);


            var Listado = await result.Include(x => x.Comments).ToListAsync();
            if (!string.IsNullOrWhiteSpace(queryObject.OrderBy)) { 
                var prop = typeof(Stock).GetProperty(queryObject.OrderBy);
                if (prop == null) throw new Exception("Campo orden invalido");
                
                if (!queryObject.IsDescending) {
                    Listado = Listado.OrderBy(x => prop.GetValue(x)).ToList();
                }
                else
                {
                    Listado = Listado.OrderByDescending(x => prop.GetValue(x)).ToList();
                }

            }

            return Listado;
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id== id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol== symbol);
        }

        public async  Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            var stockModel = await GetByIdAsync(id);
            if(stockModel == null ) return null;

            stockModel.Symbol = stock.Symbol;
            stockModel.CompanyName = stock.CompanyName;
            stockModel.Purchase = stock.Purchase;
            stockModel.Industry = stock.Industry;
            stockModel.LastDiv = stock.LastDiv;
            stockModel.MarketCap = stock.MarketCap;

            await _context.SaveChangesAsync();

            return stockModel;
        }

 
    }
}
