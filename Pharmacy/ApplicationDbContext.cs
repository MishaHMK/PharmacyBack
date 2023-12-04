using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pharmacy.Entities;

namespace Pharmacy
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
    }
}
