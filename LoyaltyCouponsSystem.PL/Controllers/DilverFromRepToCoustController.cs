using LoyaltyCouponsSystem.BLL.Service.Implementation;
using LoyaltyCouponsSystem.BLL.ViewModel.DeliverFormRepToCoust;
using LoyaltyCouponsSystem.BLL.ViewModel.QRCode;
using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;

namespace LoyaltyCouponsSystem.PL.Controllers
{
    public class DilverFromRepToCoustController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DilverFromRepToCoustController(ApplicationDbContext context,
           UserManager<ApplicationUser> userManager,
           IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task< IActionResult> Index()
        {
            var ModelVM = new DeliverFromRepToCoustVM
            {
                governorates = await _context.Governorates.ToListAsync()

            };   
            ViewBag.customer = _context.Customers.ToList();
           


            return View(ModelVM);
            
        }



        [HttpPost]
        public async Task<IActionResult> Index(DeliverFromRepToCoustVM model)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var CreatedBy = currentUser?.UserName;

            // Validate required fields
            if (string.IsNullOrEmpty(model.SelectedCustomerCode))
                ModelState.AddModelError("SelectedCustomerCode", "Customer is required.");

            if (!model.GovernorateId.HasValue)
                ModelState.AddModelError("GovernorateId", "Governorate is required.");

            if (!model.AreaId.HasValue)
                ModelState.AddModelError("AreaId", "City is required.");

            // Validate ExchangePermission uniqueness
            //var isUniqueExchangePermission = await ValidateExchangePermissionUniqueness(model.ExchangePermission);
            //if (!isUniqueExchangePermission)
            //{
            //    ModelState.AddModelError("ExchangePermission", "Exchange Permission Number is already used.");
            //}

            // If validation fails, repopulate ViewBag and return the view
            if (!ModelState.IsValid)
            {
                model.governorates = await _context.Governorates.ToListAsync(); // Reload governorates
                ViewBag.customer = await _context.Customers.ToListAsync(); // Repopulate customers

                return View(model);
            }

            byte[] imageBytes = null;

            if (model.image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.image.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            var item = new DeliverFromRepToCoust
            {
                CostomerCode = model.SelectedCustomerCode,              
                GovernorateId = model.GovernorateId,
                AreaId = model.AreaId,
                ExchangePermission = model.ExchangePermission,
                Image = imageBytes,
                CreatedBy = CreatedBy
            };

            _context.DeliverFromRepToCousts.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Transaction");
        }


        //private async Task<bool> ValidateExchangePermissionUniqueness(string exchangePermission)
        //{
        //    if (string.IsNullOrWhiteSpace(exchangePermission))
        //    {
        //        return false; // Invalid if it's empty
        //    }

        //    var exists = await _context.DeliverFromRepToCousts
        //        .AnyAsync(e => e.ExchangePermission == exchangePermission);

        //    return !exists; // Return true if it's unique, false if it exists
        //}



        public IActionResult GetImage(int id)
        {
            var item = _context.DeliverFromRepToCousts.FirstOrDefault(x => x.Id == id);

            if (item?.Image != null)
            {
                return File(item.Image, "image/jpeg");  // or the appropriate MIME type (e.g., image/png)
            }

            return NotFound();
        }

        public IActionResult Transaction(string ExchangePermissionNumber = "", string governorate = "",
            string area = "", int page = 1, int pageSize = 15)
        {

            var query = _context.DeliverFromRepToCousts
                    .Include(c => c.Governorates) // ربط المحافظات
                    .Include(c => c.Areas)        // ربط المناطق
                    .OrderByDescending(c => c.Timestamp) // ترتيب حسب Serial Number
                    .AsQueryable();

            if (!string.IsNullOrEmpty(ExchangePermissionNumber))
            {
                query = query.Where(c => c.ExchangePermission==ExchangePermissionNumber);
            }

            if (!string.IsNullOrEmpty(governorate))
            {
                query = query.Where(c => c.Governorates != null && c.Governorates.Name.Contains(governorate));
            }
            if (!string.IsNullOrEmpty(area))
            {
                query = query.Where(c => c.Areas != null && c.Areas.Name.Contains(area));
            }

            // إجمالي عدد العناصر بعد التصفية
            int totalCount = query.Count();

            // تطبيق الترقيم
            var paginatedCoupons = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new TransactionForRecieptFromRepToCustVM
                {
                    ExchangePermissionNumber = c.ExchangePermission,
                    GovernorateName = c.GovernorateId != null ? c.Governorates.Name : "N/A",
                    
                    AreaName = c.AreaId != null ? c.Areas.Name : "N/A",
                    
                    CreationDateTime = c.Timestamp,
                    GeneratedBy = c.CreatedBy,
                    CustomerCode=c.CostomerCode,
                    TechnitionCode = c.TechnitionCode,
                    ReprsentitiveCode = c.RepresintitiveCode,
                    Id = c.Id,
                    
                    
                    
                })

                .ToList().OrderByDescending(c => c.CreationDateTime);


            // تمرير بيانات الصفحة
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.Governorates = _context.Governorates.Select(s=>s.Name).ToList();

            return View(paginatedCoupons);


            
        }

       
        

        [HttpGet]
        public IActionResult GetAreas(int governorateId)
        {
            var areas = _context.Areas.Where(a => a.GovernateId == governorateId).ToList();
            return Json(areas);
        }


    }
}
