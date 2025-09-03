using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingLaptop_QT.Models
{
	public class RatingModel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public long ProductId { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập nội dung đánh giá")]
		public string Comment { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập tên")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập email")]
		//[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Yêu cầu chọn số sao")]
		//[Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
		public int Star { get; set; }

		[ForeignKey("ProductId")]
		public ProductModel Product { get; set; }

	}
}
