using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingLaptop_QT.Models.ViewModels
{
	public class CartItemViewModel
	{
		public List<CartItemModel> CartItems { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal GrandTotal { get; set; }
	}
}
