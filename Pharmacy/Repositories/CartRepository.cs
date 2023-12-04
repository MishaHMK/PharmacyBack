using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Entities;

namespace Pharmacy.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
           UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(string userId, int prodId, int qty)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    _db.Carts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == prodId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.Products.Find(prodId);
                    cartItem = new CartDetail
                    {
                        ProductId = prodId,
                        CartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = product.Price  // it is a new line after update
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<bool> DoCheckout(string userId)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.CartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    PharmacistId = "",
                    CustomerId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Carts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.CartId
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }


        public async Task<Cart> GetUserCart(string userId)
        {
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _db.Carts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  //.ThenInclude(a => a.Genre)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();

            return shoppingCart;
        }

        public async Task<int> RemoveItem(string userId, int prodId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == prodId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
