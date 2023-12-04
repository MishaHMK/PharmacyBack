using Pharmacy.Entities;
using Pharmacy.Helpers;

namespace Pharmacy.Repositories
{
    public interface IProductsRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<Product> EditProductById(int id, Product model);
        Task<Product> GetProductById(int? id);
        Task<List<Product>> GetProductList();
        Task DeleteProduct(int id);
        Task SaveAsync();
        Task<PagedList<Product>> GetProductPagedList(ProductParams productParams);
    }
}
