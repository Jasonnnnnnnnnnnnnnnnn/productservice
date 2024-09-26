////using RabbitMQ.Client;
////using RabbitMQ.Client.Events;
////using System.Text;
////using Newtonsoft.Json; // อย่าลืมติดตั้ง Newtonsoft.Json ผ่าน NuGet
////using productservice.Model;
////using Microsoft.EntityFrameworkCore;
////using System.Diagnostics;

////namespace productservice.Service
////{
////    public class RabbitMQService : IRabbitMQService
////    {
////        private readonly IConnection _connection;
////        private readonly IModel _channel;
////        private readonly ApplicationDbContext _dbContext;

////        public RabbitMQService(ApplicationDbContext dbContext)
////        {
////            var factory = new ConnectionFactory() { HostName = "localhost" };
////            _connection = factory.CreateConnection();
////            _channel = _connection.CreateModel();
////            _channel.QueueDeclare(queue: "updateCart", durable: false, exclusive: false, autoDelete: false, arguments: null);
////            _dbContext = dbContext;

////            // เรียกใช้ Consumer
////            StartConsuming();
////        }

////        // ฟังก์ชันสำหรับการส่งข้อความไปยัง RabbitMQ
////        public void Publish(string message)
////        {
////            var body = Encoding.UTF8.GetBytes(message);
////            _channel.BasicPublish(exchange: "", routingKey: "updateCart", basicProperties: null, body: body);
////            Console.WriteLine(" [x] Sent {0}", message);
////        }

////        // เริ่มต้น Consumer เพื่อรับข้อความจากคิว
////        private void StartConsuming()
////        {
////            var consumer = new EventingBasicConsumer(_channel);
////            consumer.Received += (model, ea) =>
////            {
////                var body = ea.Body.ToArray();
////                var message = Encoding.UTF8.GetString(body);
////                //var updatecartcommand = jsonconvert.deserializeobject<string>(message);

////                Debug.WriteLine(message);
////                //if (updateCartCommand != null)
////                //{
////                //    DeleteCartItem(updateCartCommand.UserId, updateCartCommand.ProductId);
////                //}
////            };

////            _channel.BasicConsume(queue: "updateCart", autoAck: true, consumer: consumer);
////        }

////        // ฟังก์ชันสำหรับการลบสินค้าออกจากตะกร้า
////        private void DeleteCartItem(int userId, int productId)
////        {
////            var cartItem = _dbContext.Carts
////                .FirstOrDefault(c => c.userId == userId && c.productId == productId);

////            if (cartItem != null)
////            {
////                _dbContext.Carts.Remove(cartItem);
////                _dbContext.SaveChanges();
////                Console.WriteLine($"Deleted product {productId} from user {userId}'s cart.");
////            }
////            else
////            {
////                Console.WriteLine($"No item found in cart for user {userId} with product {productId}.");
////            }
////        }
////    }


////}

//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System.Text;
//using Newtonsoft.Json; // อย่าลืมติดตั้ง Newtonsoft.Json ผ่าน NuGet
//using productservice.Model;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;

//namespace productservice.Service
//{
//    public class RabbitMQService : IRabbitMQService
//    {
//        private readonly IConnection _connection;
//        private readonly IModel _channel;
//        private readonly ApplicationDbContext _dbContext;

//        public RabbitMQService(ApplicationDbContext dbContext)
//        {
//            var factory = new ConnectionFactory() { HostName = "localhost" };
//            _connection = factory.CreateConnection();
//            _channel = _connection.CreateModel();
//            _channel.QueueDeclare(queue: "updateCart", durable: false, exclusive: false, autoDelete: false, arguments: null);
//            _dbContext = dbContext;

//            // เรียกใช้ Consumer
//            StartConsuming();
//        }

//        // ฟังก์ชันสำหรับการส่งข้อความไปยัง RabbitMQ
//        public void Publish(string message)
//        {
//            var body = Encoding.UTF8.GetBytes(message);
//            _channel.BasicPublish(exchange: "", routingKey: "updateCart", basicProperties: null, body: body);
//            Console.WriteLine(" [x] Sent {0}", message);
//        }

//        // เริ่มต้น Consumer เพื่อรับข้อความจากคิว
//        private void StartConsuming()
//        {
//            var consumer = new EventingBasicConsumer(_channel);
//            consumer.Received += (model, ea) =>
//            {
//                var body = ea.Body.ToArray();
//                var message = Encoding.UTF8.GetString(body);

//                try
//                {
//                    // แปลง JSON message เป็น UpdateCartCommand object
//                    var updateCartCommand = JsonConvert.DeserializeObject<UpdateCartCommand>(message);

//                    if (updateCartCommand != null)
//                    {
//                        // ลบสินค้าออกจากตะกร้า
//                        DeleteCartItem(updateCartCommand.userId, updateCartCommand.productId);
//                    }
//                    else
//                    {
//                        Debug.WriteLine("Invalid message format.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Debug.WriteLine($"Error processing message: {ex.Message}");
//                }
//            };

//            _channel.BasicConsume(queue: "updateCart", autoAck: true, consumer: consumer);
//        }

//        // ฟังก์ชันสำหรับการลบสินค้าออกจากตะกร้า
//        private void DeleteCartItem(int userId, int productId)
//        {
//            var cartItem = _dbContext.Carts
//                .FirstOrDefault(c => c.userId == userId && c.productId == productId);

//            if (cartItem != null)
//            {
//                _dbContext.Carts.Remove(cartItem);
//                _dbContext.SaveChanges();
//                Console.WriteLine($"Deleted product {productId} from user {userId}'s cart.");
//            }
//            else
//            {
//                Console.WriteLine($"No item found in cart for user {userId} with product {productId}.");
//            }
//        }
//    }
//}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using productservice.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace productservice.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQService(IServiceScopeFactory scopeFactory)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "updateCart", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _scopeFactory = scopeFactory;

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

                try
                {
                    // แปลง JSON message เป็น UpdateCartCommand object
                    var updateCartCommand = JsonConvert.DeserializeObject<UpdateCartCommand>(message);

                    if (updateCartCommand != null)
                    {
                        // ใช้ IServiceScopeFactory เพื่อสร้าง scope ใหม่และ resolve ApplicationDbContext
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            // ลบสินค้าออกจากตะกร้า
                            DeleteCartItem(dbContext, updateCartCommand.userId, updateCartCommand.productId);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Invalid message format.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "updateCart", autoAck: true, consumer: consumer);
        }

        // ฟังก์ชันสำหรับการลบสินค้าออกจากตะกร้า
        private void DeleteCartItem(ApplicationDbContext dbContext, int userId, int productId)
        {
            var cartItem = dbContext.Carts
                .FirstOrDefault(c => c.userId == userId && c.productId == productId);

            if (cartItem != null)
            {
                dbContext.Carts.Remove(cartItem);
                dbContext.SaveChanges();
                Console.WriteLine($"Deleted product {productId} from user {userId}'s cart.");
            }
            else
            {
                Console.WriteLine($"No item found in cart for user {userId} with product {productId}.");
            }
        }
    }
}

