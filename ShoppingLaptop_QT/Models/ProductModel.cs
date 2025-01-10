﻿using System.ComponentModel.DataAnnotations;

namespace ShoppingLaptop_QT.Models
{
    public class ProductModel
    {
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Sản phẩm")]
		public string Description { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập giá Sản phẩm")]
		public decimal Price { get; set; }
		public int BrandID { get; set; }
		public int CategoryID { get; set; }

		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
	}
}
