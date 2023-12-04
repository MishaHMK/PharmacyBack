using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Entities;
using Pharmacy.Helpers;
using Pharmacy.Repositories;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productRepo;

        public ProductsController(IProductsRepository productRepo)
        {
            _productRepo = productRepo;
        }   


        // Get: api/Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var responce = _productRepo.GetProductList();
            return Ok(responce.Result);
        }

        // Get: api/Products/id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var responce = _productRepo.GetProductById(id);
            return Ok(responce.Result);
        }

        // Get: api/Products/pagedProd
        [HttpGet("pagedProd")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams userParams)
        {
            var productList = await _productRepo.GetProductPagedList(userParams);
            var responce = new PaginationHeader<Product>(productList, userParams.PageNumber, userParams.PageSize, productList.TotalCount);
            return Ok(responce);
        }

        // POST api/Products/create
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([FromBody] Product model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = await _productRepo.CreateProduct(model);

            return Ok(review);
        }

        // PUT api/Products/Edit/id
        [HttpPut]
        [Route("Edit/{id}")]
        public async Task<IActionResult> EditProductById(int id, [FromBody] Product product)
        {
            var productToUpdate = await _productRepo.EditProductById(id, product);
            if (productToUpdate == null)
            {
                return NotFound($"Review with Id = {id} not found");
            }

            return Ok(productToUpdate);
        }


        // DELETE api/Products/Delete/id
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteReviewById(int id)
        {
            await _productRepo.DeleteProduct(id);

            return Ok("Removed");

        }

    }
}
