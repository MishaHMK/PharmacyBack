using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public double Price { get; set; }
        public int? Avaliable { get; set; }
        public List<OrderDetail>? OrderDetail { get; set; }
        public List<CartDetail>? CartDetail { get; set; }

    }
}
