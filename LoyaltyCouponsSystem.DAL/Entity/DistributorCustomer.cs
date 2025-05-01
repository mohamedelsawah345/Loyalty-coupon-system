namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class DistributorCustomer
    {
        public int DistributorID { get; set; }
        public Distributor Distributor { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
