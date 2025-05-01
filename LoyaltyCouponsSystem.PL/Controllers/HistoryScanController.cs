//using QRCoder;
//using LoyaltyCouponsSystem.DAL.DB;
//using LoyaltyCouponsSystem.DAL.Entity;
//using Microsoft.AspNetCore.Mvc;
//using System.IO;

//using System.Drawing;
//using ZXing; // للتعامل مع تدفقات البيانات


//namespace LoyaltyCouponsSystem.PL.Controllers
//{
//    [Route("HistoryScan")]
//    public class HistoryScanController : Controller
//    {
//        private readonly ILogger<HistoryScanController> _logger;
//        //private readonly QRCodeGeneratorHelper _QRCodeGeneratorHelper;
//        private readonly ApplicationDbContext _context;

//        public HistoryScanController(ApplicationDbContext context , ILogger<HistoryScanController> logger)
//        {

//            _context = context;
//            _logger = logger;

//        }


//        // Endpoint To Can Trake QR 
//        [HttpGet("track")]
//        public IActionResult Track(string ID)
//        {
//            var scanLog = new QRScanLog
//            {
//                QR_ID = ID,
//                ScanTime = DateTime.UtcNow,
//                UserIP = HttpContext.Connection.RemoteIpAddress.ToString(),
//                UserAgent = Request.Headers["User-Agent"].ToString(),
//                NumberOfScans = _context.QRScanLogs.Count(q => q.QR_ID == ID) + 1 //we want to handel this ==>done

//            };

//            _context.QRScanLogs.Add(scanLog);
//            _context.SaveChanges();

//            return Redirect("https://example.com"); //Redirect To What You Need
//        }

//        // Show Stats
//        [HttpGet("HistoryScans")]
//        public IActionResult HistoryScans()
//        {
//            var logs = _context.QRScanLogs.ToList();
//            return View(logs);
//        }

//        // GET: Home
//        [HttpGet("Index")]
//        public ActionResult Index()
//         {
//             return View();
//         }

//        [HttpGet("DataOfScaning")]
//        public ActionResult DataOfScaning(string Key)
//        {
//            ViewBag.Data = Key;
//            return View();
//        }
//        [HttpPost]
//        public IActionResult UploadQRCode(IFormFile postedFile)
//        {
//            string DataOfQR="";
//            if (postedFile != null && postedFile.Length > 0)
//            {
//                try
//                {
//                    using (var stream = postedFile.OpenReadStream())
//                    {
//                        using (var bitmap = new Bitmap(stream))
//                        {
//                            var reader = new BarcodeReader();
//                            var result = reader.Decode(bitmap);
//                            Console.WriteLine(result);

//                            if (result != null)
//                            {
//                                DataOfQR = result.Text;
//                                ViewBag.QRText = result.Text; // عرض النص المستخرج من QR Code
//                            }
//                            else
//                            {
//                                ViewBag.QRText = "Unable to read QR Code. Please try again.";
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ViewBag.QRText = "An error occurred: " + ex.Message;
//                }
//            }
//            else
//            {
//                ViewBag.QRText = "Please upload a valid image file.";
//            }

//            //return View("Index");
//            return RedirectToAction("DataOfScaning", new { Key = DataOfQR });
//        }
//    }
//}
