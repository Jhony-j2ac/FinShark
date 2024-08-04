using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolios(AppUser appUser);

        Task<Portfolio> CreatePortfolio(Portfolio portfolio);
        Task<Portfolio> DeletePorfolio(AppUser appUser, string symbol);
    }
}
