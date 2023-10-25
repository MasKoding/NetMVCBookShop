using System;
using Microsoft.AspNetCore.Identity;

namespace bookshop.Repositories
{
	public class CartRepository :ICartRepository
	{
		private	 ApplicationDbContext _db;
		private UserManager<IdentityUser> _userManager;
		public IHttpContextAccessor _contextAccessor;

		public CartRepository(ApplicationDbContext db,UserManager<IdentityUser> userManager,
			IHttpContextAccessor contextAccessor
			)
		{
			_db = db;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
		}

		public async Task<int> AddItem(int bookId,int qty)
		{
            string userId = GetUserId();

            using var transaction = _db.Database.BeginTransaction();
			try
			{
                

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Invalid User");
                }


                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                        cartDetails = null
                    };

                    _db.ShoppingCarts.Add(cart);

                }
                _db.SaveChanges();
                var item = await _db.CartDetails.FirstOrDefaultAsync(x => x.ShoppingCartId == cart.Id && x.BookId == bookId);

                if(item is not null) {
					item.Quantity += qty;
				}
				else
				{
					item = new CartDetail
					{
						BookId = bookId,
						ShoppingCartId = cart.Id,
						Quantity = qty
					};

					 _db.CartDetails.Add(item);
				}
				_db.SaveChanges();
				transaction.Commit();

               
            }
			catch (Exception ex)
			{
                throw ex;
			}

            var GetCartCount = await GetCartItemCount(userId);
            return GetCartCount;



        }



        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            //using var transaction = _db.Database.BeginTransaction();
            try
            {
             

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Invalid user");
                }


                var cart = await GetCart(userId);

                if (cart is null)
                {
                    throw new Exception("Cart is empty");
                }
                _db.SaveChanges();
                var item = _db.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.Id && x.BookId == bookId);

                if (item is null)
                {
                    throw new Exception("Cart Item is empty");
                }
                else if(item.Quantity ==1)
                {
                    _db.CartDetails.Remove(item);
                }
                else
                {
                    item.Quantity = item.Quantity - 1;
                }
                _db.SaveChanges();
             

            }
            catch (Exception ex)
            {
                throw ex;
            }

            var GetCartCount = await GetCartItemCount(userId);
            return GetCartCount;

        }


        public async Task<ShoppingCart> GetUserCart() {
            var userId = GetUserId();

            if(userId is null)
            {
                throw new Exception("Invalid User");
            }

            var shoppingCart = await _db.ShoppingCarts.Include(x => x.cartDetails)
                                .ThenInclude(a => a.Book)
                                .ThenInclude(a => a.Genre)
                                .Where(x => x.UserId == userId)
                                .FirstOrDefaultAsync();

            return shoppingCart;

        }


        public async Task<ShoppingCart> GetCart(string userId) {

			var result = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
			return result;
		}

		public string GetUserId()
		{
			var principal = _contextAccessor.HttpContext.User;
			var userId = _userManager.GetUserId(principal);


			return userId;  
		}

        public async Task<int> GetCartItemCount(string userId="")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId =  GetUserId();
            }

            var data = await (from cart in _db.ShoppingCarts
                        join cartDetail in _db.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        select new { cartDetail.Id }).ToListAsync();
            return data.Count;

        }

       
        public async Task<IEnumerable<ShoppingCartDetail>> GetCartDetails()
        {

            var userId = GetUserId();
            var data = await (from cartDetail in _db.CartDetails
                              join book in _db.Books
                                on cartDetail.BookId equals book.Id
                              join shoppingCart in _db.ShoppingCarts
                              on cartDetail.ShoppingCartId equals shoppingCart.Id
                              where shoppingCart.UserId == userId
                              select new ShoppingCartDetail
                              {
                                  Id = shoppingCart.Id,
                                  BookName= book.BookName,
                                  BookId = book.Id,
                                  AuthorName = book.AuthorName,
                                  Image = book.Image,
                                  Price = book.Price,
                                  Quantity = cartDetail.Quantity

                              }
                                  ).ToListAsync();

            return data;
          
        }          

        public async Task<bool> DoCheckout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not null");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Cart is null");
                }

                var cartDetail = await _db.CartDetails.Where(x => x.ShoppingCartId == cart.Id).ToListAsync();

                if (cartDetail.Count() == 0)
                {
                    throw new Exception("Cart detail is empty");
                }



                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };

                _db.Orders.Add(order);
                _db.SaveChanges();

                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = 3000

                    };

                    _db.OrderDetails.Add(orderDetail);

                }

                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

           

  


        }

    }



}

