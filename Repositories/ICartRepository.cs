using System;
namespace bookshop.Repositories
{
	public interface ICartRepository
	{
       Task<int> AddItem(int bookId, int qty);

       Task<int> RemoveItem(int bookId);

       Task<ShoppingCart> GetUserCart();

       Task<ShoppingCart> GetCart(string userId);

       Task<int> GetCartItemCount(string userId = "");

       Task<IEnumerable<ShoppingCartDetail>> GetCartDetails();

       Task<bool> DoCheckout();
    }
}

