using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductService.Application.Interfaces;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string productNameUpdateQueueName;

        public ProductService(IProductRepository productRepository, ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _serviceBusClient = serviceBusClient;
            productNameUpdateQueueName = configuration["AzureServiceBus:ProductNameUpdateQueueName"]!;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(product.Id);
            product.OrderCount = existingProduct.OrderCount;

            if (product.Name != existingProduct.Name)
            {
                await SendMessageToOrderServiceAsync(product.Id, product.Name);
            }
            await _productRepository.UpdateProductAsync(product);
        }

        private async Task SendMessageToOrderServiceAsync(string productId, string newProductName)
        {
            var messagePayload = new
            {
                ProductId = productId,
                NewProductName = newProductName
            };

            var messageBody = JsonConvert.SerializeObject(messagePayload);
            var message = new ServiceBusMessage(messageBody);

            var sender = _serviceBusClient.CreateSender(productNameUpdateQueueName);
            await sender.SendMessageAsync(message);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task UpdateOrderCountAsync(string productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product != null)
            {
                product.OrderCount += quantity;
                await _productRepository.UpdateProductAsync(product);
            }
        }
    }
}
