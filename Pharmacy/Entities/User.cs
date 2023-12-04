using Microsoft.AspNetCore.Identity;

namespace Pharmacy.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? FatherName { get; set; }
        public string? Gender { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public DateTime? LastActive { get; set; }
    }
}
