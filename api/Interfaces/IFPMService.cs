using api.Models;
using System;

namespace api.Interfaces
{
    public interface IFPMService
    {
        Task<Stock?> FindStockBySymbolAsync (string symbol);
    }

}
