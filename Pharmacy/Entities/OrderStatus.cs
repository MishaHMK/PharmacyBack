using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Entities
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required, MaxLength(20)]
        public string? StatusName { get; set; }
    }
}
