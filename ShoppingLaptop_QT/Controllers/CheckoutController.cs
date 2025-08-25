using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLaptop_QT.Models;
using ShoppingLaptop_QT.Repository;
using System.Security.Claims;

namespace ShoppingLaptop_QT.Controllers
{
	public class CheckoutController:Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext context)
		{
			_dataContext = context;
		}

		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login","Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				//TempData["success"] = "Đơn hàng đã tạo thành công";
				//return RedirectToAction("Index","Cart");
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Đơn hàng đã xác nhận thành công, Đang chờ duyệt";
				return RedirectToAction("Index","Cart");
			}
			return View();
		}

	}
}
