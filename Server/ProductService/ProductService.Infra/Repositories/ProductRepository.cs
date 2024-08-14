using Microsoft.Azure.Cosmos;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CosmosDbContext _context;

        public ProductRepository(CosmosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var query = _context.ProductContainer.GetItemQueryIterator<Product>();
            List<Product> results = new List<Product>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                ItemResponse<Product> response = await _context.ProductContainer.ReadItemAsync<Product>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException)
            {
                return null;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _context.ProductContainer.CreateItemAsync(product, new PartitionKey(product.Id));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _context.ProductContainer.UpsertItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _context.ProductContainer.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }
    }
}
