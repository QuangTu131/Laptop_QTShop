using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLaptop_QT.Models;
using ShoppingLaptop_QT.Models.ViewModels;
using ShoppingLaptop_QT.Repository;

namespace ShoppingLaptop_QT.Controllers
{
    public class ProductController:Controller
    {
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _dataContext.Products
			.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
			.ToListAsync();

			ViewBag.Keyword = searchTerm;

			return View(products);
		}

		public async Task<IActionResult> Details(int Id)
		{
			if (Id == null) return RedirectToAction("Index");

			var productsById = _dataContext.Products.
				Include(p => p.Ratings).
				Where(p => p.Id == Id).FirstOrDefault(); //category = 4
														 //related product


			var relatedProducts = await _dataContext.Products
			.Where(p => p.CategoryID == productsById.CategoryID && p.Id != productsById.Id)
			.Take(4)
			.ToListAsync();

			ViewBag.RelatedProducts = relatedProducts;

			var viewModel = new ProductDetailsViewModel
			{
				ProductDetails = productsById,

			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> CommentProduct(RatingModel rating)
		{
			if (ModelState.IsValid)
			{
				var ratingEntity = new RatingModel
				{
					ProductId = rating.ProductId,
					Name = rating.Name,
					Email = rating.Email,
					Comment = rating.Comment,
					Star = rating.Star
				};

				_dataContext.Ratings.Add(ratingEntity);
				await _dataContext.SaveChangesAsync();

				TempData["success"] = "Thêm đánh giá thành công";
				return Redirect(Request.Headers["Referer"].ToString());
			}

			// Nếu model lỗi
			var errors = ModelState.Values
								   .SelectMany(v => v.Errors)
								   .Select(e => e.ErrorMessage)
								   .ToList();

			TempData["error"] = string.Join("<br/>", errors);

			return RedirectToAction("Detail", new { id = rating.ProductId });
		}

	}
}
