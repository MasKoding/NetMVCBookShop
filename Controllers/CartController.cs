using System;
using Microsoft.AspNetCore.Mvc;

namespace bookshop.Controllers
{
	public class CartController :Controller
	{


		private readonly ICartRepository _cartRepo;
		public CartController(ICartRepository cartRepository)
		{
			_cartRepo = cartRepository;
		}

		public async Task<IActionResult> AddItem(int bookId,int qty=1,int redirect = 0)
		{
			int countUser = await _cartRepo.AddItem(bookId, qty);
			if (redirect == 0)
                return Ok(countUser);
            return RedirectToAction("CartDetail");
        }

		public  async Task<IActionResult> RemoveItem(int bookId) {

			var cartCount = await _cartRepo.RemoveItem(bookId);
			
			return RedirectToAction("CartDetail");
		}

		public IActionResult GetUserCart()
		{
			var cart = _cartRepo.GetUserCart();
			return View(cart);
		}
		public async Task<IActionResult> GetTotalItemInCart()
		{
			int cartItem = await _cartRepo.GetCartItemCount();
			return Ok(cartItem);
		}

		public async Task<IActionResult> CartDetail()
		{

			IEnumerable<ShoppingCartDetail> cartDetailItem = await _cartRepo.GetCartDetails();

			ListShoppingCartDetail
			listShopping = new ListShoppingCartDetail
			{
				shoppingCartDetails = cartDetailItem
			};

			return View(listShopping);

        }

		public async Task<IActionResult> Checkout()
		{
			bool isCheckout = await _cartRepo.DoCheckout();
			if (!isCheckout)
			{
				throw new Exception("error checkout");
			}

			return RedirectToAction("Index", "Home");
		}


	}
}

