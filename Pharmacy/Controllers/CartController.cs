using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Pharmacy.Entities;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Repositories;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        // Post: api/Cart/add/id
        [HttpPost("add/{userId}/{prodId}")]
        public async Task<IActionResult> AddProduct(string userId, int prodId, int qty = 1)
        {
            var cartCount = await _cartRepo.AddItem(userId, prodId, qty);

            return Ok(cartCount);
        }

        // DELETE api/Cart/Delete/id
        [HttpDelete]
        [Route("Delete/{userId}/{prodId}")]
        public async Task<IActionResult> RemoveItem(string userId, int prodId)
        {
            await _cartRepo.RemoveItem(userId, prodId);
            return Ok("Removed");
        }


        // GET api/Cart/User/id
        [HttpGet]
        [Route("User/{userId}")]
        public async Task<IActionResult> GetUserCart(string userId)
        {
            var cart = await _cartRepo.GetUserCart( userId);
            return Ok(cart);
        }

        [HttpGet]
        [Route("count/{userId}")]
        public async Task<IActionResult> GetTotalItemInCart(string userId)
        {
            int totalItem = await _cartRepo.GetCartItemCount(userId);
            return Ok(totalItem);
        }

        [HttpPost]
        [Route("checkout/{userId}")]
        public async Task<IActionResult> Checkout(string userId)
        {
            bool isCheckedOut = await _cartRepo.DoCheckout(userId);
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            return Ok();
        }       
    }
}
