public class RepresentativeViewModel
{
    public string UserId { get; set; } // From ApplicationUserId
    public string UserName { get; set; } // From ApplicationUser
    public string NationalId { get; set; }
    public string PhoneNumber { get; set; }
    public string OptionalPhoneNumber { get; set; }
    public string Governate { get; set; }
    public string City { get; set; }
    public string ApprovalStatus { get; set; }
    public int CouponsCount { get; set; } // Derived from Coupons collection
}
