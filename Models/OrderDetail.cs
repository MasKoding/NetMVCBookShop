﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookshop.Models
{
	[Table("OrderDetail")]
	public class OrderDetail
	{
		public int Id { get; set; }
		[Required]
		public int OrderId { get; set; }

		[Required]
		public	int BookId { get; set; }

		public int Quantity { get; set; }

		public double UnitPrice { get; set; }

		public Order Order { get; set; }

        public Book Book { get; set; }



    }
}

