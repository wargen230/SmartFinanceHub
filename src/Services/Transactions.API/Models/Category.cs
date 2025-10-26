using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transactions.API.Data;

namespace Transactions.API.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        [Column("category_id")]
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        [Column("user_id")]
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; } = null!;
        [Column("category_name")]
        public string CategoryName { get; set; } = null!;
        [Column("category_type")]
        public CategoryType Type { get; set; }
        public Transaction? Transaction { get; set; }

    }
}
