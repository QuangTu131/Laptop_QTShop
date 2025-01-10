using System.ComponentModel.DataAnnotations;

namespace ShoppingLaptop_QT.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required,MinLength(4,ErrorMessage ="Yêu cầu nhập tên Doanh mục")]
        public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Doanh mục")]
		public string Description { get; set; }
		public string Slug { get; set; }
        public int Status { get; set; }
	}
}
