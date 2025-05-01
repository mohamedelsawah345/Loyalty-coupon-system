using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "Exchange Permissions")]

        // Display all transactions with unique ExchangePermission, sorted by the latest added first
        public async Task<IActionResult> AllTransactions()
        {
            // Fetch data from the database and include related Governorate and Area names
            var transactions = await _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Distributor)
                .Include(t => t.Governorates) // Include Governorates
                .Include(t => t.Areas) // Include Areas
                .ToListAsync();

            // Process grouping and ordering in memory
            var distinctTransactions = transactions
                .GroupBy(t => t.ExchangePermission) // Group by ExchangePermission
                .Select(g => g.OrderByDescending(t => t.Timestamp).First()) // Select the latest transaction in each group
                .OrderByDescending(t => t.Timestamp) // Sort the result by Timestamp
                .ToList();

            // Add GovernorateName and AreaName properties directly to the Transaction entity
            foreach (var transaction in distinctTransactions)
            {
                transaction.GovernorateName = transaction.Governorates?.Name; // Set GovernorateName
                transaction.AreaName = transaction.Areas?.Name; // Set AreaName
            }

            // Return the data to the view
            return View(distinctTransactions);
        }





        // Get coupon details for a specific Exchange Permission
        [HttpGet]
        public async Task<IActionResult> GetCoupons(string exchangePermission)
        {
            var coupons = await _context.Transactions
                .Where(t => t.ExchangePermission == exchangePermission)
                .Select(t => new
                {
                    t.CouponSort,
                    t.CouponType,
                    t.SequenceStart,
                    t.SequenceEnd
                })
                .ToListAsync();

            // Group by sequence range to avoid duplicate cards
            var groupedCoupons = coupons
                .GroupBy(c => new { c.SequenceStart, c.SequenceEnd })
                .Select(g => new
                {
                    SequenceStart = g.Key.SequenceStart,
                    SequenceEnd = g.Key.SequenceEnd,
                    CouponSort = g.First().CouponSort,
                    CouponType = g.First().CouponType
                })
                .ToList();

            return Json(groupedCoupons);
        }
    }
}
