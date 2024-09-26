

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using productservice.Model;
//using productservice.Service; // เพิ่มการใช้งาน RabbitMQService

//namespace productservice.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class CartController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IRabbitMQService _rabbitMQService; // เพิ่มตัวแปรสำหรับ RabbitMQService

//        public CartController(ApplicationDbContext context, IRabbitMQService rabbitMQService) // Inject RabbitMQService
//        {
//            _context = context;
//            _rabbitMQService = rabbitMQService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetCarts()
//        {
//            try
//            {
//                var carts = await _context.Carts.FromSqlRaw("SELECT cart_id , product_id, quantity , `user_id` FROM  cart ;").ToListAsync();
//                return Ok(carts);
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (ex) here as needed
//                return BadRequest(new { message = "An error occurred while retrieving the Carts.", error = ex.Message });
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] Cart newPost)
//        {
//            if (newPost == null)
//            {
//                return BadRequest(new { message = "Post data is invalid." });
//            }

//            try
//            {
//                var query = "INSERT INTO cart (product_id, quantity, `user_id`) " +
//                                           "VALUES (@p0, @p1, @p2)";
//                await _context.Database.ExecuteSqlRawAsync(query, newPost.productId, newPost.quantity, newPost.userId);
//                return Ok("Create Cart Success");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = "An error occurred while creating the Cart.", error = ex.Message });
//            }
//        }

//        [HttpDelete("DeleteData/{userId}/{productId}")]
//        public IActionResult DeleteData(int userId, int productId) // เปลี่ยนลักษณะพารามิเตอร์
//        {
//            try
//            {
//                // สร้างคำสั่ง UpdateCartCommand
//                //var command = new updatecartcommand { userid = userid, productid = productid };
//                //var message = JsonConvert.SerializeObject(command);

//                // ส่งคำสั่งไปยัง RabbitMQ
//                _rabbitMQService.Publish(message);

//                return Ok(new { message = "Delete request sent to queue." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = "An error occurred while sending the delete request.", error = ex.Message });
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using productservice.Model;
using productservice.Service; // เพิ่มการใช้งาน RabbitMQService

namespace productservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRabbitMQService _rabbitMQService; // เพิ่มตัวแปรสำหรับ RabbitMQService

        public CartController(ApplicationDbContext context, IRabbitMQService rabbitMQService) // Inject RabbitMQService
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            try
            {
                var carts = await _context.Carts.FromSqlRaw("SELECT cart_id , product_id, quantity , `user_id` FROM  cart;").ToListAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return BadRequest(new { message = "An error occurred while retrieving the Carts.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cart newPost)
        {
            if (newPost == null)
            {
                return BadRequest(new { message = "Post data is invalid." });
            }

            try
            {
                var query = "INSERT INTO cart (product_id, quantity, `user_id`) " +
                                           "VALUES (@p0, @p1, @p2)";
                await _context.Database.ExecuteSqlRawAsync(query, newPost.productId, newPost.quantity, newPost.userId);
                return Ok("Create Cart Success");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while creating the Cart.", error = ex.Message });
            }
        }

        [HttpDelete("DeleteData/{userId}/{productId}")]
        public IActionResult DeleteData(int userId, int productId)
        {
            try
            {
                // สร้างคำสั่ง UpdateCartCommand
                var command = new UpdateCartCommand { userId = userId, productId = productId };

                // แปลงคำสั่งเป็น JSON
                var message = JsonConvert.SerializeObject(command);

                // ส่งคำสั่งไปยัง RabbitMQ
                _rabbitMQService.Publish(message);

                return Ok(new { message = "Delete request sent to queue." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while sending the delete request.", error = ex.Message });
            }
        }
    }
}


