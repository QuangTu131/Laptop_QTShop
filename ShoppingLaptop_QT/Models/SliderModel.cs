using ShoppingLaptop_QT.Repository.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingLaptop_QT.Models
{
	public class SliderModel
	{
		public int Id { get; set; }

		public string Image { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; }
	}
}
