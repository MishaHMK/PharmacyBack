using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Entities;
using Pharmacy.Helpers;

namespace Pharmacy.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProductsRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
           UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteProduct(int id)
        {
            var productToDelete = await _db.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            _db.Products.Remove(productToDelete);
            await _db.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(int? id)
        {
            var product = await _db.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task<List<Product>> GetProductList()
        {
            var productList = await _db.Products.ToListAsync();
            return productList;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            Product newProduct = new Product()
            {
                Name = product.Name, 
                Description = product.Description,
                Category = product.Category, 
                Price = product.Price,
                Avaliable = product.Avaliable
            };

            await _db.Products.AddAsync(newProduct);
            await _db.SaveChangesAsync();

            return newProduct;
        }

        public async Task<Product> EditProductById(int id, Product model)
        {
            var productToUpdate = await _db.Products.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (productToUpdate == null)
            {
                return productToUpdate;
            }

            productToUpdate.Name = model.Name;
            productToUpdate.Description = model.Description;
            productToUpdate.Category = model.Category;
            productToUpdate.Price = model.Price;
            productToUpdate.Avaliable = model.Avaliable;


            await _db.SaveChangesAsync();

            return productToUpdate;
        }


        public async Task<PagedList<Product>> GetProductPagedList(ProductParams productParams)
        {
            var query = (from product in _db.Products
                         select new Product
                         {
                             Id = product.Id,
                             Name = product.Name,
                             Description = product.Description,
                             Category = product.Category,
                             Price = product.Price, 
                             Avaliable = product.Avaliable   
                         }
                         );

            if (productParams.SearchName != null && productParams.SearchName != "")
            {
                query = query.Where(p => p.Name.Contains(productParams.SearchName));
            }

            if (productParams.Category != null && (productParams.Category != "" && productParams.Category != "Any"))
            {
                query = query.Where(p => p.Category == productParams.Category);
            }

            if (productParams.Sort != null && productParams.Sort != "")
            {
                query = productParams.Sort switch
                {
                    "name" => productParams.OrderBy switch
                    {
                        "ascend" => query.OrderBy(u => u.Name),
                        "descend" => query.OrderByDescending(u => u.Name),
                    },
                };
            }

            return await PagedList<Product>.CreateAsync(query, productParams.PageNumber, productParams.PageSize, query.Count());
        }

        public async Task SaveAsync()
        {
            _db.SaveChanges();
        }
    }
}
