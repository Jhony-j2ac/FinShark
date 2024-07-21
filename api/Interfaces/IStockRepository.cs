using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock?> AddAsync(Stock stockDto);

        Task<Stock?> UpdateAsync(int id, Stock stockDto);

        Task<Stock?> DeleteAsync(int id);
    }
}
