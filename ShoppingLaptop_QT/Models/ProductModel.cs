using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoppingLaptop_QT.Repository.Validation;

namespace ShoppingLaptop_QT.Models
{
    public class ProductModel
    {
		[Key]
		public long Id { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
		public string Name { get; set; }
		public string Slug { get; set; }


		[Required(ErrorMessage = "Yêu cầu nhập mô tả Sản phẩm")]
		public string Description { get; set; }


		[Required(ErrorMessage = "Yêu cầu nhập giá Sản phẩm")]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		// [Required(ErrorMessage = "Yêu cầu chọn ảnh")]
		public string Image { get; set; } 

		[Required, Range(1,int.MaxValue, ErrorMessage = "Yêu cầu chọn một thương hiệu")]
		public int BrandID { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn một doanh mục")]
		public int CategoryID { get; set; }

		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }

		public RatingModel Ratings { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }

	}
}
