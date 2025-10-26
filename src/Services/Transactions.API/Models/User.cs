using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transactions.API.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public string Id { get; set; } = null!;
        public Account Account { get; set; } = null!;
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
