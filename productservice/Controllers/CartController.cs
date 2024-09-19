using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productservice.Model;

namespace productservice.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            try
            {
                var carts = await _context.Carts.FromSqlRaw("SELECT cart_id , product_id, quantity , `user`   FROM  cart ;").ToListAsync();
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
                var query = "INSERT INTO cart (product_id, quantity, `user`) " +
                                           "VALUES (@p0, @p1, @p2)";
                await _context.Database.ExecuteSqlRawAsync(query, newPost.productId, newPost.quantity, newPost.user);
                return Ok("Create Cart Success");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while creating the Cart.", error = ex.Message });
            }
        }

        [HttpDelete("DeleteData/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            try
            {
                var existingCart = await _context.Carts
                    .FindAsync(id);

                if (existingCart == null)
                {
                    return NotFound(new { message = "Carts  not found." });
                }
                _context.Carts.Remove(existingCart);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Carts  deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while deleting the Carts .", error = ex.Message });
            }
        }







    }
}
