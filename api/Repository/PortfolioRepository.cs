using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;

        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);

            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePorfolio(AppUser appUser, string symbol)
        {
            var porfolio =  await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id
            && symbol.ToLower() == x.Stock.Symbol.ToLower() );

            if (porfolio == null) return null;
            _context.Portfolios.Remove(porfolio);
            await _context.SaveChangesAsync();
            return porfolio;
        }

        public async Task<List<Stock>> GetUserPortfolios(AppUser appUser)
        {
            return await _context.Portfolios
                .Where(x => x.AppUserId == appUser.Id)
                .Select(x => new Stock
                {
                    Id = x.StockId,
                    Symbol = x.Stock.Symbol,
                    Comments = x.Stock.Comments,
                    CompanyName = x.Stock.CompanyName,
                    Industry = x.Stock.Industry,
                    LastDiv = x.Stock.LastDiv,
                    MarketCap = x.Stock.MarketCap,
                    Purchase = x.Stock.Purchase
                })
                .ToListAsync();
        }
    }
}
