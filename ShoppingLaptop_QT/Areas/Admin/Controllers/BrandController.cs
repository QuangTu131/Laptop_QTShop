using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingLaptop_QT.Repository;
using ShoppingLaptop_QT.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;


namespace ShoppingLaptop_QT.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
    [Route("Admin/Brand")]
    public class BrandController : Controller
	{

		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
		[Route("Index")]
		public async Task<IActionResult> Index(int pg = 1)
		{
			List<BrandModel> brands = _dataContext.Brands.ToList(); //33 datas


			const int pageSize = 10; //10 items/trang

			if (pg < 1) //page < 1;
			{
				pg = 1; //page ==1
			}
			int recsCount = brands.Count(); //33 items;

			var pager = new Paginate(recsCount, pg, pageSize);

			int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

			//category.Skip(20).Take(10).ToList()

			var data = brands.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Pager = pager;

			return View(data);
		}

		[Route("Create")]
        public IActionResult Create()
		{
			return View();
		}

        [Route("Create")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BrandModel brand)
		{
			
			if (ModelState.IsValid)
			{
				// Code them du lieu
				brand.Slug = brand.Name.Replace(" ", "-");
				var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Thương hiệu đã có trong Database");
					return View(brand);
				}
				_dataContext.Add(brand);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm Thương hiệu thành công";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model có một số thứ đang bị lỗi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}
			return View(brand);
		}

		[Route("Edit")]
		public async Task<IActionResult> Edit(int Id)
		{
			BrandModel brand = await _dataContext.Brands.FindAsync(Id);
			return View(brand);
		}

		[Route("Edit")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(BrandModel brand)
		{

			if (ModelState.IsValid)
			{
				// Code them du lieu
				brand.Slug = brand.Name.Replace(" ", "-");
				var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Thương hiệu đã có trong Database");
					return View(brand);
				}
				_dataContext.Update(brand);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật thương hiệu thành công";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model có một số thứ đang bị lỗi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}
			return View(brand);
		}

		[Route("Delete")]
		public async Task<IActionResult> Delete(int Id)
		{
			BrandModel brand = await _dataContext.Brands.FindAsync(Id);
			
			_dataContext.Brands.Remove(brand);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Thương hiệu đã xóa thành công";
			return RedirectToAction("Index");

		}

	}
}
