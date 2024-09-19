using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productservice.Model;
using MySqlConnector;


namespace productservice.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProvinces()
        {
            try
            {
                var products = await _context.Products.FromSqlRaw("SELECT product_id , `name`, price , `description` , category   FROM  product ;").ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return BadRequest(new { message = "An error occurred while retrieving the products.", error = ex.Message });
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProduct(int Id)
        {
            try
            {
                var products = await _context.Products.FromSqlRaw("SELECT product_id , `name`, price , `description` , category   FROM  product WHERE product_id = {0}", Id).FirstOrDefaultAsync();

                if (products == null)
                {
                    return NotFound("Product not found.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, $"An error occurred while retrieving the Product: {ex.Message}");
            }
       
        
          }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product newPost)
        {
            if (newPost == null)
            {
                return BadRequest(new { message = "Post data is invalid." });
            }

            try
            {
                var query = "INSERT INTO product (`name`, price, `description`, category) " +
                                           "VALUES (@p0, @p1, @p2, @p3)";
                await _context.Database.ExecuteSqlRawAsync(query, newPost.name, newPost.price, newPost.description, newPost.category);
                return Ok("Create Product Success");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while creating the post.", error = ex.Message });
            }
        }

        [HttpPatch("UpdateData")]
        public async Task<IActionResult> UpdateData([FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest(new { message = "Invalid data." });
            }

            try
            {
                Product? existingProduct = await _context.Products
                    .FindAsync(updatedProduct.productId);

                if (existingProduct == null)
                {
                    return NotFound(new { message = "Products not found." });
                }

                existingProduct.name = updatedProduct.name;
                existingProduct.price = updatedProduct.price;
                existingProduct.description = updatedProduct.description;
                existingProduct.category = updatedProduct.category;

                await _context.SaveChangesAsync();
                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while updating the Products.", error = ex.Message });
            }
        }

        [HttpDelete("DeleteData/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            try
            {
                var existingProduct = await _context.Products
                    .FindAsync(id);

                if (existingProduct == null)
                {
                    return NotFound(new { message = "Products  not found." });
                }
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Products  deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while deleting the Products.", error = ex.Message });
            }
        }





    }

}
