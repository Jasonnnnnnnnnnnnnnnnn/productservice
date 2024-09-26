using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace productservice.Model
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int userId { get; set; }

        [Column("name")]
        public string name { get; set; }
    }
}
