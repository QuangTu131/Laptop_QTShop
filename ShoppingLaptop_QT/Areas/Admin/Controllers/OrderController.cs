using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLaptop_QT.Models;
using ShoppingLaptop_QT.Repository;

namespace ShoppingLaptop_QT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Route("Admin/Order")]
    public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}

		[Route("Index")]
		public async Task<IActionResult> Index(int pg = 1)
		{
			List<OrderModel> orders = _dataContext.Orders.ToList(); //33 datas


			const int pageSize = 10; //10 items/trang

			if (pg < 1) //page < 1;
			{
				pg = 1; //page ==1
			}
			int recsCount = orders.Count(); //33 items;

			var pager = new Paginate(recsCount, pg, pageSize);

			int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

			//category.Skip(20).Take(10).ToList()

			var data = orders.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Pager = pager;

			return View(data);
		}


		public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _dataContext.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode == ordercode).ToListAsync();
            return View(DetailsOrder);
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully" });
            }
            catch (Exception)
            {


                return StatusCode(500, "An error occurred while updating the order status.");
            }
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string ordercode)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();
            }
            try
            {

                //delete order
                _dataContext.Orders.Remove(order);


                await _dataContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }



    }
}
