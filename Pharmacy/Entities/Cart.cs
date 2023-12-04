using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public bool IsDeleted { get; set; } = false; 

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
