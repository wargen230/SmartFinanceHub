using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transactions.API.Data;

namespace Transactions.API.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [Column("transactions_id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("account_id")]
        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; } = null!;
        [Column("card_id")]
        public Guid CardId { get; set; }
        [ForeignKey(nameof(CardId))]
        public Card Card { get; set; } = null!;
        [Column("amount")]
        public decimal amount { get; set; }
        [Column("currency")]
        public Currency Currency { get; set; }
        [Column("type")]
        public CategoryType Type { get; set; }
        [Column("category_id")]
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("Merchant")]
        public string? Merchant {  get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("source")]
        public Source Source { get; set; }
    }
}
