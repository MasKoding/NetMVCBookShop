using System;
using Microsoft.AspNetCore.Identity;

namespace bookshop.Repositories
{
	public class OrderRepository :IOrderRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly HttpContextAccessor _contextAccessor;
		private readonly UserManager<IdentityUser> _userManager;

		public OrderRepository(ApplicationDbContext db,UserManager<IdentityUser> userManager,
			HttpContextAccessor contextAccessor
			)
		{
			_db = db;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
		}

		public async Task<IEnumerable<Order>> UserOrders()
		{
			var userId = GetUserId();

			if (string.IsNullOrEmpty(userId))
			{
				throw new Exception("User is null");
			}

			var orders = await _db.Orders
							.Include(x => x.OrderStatus)
							.Include(x => x.OrderDetails)
							.ThenInclude(x => x.Book)
							.ThenInclude(x => x.Genre)
							.Where(x => x.UserId == userId)
							.ToListAsync();
			return orders;
						
		}

		public string GetUserId() {
			var user = _contextAccessor.HttpContext.User;
			var userId = _userManager.GetUserId(user);

			return userId;

		}


	}
}

