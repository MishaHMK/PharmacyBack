using Pharmacy.Entities;

namespace Pharmacy.Helpers
{
    public class OrderResponce
    {
        public int Id { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public double TotalPrice { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }
}
