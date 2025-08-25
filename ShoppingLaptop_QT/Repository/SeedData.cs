using Microsoft.EntityFrameworkCore;
using ShoppingLaptop_QT.Models;
using System;

namespace ShoppingLaptop_QT.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			//_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is Large Product in the world", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "PC", Slug = "pc", Description = "PC is Large Product in the world", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is Large Brand in the world", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "SamSung", Slug = "samsung", Description = "Samsung is Large Brand in the world", Status = 1 };

				_context.Products.AddRange(
					new ProductModel { Name = "MacBook", Slug = "macbook", Description = "MacBook is the Best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 123 },
					new ProductModel { Name = "PC", Slug = "pc", Description = "PC is the Best", Image = "1.jpg", Category = pc, Brand = samsung, Price = 123 }
				);
				_context.SaveChanges();
			}
		}
	}
}
