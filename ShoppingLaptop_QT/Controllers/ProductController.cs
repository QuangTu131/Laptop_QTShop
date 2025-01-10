using Microsoft.AspNetCore.Mvc;

namespace ShoppingLaptop_QT.Controllers
{
    public class ProductController:Controller
    {
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Details()
		{
			return View();
		}
	}
}
