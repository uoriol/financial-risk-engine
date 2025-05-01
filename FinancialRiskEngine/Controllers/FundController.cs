using Microsoft.AspNetCore.Mvc;

namespace FinancialRiskEngine.Controllers
{
    public class FundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
