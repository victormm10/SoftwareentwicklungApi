using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductItem>> GetProductListAsync();

        Task<ProductItem> GetProductByIdAsync(int id);

        Task<bool> CheckIfProductExistsAsync(string name);

        Task<ProductItem> AddProductAsync(ProductItem product);

        Task UpdateProductAsync(ProductItem product);

        Task<bool> DeleteProductAsync(int id);
    }
}
