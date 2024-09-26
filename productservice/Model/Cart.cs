using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace productservice.Model
{
    public class Cart
    {
        [Key]
        [Column("cart_id")]
        public int cartId { get; set; }


        [Column("product_id")]
        public int productId { get; set; }

        [Column("quantity")]
        public int  quantity { get; set; }

        [Column("user_id")]
        public int userId { get; set; }






    }
}
