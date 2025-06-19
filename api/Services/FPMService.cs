using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using System.Text.Json;

namespace api.Services
{
    public class FPMService : IFPMService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public FPMService(HttpClient client, IConfiguration config)
        {
            this._client = client;
            this._config = config;
        }
        public async Task<Stock?> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await this._client.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FPMKey"]}");
                if (result.IsSuccessStatusCode)
                {
                    var strContent = await result.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(strContent))
                    {
                        throw new HttpRequestException("No content found");
                    }

                    var tasks = JsonSerializer.Deserialize<FMPStock[]>(strContent);
                    var stock = tasks?.Length == 1 ? tasks[0] : null;
                    if(stock != null)
                    {
                        return stock.ToStockFromFMP();
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
