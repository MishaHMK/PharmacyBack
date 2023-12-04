namespace Pharmacy.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task SaveAsync();
    }
}
