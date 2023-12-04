using Microsoft.AspNetCore.Mvc;
using Pharmacy.Entities;
using Pharmacy.Helpers;
using Pharmacy.Repositories;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrderController : ControllerBase
    {
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }


        // Get: api/UserOrder
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> UserOrders(string userId)
        {
            var orders = await _userOrderRepo.UserOrders(userId);
            return Ok(orders);
        }

        // Get: api/UserOrder
        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepo.AllUserOrders();
            return Ok(orders);
        }

        // Get: api/UserOrder/pagedUserOrders
        [HttpGet("pagedUserOrders")]
        public async Task<IActionResult> GetUserOrdersPaged([FromQuery] OrderParams userParams)
        {
            var orderList = await _userOrderRepo.GetUserOrdersPagedList(userParams);
            var responce = new PaginationHeader<OrderResponce>(orderList, userParams.PageNumber, userParams.PageSize, orderList.TotalCount);
            return Ok(responce);
        }

        // Get: api/UserOrder/pagedAllOrders
        [HttpGet("pagedAllOrders")]
        public async Task<IActionResult> GetAllOrdersPaged([FromQuery] OrderParams userParams)
        {
            var orderList = await _userOrderRepo.GetAllOrdersPagedList(userParams);
            var responce = new PaginationHeader<OrderResponce>(orderList, userParams.PageNumber, userParams.PageSize, orderList.TotalCount);
            return Ok(responce);
        }

        // PATCH  api/UserOrder/Approve/id
        [HttpPatch]
        [Route("Approve/{id}/{status}")]
        public async Task<IActionResult> ApproveAppointmentById(int id, string status)
        {
            var orderToUpdate = await _userOrderRepo.СhangeOrderStatusById(id, status);

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
