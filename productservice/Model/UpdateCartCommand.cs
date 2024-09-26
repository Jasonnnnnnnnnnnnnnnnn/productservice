using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace productservice.Model
{
    public class UpdateCartCommand
    {

        [Key]
        [Column("update_id")]
        public int updateId { get; set; }

        [Column("user_id")]
        public int userId { get; set; }

        [Column("product_id")]
        public int productId { get; set; }
    }
}
