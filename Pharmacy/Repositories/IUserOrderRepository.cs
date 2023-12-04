using Pharmacy.Entities;
using Pharmacy.Helpers;

namespace Pharmacy.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders(string userId);
        Task<IEnumerable<Order>> AllUserOrders();
        Task<PagedList<OrderResponce>> GetUserOrdersPagedList(OrderParams orderParams);
        Task<PagedList<OrderResponce>> GetAllOrdersPagedList(OrderParams orderParams);
        Task<Order> СhangeOrderStatusById(int id, string status);
    }
}
