using ClassLibrary.Models;
using ClassLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerInfrascture.Repositories
{
    public class SqlServerProductRepository : IProductRepository
    {
        private SqlServerDbContext _database;
        public SqlServerProductRepository()
        {
            _database = new SqlServerDbContext();
        }

        public async Task<ProductItem> AddProductAsync(ProductItem product)
        {
            _database.ProductItem.Add(product);
            await _database.SaveChangesAsync();

            return product;
        }

        public async Task<bool> CheckIfProductExistsAsync(string name)
        {
            return await _database.ProductItem.Where(x => x.Name.ToLower() == name.ToLower()).AnyAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var dbItem = _database.ProductItem.SingleOrDefault(x => x.Id == id);

            _database.ProductItem.Remove(dbItem);
            await _database.SaveChangesAsync();

            return !await _database.ProductItem.Where(x => x.Id == id).AnyAsync();
        }

        public async Task<ProductItem> GetProductByIdAsync(int id)
        {
            return await _database.ProductItem.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ProductItem>> GetProductListAsync()
        {
            return await _database.ProductItem.OrderByDescending(o => o.Id).ToListAsync();
        }

        public async Task UpdateProductAsync(ProductItem product)
        {
            var dbItem = _database.ProductItem.SingleOrDefault(x => x.Id == product.Id);

            var entry = _database.Entry(dbItem);
            entry.CurrentValues.SetValues(product);

            await _database.SaveChangesAsync();
        }
    }
}
