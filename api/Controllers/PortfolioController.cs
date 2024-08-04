
using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/Porfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _porfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _porfolioRepository = portfolioRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserPorfolio()
        {
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _porfolioRepository.GetUserPortfolios(appUser);
            return Ok(userPortfolio);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromBody] string symbol)
        {
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null) return BadRequest("Stock not found");

            var userPorfolio = await _porfolioRepository.GetUserPortfolios(appUser);

            if(userPorfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot same stock to portfolio");
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };

            await _porfolioRepository.CreatePortfolio(portfolioModel);

            if(portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
                    
            return Ok("Created");
        }

    }
}
