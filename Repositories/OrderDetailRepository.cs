using System;
using Microsoft.AspNetCore.Identity;

namespace bookshop.Repositories
{
	public class OrderDetailRepository
	{
		private ApplicationDbContext _db;
		private UserManager<IdentityUser> _userManager;

		public OrderDetailRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager) {
			this._db = db;
			this._userManager = userManager;
		}

			

	}
}

