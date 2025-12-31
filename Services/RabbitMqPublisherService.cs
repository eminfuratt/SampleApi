using RabbitMQ.Client;
using SampleApi.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SampleApi.Services
{
    public class RabbitMqPublisherService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqPublisherService()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin123"
            };
        }

        //Sipariş mesajını kuyruğa gönder
        public async Task PublishOrderPurchasedAsync(OrderPurchaseEventDto dto)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            //kuyruk oluştur
            channel.QueueDeclare(
            queue: "order-purchased",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dto));

            //mesaj kuyruğa gönder
            channel.BasicPublish(
            exchange: "",
            routingKey: "order-purchased",
            basicProperties: null,
            body: body
        );
        }

    }
}
