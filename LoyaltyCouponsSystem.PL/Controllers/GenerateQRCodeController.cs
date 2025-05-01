using LoyaltyCouponsSystem.BLL.Service.Abstraction;
using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateExcel;
using LoyaltyCouponsSystem.BLL.Service.Implementation.GeneratePDF;
using LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateQR;
using LoyaltyCouponsSystem.BLL.Service.Implementation.GenerateWord;
using LoyaltyCouponsSystem.BLL.ViewModel.QRCode;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class GenerateQRCodeController : Controller
    {
        private readonly ILogger<GenerateQRCodeController> _logger;
        private readonly IQRCodeGeneratorHelper _QRCodeGeneratorHelper;
        private readonly ApplicationDbContext _context;
        //ForGeneratedBy
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GenerateQRCodeController(
            ILogger<GenerateQRCodeController> logger,
            IQRCodeGeneratorHelper QRCodeGeneratorHelper,
            ApplicationDbContext context
            , UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _QRCodeGeneratorHelper = QRCodeGeneratorHelper;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Policy = "Generate QR Codes")]

        public async Task<IActionResult> GetAreas(int governorateId)
        {
            var areas = await _context.Areas
                .Where(d => d.GovernateId == governorateId)
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return Ok(areas);
        }

       

        [HttpGet]
        public async Task<IActionResult> GenerateCouponsPDF(
            List<byte[]> couponImages,
            int couponsPerRow,
            float horizontalSpacing,
            float verticalSpacing,
            float couponSize)
        {
            if (couponImages == null || !couponImages.Any())
            {
                return BadRequest("No coupon images provided.");
            }

            try
            {
                var generatePdfWithCupones = new clsGeneratePdfWithCupones();
                byte[] pdfData = await generatePdfWithCupones.GeneratePDFWithCouponsAsync(
                    couponImages, couponsPerRow, horizontalSpacing, verticalSpacing, couponSize);

                return File(pdfData, "application/pdf", "Coupons.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateCouponsWord(
    List<byte[]> couponImages,
    int couponsPerRow,
    float horizontalSpacing,
    float verticalSpacing,
    float couponSize)
        {
            if (couponImages == null || !couponImages.Any())
            {
                return BadRequest("No coupon images provided.");
            }

            try
            {
                var generateWordWithCoupons = new clsGenerateWordWithCoupons();
                byte[] wordData = await generateWordWithCoupons.GenerateWordWithCouponsAsync(
                    couponImages, couponsPerRow, horizontalSpacing, verticalSpacing, couponSize);

                return File(wordData, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Coupons.docx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateCouponsExcel(List<Coupon> coupons)
        {
            if (coupons == null || !coupons.Any())
            {
                return BadRequest("No coupons provided.");
            }

            try
            {
                var generateExcelWithCoupons = new clsGenerateExcelWithCoupons();
                byte[] excelData = await generateExcelWithCoupons.GenerateExcelWithCouponsAsync(coupons);

                // Sort the coupons by SerialNumber
                var sortedCoupons = coupons.OrderBy(c => c.SerialNumber).ToList();

                // Get the lowest and highest SerialNumber
                var lowestSerialNumber = sortedCoupons.FirstOrDefault()?.SerialNumber ?? "default";
                var highestSerialNumber = sortedCoupons.LastOrDefault()?.SerialNumber ?? "default";
                var couponType = sortedCoupons.LastOrDefault()?.TypeOfCoupone ?? "default";

                // Dynamically set the filename to include the range from lowest to highest serial number
                var fileName = $"_{lowestSerialNumber}_to_{highestSerialNumber}_ {couponType}.xlsx";

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetMaxSerialNumber(int typeOfCoupon)
        {
            var serviceToMangeCounters = new ServiceToMangeCounters(_context);
            var maxSerialNumber = await serviceToMangeCounters.GetNextSerialNumInYearAsync(typeOfCoupon.ToString());

            return Json(maxSerialNumber);
        }

        public async Task<IActionResult> DetailsOfCoupones()
        {
            var viewModel = new QRCodeDetailsViewModel
            {
                governorates = await _context.Governorates.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PrintingExistingCoupones(string FromSerialNum ,string ToSerialNum, int Size=3)
        {
            if (FromSerialNum == null|| ToSerialNum==null)
            {
                return BadRequest("Details are missing.");
            }

            // قائمة الـ QR Codes
            var qrCodesList = _context.Coupons.Where(c => string.Compare(c.SerialNumber, FromSerialNum) >= 0 &&
                                         string.Compare(c.SerialNumber, ToSerialNum) <= 0).ToList();

            

            // إنشاء QR Codes
            //var generateListOfCoupons = new GenerateListOfCoupons();
            //string baseUrl = $"{Request.Scheme}://{Request.Host}";
            //var qrCodesAsBytes = generateListOfCoupons.GenerateQRCodesAsync(qrCodesList, baseUrl);

            // إنشاء ملف PDF من QR Codes
            // return await GenerateCouponsPDF(await qrCodesAsBytes.ConfigureAwait(false), detailsVM.CouponsPerRow, detailsVM.HorizontalSpacing, detailsVM.VerticalSpacing, detailsVM.CouponSize);

            // إنشاء ملف Word من QR Codes
            // return await GenerateCouponsWord(await qrCodesAsBytes.ConfigureAwait(false), detailsVM.CouponsPerRow, detailsVM.HorizontalSpacing, detailsVM.VerticalSpacing, detailsVM.CouponSize);
            // إنشاء ملف Excel من QR Codes
            return await GenerateCouponsExcel(qrCodesList);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DetailsOfCoupones(QRCodeDetailsViewModel detailsVM)
        {
            if (detailsVM == null)
            {
                return BadRequest("Details are missing.");
            }
            var qrCodesList = new List<Coupon>();

            if (ModelState.IsValid)
            {
                var existingCoupon = _context.Coupons
                                     .FirstOrDefault(c => c.SerialNumber == detailsVM.SerialNumber);
                if (existingCoupon != null)
                {
                    ModelState.AddModelError("SerialNumber", "هذا الرقم التسلسلي موجود بالفعل.");
                    return View(detailsVM);
                }

                // قائمة الـ QR Codes
               
                var serviceToMangeCounters = new ServiceToMangeCounters(_context);
                int currentYear = DateTime.Now.Year;
                var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);  // Access logged-in user
                ServiceToSetSerialNumber serialNumber = new ServiceToSetSerialNumber();
                var numInYear = await serviceToMangeCounters.GetNextNumInYearAsync();
                var SerialNumber = serialNumber.GetSerialNumber(detailsVM.SerialNumber, detailsVM.TypeOfCoupon, 0);
                var SerialNumberAsLong = long.Parse(SerialNumber);
                var CreatedBy = currentUser?.UserName;
                ServiseToMangeType type = new ServiseToMangeType();
                var TypeOfCouponeee = type.TypeOfCoupoun(detailsVM.TypeOfCoupon);
                var Value = detailsVM.Value;
                var GovernorateId = detailsVM.GovernorateId;
                var AreaId = detailsVM.AreaId;

                // إنشاء الكوبونات
                for (int i = 0; i < detailsVM.Count; i++)
                {
                    var couponDetails = new Coupon
                    {

                        TypeOfCoupone = TypeOfCouponeee,
                        Value = Value,
                        GovernorateId = GovernorateId,
                        AreaId = AreaId,
                        NumInYear = numInYear + i,
                        SerialNumber = (SerialNumberAsLong + i).ToString(),
                        CreatedBy = CreatedBy

                    };

                    qrCodesList.Add(couponDetails);
                }
                await _context.Coupons.AddRangeAsync(qrCodesList);
                await _context.SaveChangesAsync();

                QRCodeTransactionGenerated qRCodeTransactionGenerated = new()
                {
                    NumberOfCoupones = qrCodesList.Count,
                    FromSerialNumber = serialNumber.GetSerialNumber(detailsVM.SerialNumber, detailsVM.TypeOfCoupon, 0),
                    ToSerialNumber = serialNumber.GetSerialNumber(detailsVM.SerialNumber, detailsVM.TypeOfCoupon, qrCodesList.Count - 1),
                    GeneratedBy = currentUser?.UserName,
                    Value = detailsVM.Value,
                    TypeOfCoupone = TypeOfCouponeee,
                    GovernorateID = detailsVM.GovernorateId,
                    AreaId = detailsVM.AreaId,
                    CreationDateTime = DateTime.Now



                };
                await _context.qRCodeTransactionGenerateds.AddAsync(qRCodeTransactionGenerated);
                await _context.SaveChangesAsync();


                await serviceToMangeCounters.UpdateMaxSerialNumAsync(detailsVM.SerialNumber, detailsVM.TypeOfCoupon, detailsVM.Count - 1);

                //// إنشاء QR Codes
                //var generateListOfCoupons = new GenerateListOfCoupons();
                //string baseUrl = $"{Request.Scheme}://{Request.Host}";
                //var qrCodesAsBytes = generateListOfCoupons.GenerateQRCodesAsync(qrCodesList, baseUrl);

                // إنشاء ملف PDF من QR Codes
                // return await GenerateCouponsPDF(await qrCodesAsBytes.ConfigureAwait(false), detailsVM.CouponsPerRow, detailsVM.HorizontalSpacing, detailsVM.VerticalSpacing, detailsVM.CouponSize);

                // إنشاء ملف Word من QR Codes
                // return await GenerateCouponsWord(await qrCodesAsBytes.ConfigureAwait(false), detailsVM.CouponsPerRow, detailsVM.HorizontalSpacing, detailsVM.VerticalSpacing, detailsVM.CouponSize);
            } // إنشاء ملف Excel من QR Codes
                return await GenerateCouponsExcel(qrCodesList);
           



        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





    }
}
