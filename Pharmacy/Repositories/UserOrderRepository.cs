using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Entities;
using Pharmacy.Helpers;

namespace Pharmacy.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;


        public UserOrderRepository(ApplicationDbContext db,
            UserManager<User> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Order> GetOrderById(int? id)
        {
            var order = await _db.Orders.Where(x => x.Id == id).FirstOrDefaultAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> UserOrders(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders = await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Product)
                            .Where(a => a.CustomerId == userId)
                            .ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<Order>> AllUserOrders()
        {
            var orders = await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Product)
                            .ToListAsync();
            return orders;
        }

        public async Task<PagedList<OrderResponce>> GetUserOrdersPagedList(OrderParams orderParams)
        {
            var query = (from order in _db.Orders
                         where order.CustomerId == orderParams.UserId
                         select new OrderResponce
                         {
                             Id = order.Id,
                             OrderDate = order.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                             OrderStatus = _db.OrderStatuses.Where(x => x.Id == order.OrderStatusId)
                                                            .Select(x => x.StatusName).SingleOrDefault(),
                             TotalPrice = _db.OrderDetails
                                            .Where(od => od.OrderId == order.Id)
                                            .Sum(od => od.Quantity * od.UnitPrice),
                             OrderDetail = _db.OrderDetails
                                   .Where(od => od.OrderId == order.Id)
                                   .Include(od => od.Product)
                                   .ToList()
                         }
                       );

            if (orderParams.OrderStatus != null && (orderParams.OrderStatus != "" && orderParams.OrderStatus != "Any"))
            {
                query = query.Where(p => p.OrderStatus == orderParams.OrderStatus);
            }

            if (orderParams.Sort != null && orderParams.Sort != "")
            {
                query = orderParams.Sort switch
                {
                    "name" => orderParams.OrderBy switch
                    {
                        "ascend" => query.OrderBy(u => u.Id),
                        "descend" => query.OrderByDescending(u => u.Id),
                    },
                };
            }

            return await PagedList<OrderResponce>.CreateAsync(query, orderParams.PageNumber, orderParams.PageSize, query.Count());
        }

        public async Task<PagedList<OrderResponce>> GetAllOrdersPagedList(OrderParams orderParams)
        {
            var query = (from order in _db.Orders
                         select new OrderResponce
                         {
                             Id = order.Id,
                             OrderDate = order.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                             OrderStatus = _db.OrderStatuses.Where(x => x.Id == order.OrderStatusId)
                                                            .Select(x => x.StatusName).SingleOrDefault(),
                             TotalPrice = _db.OrderDetails
                                            .Where(od => od.OrderId == order.Id)
                                            .Sum(od => od.Quantity * od.UnitPrice),
                             OrderDetail = _db.OrderDetails
                                   .Where(od => od.OrderId == order.Id)
                                   .Include(od => od.Product)
                                   .ToList()
                         }
                       );

            if (orderParams.OrderStatus != null && (orderParams.OrderStatus != "" && orderParams.OrderStatus != "Any"))
            {
                query = query.Where(p => p.OrderStatus == orderParams.OrderStatus);
            }

            if (orderParams.Sort != null && orderParams.Sort != "")
            {
                query = orderParams.Sort switch
                {
                    "name" => orderParams.OrderBy switch
                    {
                        "ascend" => query.OrderBy(u => u.Id),
                        "descend" => query.OrderByDescending(u => u.Id),
                    },
                };
            }

            return await PagedList<OrderResponce>.CreateAsync(query, orderParams.PageNumber, orderParams.PageSize, query.Count());
        }

        public async Task<Order> СhangeOrderStatusById(int id, string status)
        {
            var orderToUpdate = await GetOrderById(id);

            var newStatus = _db.OrderStatuses.Where(os => os.StatusName == status).FirstOrDefault();

            if (newStatus != null)
            {
                orderToUpdate.OrderStatusId = newStatus.StatusId;
                await _db.SaveChangesAsync();
            }

            return orderToUpdate;
        }
    }
}
