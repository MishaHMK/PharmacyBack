using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Entities
{
    public class CartDetail
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
