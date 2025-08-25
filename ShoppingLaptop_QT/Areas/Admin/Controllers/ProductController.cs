using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingLaptop_QT.Repository;
using ShoppingLaptop_QT.Models;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingLaptop_QT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Route("Admin/Product")]
    public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DataContext context,IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}


		[Route("Index")]
		public async Task<IActionResult> Index(int pg = 1)
		{
            List<ProductModel> products = _dataContext.Products
			 .Include(p => p.Category)
			 .Include(p => p.Brand)
			 .ToList();
            //33 datas


            const int pageSize = 10; //10 items/trang

			if (pg < 1) //page < 1;
			{
				pg = 1; //page ==1
			}
			int recsCount = products.Count(); //33 items;

			var pager = new Paginate(recsCount, pg, pageSize);

			int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

			//category.Skip(20).Take(10).ToList()

			var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Pager = pager;

			return View(data);
		}

		[Route("Create")]
		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
			return View();
		}

		[Route("Create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name",product.CategoryID);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name",product.BrandID);
			if (ModelState.IsValid)
			{
				// Code them du lieu
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if (slug!=null)
				{
					ModelState.AddModelError("", "Sản phẩm đã có trong Database");
					return View(product);
				}
				
				if (product.ImageUpload!=null)
				{
					string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadsDir, imageName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					product.Image = imageName;
				}
				
				_dataContext.Add(product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm sản phẩm thành công";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model có một số thứ đang bị lỗi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach(var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n",errors);
				return BadRequest(errorMessage);
			}		
			return View(product);
		}

		[Route("Edit")]
		public async Task<IActionResult> Edit(long Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryID); 
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandID);
			return View(product);
		}


		[Route("Edit")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(long Id ,ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryID);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandID);
			
			var existed_product = _dataContext.Products.Find(product.Id); //tim sp theo id product
			if (ModelState.IsValid)
			{
				
				product.Slug = product.Name.Replace(" ", "-");
				//var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				//if (slug != null)
				//{
				//	ModelState.AddModelError("", "Sản phẩm đã có trong Database");
				//	return View(product);
				//}

				if (product.ImageUpload != null)
				{
					string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadsDir, imageName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					existed_product.Image = imageName;
				}
				// Update other product properties
				existed_product.Name = product.Name;
				existed_product.Description = product.Description;
				existed_product.Price = product.Price;
				existed_product.CategoryID = product.CategoryID;
				existed_product.BrandID = product.BrandID;
				// ... other properties

				_dataContext.Update(existed_product); // update the existed product object
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật sản phẩm thành công";
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
			return View(product);
		}

		[Route("Delete")]
		public async Task<IActionResult> Delete(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			if (!string.Equals(product.Image, "noname.jpg"))
			{
				string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
				string oldfileImage = Path.Combine(uploadsDir, product.Image);
				if (System.IO.File.Exists(oldfileImage))
				{
					System.IO.File.Delete(oldfileImage);
				}
			}	
			_dataContext.Products.Remove(product); 
			await _dataContext.SaveChangesAsync(); 
			TempData["error"] = "Sản phẩm đã xóa"; 
			return RedirectToAction("Index");
		}

	}
}
