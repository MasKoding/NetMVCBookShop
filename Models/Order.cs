using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookshop.Models
{
	[Table("Order")]
	public class Order
	{
			public int Id { get; set; }

			public string UserId { get; set; }

		    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

			[Required]
			public int OrderStatusId { get; set; }

			public OrderStatus OrderStatus { get; set; }

			public List<OrderDetail> OrderDetails { get; set; }
	}	
}

