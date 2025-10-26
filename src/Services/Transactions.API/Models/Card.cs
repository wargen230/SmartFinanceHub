using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transactions.API.Models
{
    [Table("Cards")]
    public class Card
    {
        [Key]
        [Column("card_id")]
        public Guid CardId { get; set; } = Guid.NewGuid();
        [Column("account_id")]
        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; } = null!;
        [Column("card_number_hash")]
        public string CardNumber { get; set; } = null!;
        [Column("masked_number")]
        public string masked_number { get; set; } = null!;
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }
        [Column("provider")]
        public string Provider { get; set; } = null!;
        public Transaction? Transaction { get; set; }
    }
}
