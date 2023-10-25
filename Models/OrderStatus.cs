using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookshop.Models
{
	[Table("OrderStatus")]
	public class OrderStatus
	{
		public int Id{ get;set;}

		public int StatusId { get; set; }

		[Required,MaxLength(20)]
		public string? StatusName { get; set; }



	}
}

