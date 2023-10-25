using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookshop.Models
{

	[Table("ShoppingCart")]
	public class ShoppingCart
	{
		public int Id {	get;set;}

		[Required]
		public string UserId { get; set; }

		public bool IsDeleted { get; set; } = false;

		public ICollection<CartDetail> cartDetails { get; set; }

	}
}

