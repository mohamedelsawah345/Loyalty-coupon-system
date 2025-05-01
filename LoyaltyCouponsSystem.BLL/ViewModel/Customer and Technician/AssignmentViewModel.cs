using LoyaltyCouponsSystem.BLL.Service.Implementation.Validations;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class AssignmentViewModel 
{
    public int TransactionID { get; set; }
    public List<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
    
    public List<SelectListItem> Distributors { get; set; } = new List<SelectListItem>();
    public List<SelectListItem>? Governates { get; set; } = new List<SelectListItem>();
    public List<SelectListItem>? Cities { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> CouponSorts { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> CouponTypes { get; set; } = new List<SelectListItem>();

    public string? SelectedCustomerCode { get; set; }
    public string? SelectedDistributorCode { get; set; }

    [Required(ErrorMessage = "Coupon sort is required.")]
    public string SelectedCouponSort { get; set; }

    [Required(ErrorMessage = "Coupon type is required.")]
    public string SelectedCouponType { get; set; }
    public string? SelectedGovernate { get; set; }
    public string? SelectedCity { get; set; }

    [Required(ErrorMessage = "SequenceStart is required.")]
    [RegularExpression(@"^[1-6]\d*$", ErrorMessage = "SequenceStart must start with a valid prefix (1-6).")]
    public string SequenceStart { get; set; }  // Add this property

    [Required(ErrorMessage = "SequenceEnd is required.")]
    [RegularExpression(@"^[1-6]\d*$", ErrorMessage = "SequenceEnd must start with a valid prefix (1-6).")]
    public string SequenceEnd { get; set; }  // Add this property
    public static string LastSequenceNumber { get; set; }  // Store the last sequence number dynamically


    [StringLength(50, ErrorMessage = "Exchange permission cannot exceed 50 characters.")]
    public string? ExchangePermission { get; set; }

    public string? CustomerDetails { get; set; }
    public string? TechnicianDetails { get; set; }
    public string? CreatedBy { get; set; }
    public string? GovernorateName { get; set; }
    public string? AreaName { get; set; }



}
