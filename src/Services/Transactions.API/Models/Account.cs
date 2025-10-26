using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transactions.API.Models
{
    public class Account
    {
        [Key]
        [Column("account_id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("currency")]
        public int Currency { get; set; }
        [Column("balance")]
        public decimal Balance { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
