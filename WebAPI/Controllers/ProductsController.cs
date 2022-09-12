using ClassLibrary.Models;
using ClassLibrary.Repositories;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Validators;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepo;
        private ProductValidator _validator;

        public ProductsController(IProductRepository productRepo, ProductValidator validator)
        {
            _productRepo = productRepo;
            _validator = validator;
        }

        /// <summary>
        /// Get product list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductList")]
        public async Task<List<ProductItem>> GetProductList()
        {
            return await _productRepo.GetProductListAsync();
        }

        /// <summary>
        /// Get product by it's Identidier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<ProductItem> GetProductById(int id)
        {
            return await _productRepo.GetProductByIdAsync(id);
        }

        /// <summary>
        /// Validate if the product name is available
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckIfProductExists/{name}")]
        public async Task<bool> CheckIfProductExists(string name)
        {
            return await _productRepo.CheckIfProductExistsAsync(name);
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProductItem")]
        public async Task<ActionResult> SaveProductItem(ProductItem product)
        {
            ValidationResult results = _validator.Validate(product);

            if (!results.IsValid)
                return BadRequest();

            var newProduct = await _productRepo.AddProductAsync(product);

            return Ok(newProduct);
        }

        /// <summary>
        /// Update product information
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateProductItem")]
        public async Task<ActionResult> UpdateProductItem(ProductItem product)
        {
            ValidationResult results = _validator.Validate(product);

            if (!results.IsValid)
                return BadRequest();

            await _productRepo.UpdateProductAsync(product);

            return Ok();
        }

        /// <summary>
        /// Delete product by it's identidfier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProductById/{id}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var deleted = await _productRepo.DeleteProductAsync(id);
            if (!deleted)
                return BadRequest();

            return Ok();
        }
    }
}
