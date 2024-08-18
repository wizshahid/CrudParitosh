using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Services
{
    public class ProductNameUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public ProductNameUpdateBackgroundService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _serviceBusClient = new ServiceBusClient(configuration["AzureServiceBus:ConnectionString"]);

            _serviceBusProcessor = _serviceBusClient.CreateProcessor(
                topicName: configuration["AzureServiceBus:ProductNameUpdateTopicName"],
                subscriptionName: configuration["AzureServiceBus:SubscriptionName"],
                new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 1,
                    AutoCompleteMessages = false
                });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _serviceBusProcessor.ProcessMessageAsync += ProcessMessageAsync;
            _serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;

            await _serviceBusProcessor.StartProcessingAsync(stoppingToken);

            await Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            var content = args.Message.Body.ToString();
            var productUpdateInfo = JsonConvert.DeserializeObject<ProductUpdateInfo>(content);

            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrdersService>();
                await orderService.UpdateOrderProductNameAsync(productUpdateInfo.ProductId, productUpdateInfo.NewProductName);
            }

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            // Handle the error (log it, etc.)
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceBusProcessor.StopProcessingAsync(cancellationToken);
            await _serviceBusProcessor.DisposeAsync();
            await _serviceBusClient.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }

    public class ProductUpdateInfo
    {
        public string ProductId { get; set; } = null!;
        public string NewProductName { get; set; } = null!;
    }
}
