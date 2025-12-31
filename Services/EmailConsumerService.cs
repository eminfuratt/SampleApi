using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SampleApi.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SampleApi.Services
{
    public class EmailConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EmailService _emailService; 
        public EmailConsumerService(EmailService emailService)
        {
            _emailService = emailService;

            // RabbitMQ bağlantısı
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin123"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Kuyruğu oluştur
            _channel.QueueDeclare(
                queue: "order-purchased",
                durable: false,      // Sunucu kapanınca kuyruk kaybolur
                exclusive: false,    // Kuyruk birden fazla consumer tarafından kullanılabilir
                autoDelete: false,   // Subscriber kapanınca silinmez
                arguments: null
            );

            Console.WriteLine("📥 EmailConsumerService STARTED");
        }

        // BackgroundService’in sürekli çalışan metodu anklocgment 
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    
                    var body = args.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<OrderPurchaseEventDto>(json);

                    Console.WriteLine($"📨 ORDER RECEIVED → OrderId: {message.OrderId}");

                    // Gerçek e-posta gönder
                    await _emailService.SendOrderEmailAsync(
                        message.Email,
                        message.OrderId,
                        message.TotalPrice
                    );

                    // Mesajı onayla, kuyruktan sil
                    _channel.BasicAck(
                        deliveryTag: args.DeliveryTag,
                        multiple: false
                    );

                    Console.WriteLine("✅ EMAIL SENT & MESSAGE ACKED");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ EMAIL ERROR: " + ex.Message);
                }
            };

            // Kuyruğu dinlemeye başla
            _channel.BasicConsume(
                queue: "order-purchased",
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
