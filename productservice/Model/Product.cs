using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace productservice.Model

{
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int productId { get; set; }

        
        [Column("name")]
        public string name { get; set; }

        [Column("price")]
        public int price { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("category")]
        public string category { get; set; }

    }
}
