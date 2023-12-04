using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? PharmacistId { get; set; }
        public string? CustomerId { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }
}
