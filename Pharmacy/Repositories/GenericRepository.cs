namespace Pharmacy.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _db;

        internal Microsoft.EntityFrameworkCore.DbSet<T> dbSet { get; set; }

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = db.Set<T>();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
