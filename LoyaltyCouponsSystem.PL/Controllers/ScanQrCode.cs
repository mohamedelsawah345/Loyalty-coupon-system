using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using QrCodeScannerApp.Controllers;
using System.Net.NetworkInformation;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class ScanQrCode : Controller
    {
        private readonly ILogger<QrCodeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ServiceToManageStatues _Status;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScanQrCode(ServiceToManageStatues Status, ILogger<QrCodeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _Status = Status;
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /QrCode/Index
        public IActionResult Index()
        {
            // Fetch scanned serial numbers from the database and remove duplicates
            var scannedSerials = _context.Coupons
                .Where(c => !string.IsNullOrEmpty(c.ScannedBy))  // Only consider scanned coupons
                .Select(c => c.SerialNumber)  // Select only the SerialNumber
                .Distinct()  // Remove duplicates
                .ToList();

            ViewBag.ScannedSerialNumbers = scannedSerials ?? new List<string>();

            // Return the view with the list of scanned serial numbers
            return View();
        }

        // POST: /QrCode/SaveData
        [HttpPost]
        public IActionResult SaveData([FromBody] string qrCodeData)
        {
            if (string.IsNullOrEmpty(qrCodeData))
            {
                return Json(new { success = false, message = "Invalid QR Code data" });
            }

            // Trim the qrCodeData to remove leading and trailing whitespaces
            var trimmedQrCodeData = qrCodeData.Trim();

            // Fetch the coupon based on the trimmed CouponeId (qrCodeData in this case)
            var coupon = _context.Coupons.FirstOrDefault(c => c.CouponeId.Trim() == trimmedQrCodeData);

            if (coupon == null)
            {
                return Json(new { success = false, message = "Coupon not found" });
            }

            // Retrieve the SerialNumber from the coupon object
            var serialNumber = coupon.SerialNumber;

            // Check if the coupon has already been scanned
            var alreadyScanned = _context.Coupons
                .Any(c => c.SerialNumber == serialNumber && !string.IsNullOrEmpty(c.ScannedBy));

            if (alreadyScanned)
            {
                return Json(new { success = false, message = "This coupon has already been scanned." });
            }

            // Update the ScannedBy field with the current user's identity
            coupon.ScannedBy = User.Identity.Name;

            // Call your service method (assuming it handles the serialNumber appropriately)
            _Status.ManageStatuesScaningsecond(serialNumber);

            // Save changes to the database
            _context.SaveChanges();

            // Return the response with the serialNumber
            return Json(new { success = true, serialNumber = serialNumber });
        }






    }
}
