using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Transactions;
using Transactions.API.Db;
using Transactions.API.Models;

namespace Transactions.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsDbContext _db;
        private readonly IDistributedCache _cache;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(IDistributedCache cache, ILogger<TransactionsController> logger, TransactionsDbContext db)
        {
            _cache = cache;
            _logger = logger;
            _db = db;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionRegisterModel value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        sucsess = false,
                        message = "Invalid data",
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                var accountExists = await _db.Accounts.AnyAsync(c => c.Id == value.AccountId);
                if (!accountExists)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Account not found"
                    });

                var cardExists = await _db.Cards.AnyAsync(c => c.CardId == value.CardId);
                if (!cardExists)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Card not found"
                    });

                var categoryExists = await _db.Categories.AnyAsync(c => c.CategoryId == c.CategoryId);
                if (!cardExists)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Category not found"
                    });

                var transaction = new Transactions.API.Models.Transaction
                {
                    AccountId = value.AccountId,
                    CardId = value.CardId,
                    amount = value.Amount,
                    Currency = value.Currency,
                    Type = value.Type,
                    CategoryId = value.CategoryId,
                    Date = value.Date.ToUniversalTime(),
                    Description = value.Description,
                    Merchant = value.Merchant,
                    CreatedAt = DateTime.UtcNow,
                    Source = value.Source
                };

                _db.Transactions.Add(transaction);
                await _db.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(AddTransaction),
                    new { id = transaction.Id,
                        success = true,
                        message = "Transaction created successfully",
                        data = transaction
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _db.Transactions
                 .Include(t => t.Account)
                 .Include(t => t.Card)
                 .Include(t => t.Category)
                 .Select(t => new
                 {
                     t.Id,
                     t.amount,
                     t.Description,
                     t.Merchant,
                     Account = t.Account == null ? null : new
                     {
                         t.Account.Id,
                         t.Account.Name
                     },
                     Card = t.Card == null ? null : new
                     {
                         t.Card.CardId,
                         t.Card.CardNumber
                     },
                     Category = t.Category == null ? null : new
                     {
                         t.Category.CategoryId,
                         t.Category.CategoryName
                     },
                     t.Date,
                     t.Type
                 })
                 .ToListAsync();

            return Ok(new
            {
                success = true,
                count = transactions.Count,
                result = transactions
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            var transaction = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.Card)
                .Include(t => t.Category)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.Id,
                    t.amount,
                    t.Description,
                    t.Merchant,
                    Account = t.Account == null ? null : new
                    {
                        t.Account.Id,
                        t.Account.Name
                    },
                    Card = t.Card == null ? null : new
                    {
                        t.Card.CardId,
                        t.Card.CardNumber
                    },
                    Category = t.Category == null ? null : new
                    {
                        t.Category.CategoryId,
                        t.Category.CategoryName
                    },
                    t.Date,
                    t.Type
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Transaction not found"
                });
            }

            return Ok(new
            {
                success = true,
                result = transaction
            });
        }
    }
}
