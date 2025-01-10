using Microsoft.EntityFrameworkCore;
using ShoppingLaptop_QT.Models;

namespace ShoppingLaptop_QT.Repository
{
	public class DataContext:DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
	}
}
