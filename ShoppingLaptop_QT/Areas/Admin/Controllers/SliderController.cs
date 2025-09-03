using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLaptop_QT.Models;
using Microsoft.EntityFrameworkCore
using ShoppingLaptop_QT.Repository;

namespace ShoppingLaptop_QT.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/Slider")]
	[Authorize(Roles = "Admin")]
	public class SliderController : Controller
	{
		private readonly DataContext _dataContext;
		public SliderController(DataContext context)
		{
			_dataContext = context;
		}
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Sliders.OrderByDescending(p => p.Id).ToListAsync());
		}


	}

	
}
