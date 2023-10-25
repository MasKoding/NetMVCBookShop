using System;
namespace bookshop.Repositories
{
	public interface IOrderRepository
	{
        Task<IEnumerable<Order>> UserOrders();

    }
}

