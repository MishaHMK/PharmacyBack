using Pharmacy.Entities;

namespace Pharmacy.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(string userId, int prodId, int qty);
        Task<int> RemoveItem(string userId,int prodId);
        Task<Cart> GetUserCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<Cart> GetCart(string userId);
        Task<bool> DoCheckout(string userId);
    }
}
