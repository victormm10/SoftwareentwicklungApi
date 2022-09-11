using ClassLibrary.Models;
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
        private LocalDBMSSQLLocalDBContext _database;
        private ProductValidator _validator;

        public ProductsController(LocalDBMSSQLLocalDBContext database, ProductValidator validator)
        {
            _database = database;
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
            return await _database.ProductItem.ToListAsync();
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
            return await _database.ProductItem.Where(x => x.Id == id).FirstOrDefaultAsync();
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
            return await _database.ProductItem.Where(x => x.Name.ToLower() == name.ToLower()).AnyAsync();
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

            _database.ProductItem.Add(product);
            await _database.SaveChangesAsync();

            return Ok(product);
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

            var dbItem = _database.ProductItem.SingleOrDefault(x => x.Id == product.Id);

            var entry = _database.Entry(dbItem);
            entry.CurrentValues.SetValues(product);

            await _database.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Delete product by it's identidfier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProductById")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var dbItem = _database.ProductItem.SingleOrDefault(x => x.Id == id);

            _database.ProductItem.Remove(dbItem);
            await _database.SaveChangesAsync();

            var exists = _database.ProductItem.Where(x => x.Id == id).Any();
            if (exists)
                return BadRequest();

            return Ok();
        }
    }
}
