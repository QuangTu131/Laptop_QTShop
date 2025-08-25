﻿using System.ComponentModel.DataAnnotations;

namespace ShoppingLaptop_QT.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập Username")]
		public string Username { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập Password")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
