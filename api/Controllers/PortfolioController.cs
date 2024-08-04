using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
