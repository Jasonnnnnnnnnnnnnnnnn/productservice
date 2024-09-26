using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json; // อย่าลืมติดตั้ง Newtonsoft.Json ผ่าน NuGet
using productservice.Model;
using Microsoft.EntityFrameworkCore;

namespace productservice.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ApplicationDbContext _dbContext;

        public RabbitMQService(ApplicationDbContext dbContext)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "updateCart", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _dbContext = dbContext;

            // เรียกใช้ Consumer
            StartConsuming();
        }

        // ฟังก์ชันสำหรับการส่งข้อความไปยัง RabbitMQ
        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: "updateCart", basicProperties: null, body: body);
            Console.WriteLine(" [x] Sent {0}", message);
        }

        // เริ่มต้น Consumer เพื่อรับข้อความจากคิว
        private void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var updateCartCommand = JsonConvert.DeserializeObject<UpdateCartCommand>(message);

                if (updateCartCommand != null)
                {
                    DeleteCartItem(updateCartCommand.UserId, updateCartCommand.ProductId);
                }
            };

            _channel.BasicConsume(queue: "updateCart", autoAck: true, consumer: consumer);
        }

        // ฟังก์ชันสำหรับการลบสินค้าออกจากตะกร้า
        private void DeleteCartItem(int userId, int productId)
        {
            var cartItem = _dbContext.Carts
                .FirstOrDefault(c => c.userId == userId && c.productId == productId);

            if (cartItem != null)
            {
                _dbContext.Carts.Remove(cartItem);
                _dbContext.SaveChanges();
                Console.WriteLine($"Deleted product {productId} from user {userId}'s cart.");
            }
            else
            {
                Console.WriteLine($"No item found in cart for user {userId} with product {productId}.");
            }
        }
    }

    // คำสั่งสำหรับการลบสินค้าจากตะกร้า
    public class UpdateCartCommand
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
