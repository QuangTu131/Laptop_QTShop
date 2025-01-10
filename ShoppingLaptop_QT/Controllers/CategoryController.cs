using Microsoft.AspNetCore.Mvc;

namespace ShoppingLaptop_QT.Controllers
{
    public class CategoryController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
