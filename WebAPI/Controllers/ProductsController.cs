using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private LocalDBMSSQLLocalDBContext _database;
        public ProductsController(LocalDBMSSQLLocalDBContext database)
        {
            _database = database;
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
        [Route("GetProductById")]
        public async Task<ProductItem> GetProductById(int id)
        {
            return await _database.ProductItem.Where( x => x.Id == id ).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProductItem")]
        public async Task<ActionResult> SaveProductItem( ProductItem product )
        {
            var entry = _database.Entry(product);

            _database.ProductItem.Add(product);
            await _database.SaveChangesAsync();
                       

            if (entry.State == EntityState.Added)
                return Ok();
            else
                return BadRequest();
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
            var dbItem = _database.ProductItem.SingleOrDefault( x => x.Id == product.Id );

            var entry = _database.Entry(dbItem);
            entry.CurrentValues.SetValues(product);

            await _database.SaveChangesAsync();

            if (entry.State == EntityState.Modified)
                return Ok();
            else
                return BadRequest();
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

            if (_database.Entry(dbItem).State == EntityState.Deleted)
                return Ok();
            else
                return BadRequest();
        }
    }
}
