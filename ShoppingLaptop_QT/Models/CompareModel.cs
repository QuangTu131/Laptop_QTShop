using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingLaptop_QT.Models
{
	public class CompareModel
	{
		[Key]
		public int Id { get; set; }

		public long ProductId { get; set; }

		public string UserId { get; set; }

		[ForeignKey("ProductId")]
		public ProductModel Product { get; set; }

	}
}
