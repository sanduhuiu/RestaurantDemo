using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Restaurant.Services.OrderAPI.Messages;
using Restaurant.Services.OrderAPI.Models;
using Restaurant.Services.OrderAPI.Repository;
using System.Text;

namespace Restaurant.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionName;
        private readonly string checkoutMessageTopic;
        private readonly OrderRepository _orderRepository;

        private ServiceBusProcessor checkoutProcesssor;

        private readonly IConfiguration _configuration;
        public AzureServiceBusConsumer(OrderRepository orderRepository,IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;

            serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionStrings");
            subscriptionName = configuration.GetValue<string>("SubscriptionName"); 
            checkoutMessageTopic = configuration.GetValue<string>("CheckoutMessageTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            checkoutProcesssor = client.CreateProcessor(checkoutMessageTopic, subscriptionName);
        }

        public async Task Start()
        {
            checkoutProcesssor.ProcessMessageAsync += OnCheckoutMessageReceived;
            checkoutProcesssor.ProcessErrorAsync += ErrorHandler;
            await checkoutProcesssor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await checkoutProcesssor.StopProcessingAsync();
            await checkoutProcesssor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickupDateTime = checkoutHeaderDto.PickupDateTime,
            };

            foreach(var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };

                orderHeader.CartTotalItems+=detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);

            }

            await _orderRepository.AddOrder(orderHeader);
        }
    }
}
