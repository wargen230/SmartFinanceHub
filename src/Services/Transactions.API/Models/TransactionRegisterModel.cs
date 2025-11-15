using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transactions.API.Data;

namespace Transactions.API.Models
{
    public class TransactionRegisterModel
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid CardId { get; set; }
        [Required]
        [Range(typeof(decimal),"0.0M", "79228162514264337593543950335M", ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public CategoryType Type { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        [Required]
        public DateTime Date { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(100)]
        public string? Merchant { get; set; }
        [Required]
        public Source Source { get; set; }
    }
}
