using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all required data sequentially (no parallel execution)
            ViewBag.Employes = await _context.Users.CountAsync();
            int customers = await _context.Customers.CountAsync();
            int distributors = await _context.Distributors.CountAsync();
            int technicians = await _context.Technicians.CountAsync();
            ViewBag.Vistores = customers + distributors + technicians;

            ViewBag.Order = await _context.Transactions
              .Where(t => !string.IsNullOrEmpty(t.ExchangePermission)) // Filter out rows with empty ExchangePermission
              .Select(t => t.ExchangePermission) // Select the ExchangePermission field
              .Distinct() // Ensure distinct values are counted
              .CountAsync();



            ViewBag.TotalSales = await _context.Coupons.SumAsync(s => s.Value);

            // Fetch all coupons at once to avoid multiple database hits
            var coupons = await _context.Coupons.ToListAsync();

            // Coupon counts by type
            ViewBag.NumberOfCopounesRakTherm = coupons.Count(s => s.TypeOfCoupone == "راك ثيرم");
            ViewBag.NumberOfCopounesGeTex = coupons.Count(s => s.TypeOfCoupone == "صرف جي تكس");
            ViewBag.NumberOfCopounesHozenda = coupons.Count(s => s.TypeOfCoupone == "اقطار كبيرة وهودذا");
            ViewBag.NumberOfCopounesKaabRakTherm = coupons.Count(s => s.TypeOfCoupone == "كعب راك ثيرم");
            ViewBag.NumberOfCopouneskaabGeTex = coupons.Count(s => s.TypeOfCoupone == "كعب صرف جي تكس");
            ViewBag.NumberOfCopounesKaabHozenda = coupons.Count(s => s.TypeOfCoupone == "كعب اقطار كبيرة وهودذ");

            // Group data for coupon statuses
            var couponGroups = coupons
                .GroupBy(s => new { s.TypeOfCoupone, s.Status })
                .Select(g => new
                {
                    Type = g.Key.TypeOfCoupone,
                    Status = g.Key.Status,
                    Count = g.Count()
                })
                .ToList();

            // Define coupon types and statuses
            var types = new[]
            {
                "راك ثيرم", "صرف جي تكس", "اقطار كبيرة وهودذا",
                "كعب راك ثيرم", "كعب صرف جي تكس", "كعب اقطار كبيرة وهودذ"
            };
            var statuses = new[] { "Created", "Pending", "Preapproved", "Approved" };

            // Create a dictionary to store counts
            var couponCounts = types.ToDictionary(
                type => type,
                type => statuses.ToDictionary(status => status, status => 0)
            );

            // Populate the dictionary with actual data
            foreach (var group in couponGroups)
            {
                if (couponCounts.ContainsKey(group.Type) && couponCounts[group.Type].ContainsKey(group.Status))
                {
                    couponCounts[group.Type][group.Status] = group.Count;
                }
            }

            // Pass data to ViewBag
            ViewBag.CouponCounts = couponCounts;

            return View();
        }
    }
}
