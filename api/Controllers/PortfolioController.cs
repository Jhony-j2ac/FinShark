
using api.Extensions;
using api.Interfaces;
using api.Models;
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
        private readonly IStockRepository _stockRepo;
        private readonly IFPMService _fpmService;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository, IFPMService fpmService, IStockRepository stockRepo)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _porfolioRepository = portfolioRepository;
            _fpmService = fpmService;
            _stockRepo = stockRepo;
        }

        [HttpGet]
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

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock is null)
            {
                stock = await this._fpmService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock not found on fpm");
                }
                await this._stockRepo.AddAsync(stock);

            }

            var userPorfolio = await _porfolioRepository.GetUserPortfolios(appUser);

            if (userPorfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot same stock to portfolio");
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };

            await _porfolioRepository.CreatePortfolio(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }

            return Ok("Created");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio([FromBody] string symbol)
        {
            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);

            var portfolios = await _porfolioRepository.GetUserPortfolios(appUser);

            var porfolio = portfolios?.Where(x => x.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (porfolio.Count != 1) return BadRequest("Porfolio cannot be deleted");

            var portfolio = await _porfolioRepository.DeletePorfolio(appUser,  symbol);

            if(porfolio == null) return BadRequest("Porfolio to delete not found");
            return Ok(porfolio);
        }

    }
}
