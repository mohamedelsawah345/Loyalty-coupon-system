using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.BLL.ViewModel.QRCode;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.PL.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;

namespace QrCodeScannerApp.Controllers
{
    public class QrCodeController : Controller
    {
        

        private readonly ILogger<QrCodeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ServiceToManageStatues _Status;

        //ForGeneratedBy
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public QrCodeController(ServiceToManageStatues Status,ILogger<QrCodeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _Status = Status;
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        // GET: /QrCode/Index
        public async Task<IActionResult> Index(string receiptNumber) // Corrected spelling
        {
            if (string.IsNullOrEmpty(receiptNumber))
            {
                TempData["Error"] = "Receipt number is required.";
                return RedirectToAction("AllTransactions");
            }

            // Fetch scanned QR codes and count
            ViewBag.ScannedQrCodes = _context.QRScanLogs
                .Where(c => c.ReceiptNumber == receiptNumber)
                .Select(i => i.QR_ID);

            ViewBag.Counter = _context.QRScanLogs
                .Where(c => c.ReceiptNumber == receiptNumber)
                .Select(c => c.QR_ID)
                .Distinct()
                .Count();

            ViewBag.ReceiptNumber = receiptNumber; // Store receipt number

            return View();
        }



        // POST: /QrCode/SaveData
        // POST: /QrCode/SaveData
        [HttpPost]
        public IActionResult SaveData([FromBody] SaveQrCodeRequest request)
        {
            if (!string.IsNullOrEmpty(request.QrCodeData) && !string.IsNullOrEmpty(request.ReceiptNumber))
            {
                // Trim spaces to ensure consistency
                string trimmedQrCodeData = request.QrCodeData.Trim();

                // Check if the QR code exists in the Coupons table
                var coupon = _context.Coupons.FirstOrDefault(c => c.CouponeId.Trim() == trimmedQrCodeData);
                if (coupon == null)
                {
                    return Json(new { success = false, message = "Invalid QR code. Coupon does not exist." });
                }

                // Check if the QR code has already been saved in QRScanLogs
                bool isQrAlreadyScanned = _context.QRScanLogs.Any(q => q.QR_ID.Trim() == trimmedQrCodeData && q.ReceiptNumber == request.ReceiptNumber);
                if (isQrAlreadyScanned)
                {
                    return Json(new { success = false, message = "This QR code has already been scanned and saved." });
                }

                // Get the logged-in user
                string scannedBy = User.Identity.Name;  // Logged-in username

                // Save the new QR scan log only if it does not exist
                var log = new QRScanLog()
                {
                    QR_ID = trimmedQrCodeData,
                    ScanedBy = scannedBy,  // Set to logged-in user's name
                    ScanTime = DateTime.Now,
                    ReceiptNumber = request.ReceiptNumber,  // Set dynamically from the request
                    CouponId = coupon.CouponeId // Associate QRScanLog with Coupon
                };
                _Status.ManageStatuesScaningFrist(trimmedQrCodeData);
                // Save the scanned QR code data
                _context.QRScanLogs.Add(log);
                _context.SaveChanges();

                return Json(new { success = true, message = "QR code data saved successfully." });
            }

            return Json(new { success = false, message = "Invalid QR code data." });
        }







    }
}