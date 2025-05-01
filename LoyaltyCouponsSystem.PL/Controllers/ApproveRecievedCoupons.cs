using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class ApproveRecievedCouponsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public ApproveRecievedCouponsController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            string targetRole = "مندوب تسليم إدارة المخازن";

            var role = await _roleManager.FindByNameAsync(targetRole);
            var usersInRole = role != null
                ? _context.UserRoles
                    .Where(ur => ur.RoleId == role.Id)
                    .Select(ur => ur.UserId)
                    .ToList()
                : new List<string>();

            var userSelectList = _context.Users
                .Where(u => usersInRole.Contains(u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToList();

            var governorates = _context.Governorates
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                }).ToList();

            ApproveRecievedCoupons model = new ApproveRecievedCoupons();

            if (id.HasValue)
            {
                model = _context.ApproveRecievedCoupons
                    .FirstOrDefault(a => a.Id == id);

                if (model != null)
                {
                    var areas = _context.Areas
                        .Where(a => a.GovernateId == model.GovernorateId)
                        .Select(a => new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Name
                        }).ToList();

                    ViewBag.Areas = areas;
                }
            }

            ViewBag.UsersDropdown = userSelectList;
            ViewBag.Governorates = governorates;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ApproveRecievedCoupons model)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in user
                var currentUser = await _userManager.GetUserAsync(User);

                if (model.Id == 0) // Creating a new record
                {
                    model.CreatedBy = currentUser?.UserName; // Set CreatedBy as the logged-in user's username
                    _context.ApproveRecievedCoupons.Add(model);
                }
                else // Updating an existing record
                {
                    _context.ApproveRecievedCoupons.Update(model);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }


        [HttpGet]
        public IActionResult GetAreasByGovernorate(int governorateId)
        {
            var areas = _context.Areas
                .Where(a => a.GovernateId == governorateId)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();

            return Json(areas);
        }

        public async Task<IActionResult> AllTransactions()
        {
            // Fetch all records from ApproveRecievedCoupons along with related entities
            var coupons = await _context.ApproveRecievedCoupons
                .Include(c => c.Governorates)
                .Include(c => c.Areas)
                .Include(c => c.ApplicationUser)
                .ToListAsync();

            // Pass the list of coupons to the View
            ViewBag.Coupons = coupons;

            return View();
        }

        public async Task<IActionResult> ScannedQrCodes(string receiptNumber)
        {
            if (string.IsNullOrEmpty(receiptNumber))
            {
                TempData["Error"] = "Receipt number is required.";
                return RedirectToAction("AllTransactions", "ReceiveFromCustomer"); // Or another relevant action
            }

            // Fetch QR scan logs based on the receipt number
            var qrScanLogs = await _context.QRScanLogs
                .Where(log => log.ReceiptNumber == receiptNumber)
                .OrderByDescending(log => log.ScanTime)
                .ToListAsync();

            if (qrScanLogs == null || !qrScanLogs.Any())
            {
                TempData["Error"] = "No QR scan logs found for the provided receipt number.";
                return RedirectToAction("AllTransactions"); // Or another relevant action
            }

            // Get the serial number from the Coupon table
            foreach (var log in qrScanLogs)
            {
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.CouponeId == log.QR_ID);

                if (coupon != null)
                {
                    log.SerialNumber = coupon.SerialNumber;
                }
                else
                {
                    log.SerialNumber = "Not Found"; // Or any fallback value
                }
            }

            return View(qrScanLogs);
        }

    }
}
