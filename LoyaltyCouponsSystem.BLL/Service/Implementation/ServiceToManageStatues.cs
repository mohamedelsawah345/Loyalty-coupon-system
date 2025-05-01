using LoyaltyCouponsSystem.BLL.ViewModel.QRCode;
using LoyaltyCouponsSystem.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class ServiceToManageStatues
    {
        private readonly ApplicationDbContext _context;

        public ServiceToManageStatues(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task <bool> ManageStatuesEcxhangeOrder(string FromSequence,string ToSequence,string CustomerCode,string DistributorCode) 
        {

            var Result=_context.Coupons.Where(c => string.Compare(c.SerialNumber, FromSequence) >= 0 &&
                                         string.Compare(c.SerialNumber, ToSequence) <= 0).ToList();

            foreach (var item in Result)
            {
                item.Status = "Pending";
                item.FlagToPrint = false;
               


                item.DistributorCode = DistributorCode;

                item.CustomerCode=CustomerCode;

                
            }
            _context.SaveChanges();

            return true;
        
        
        }

        public async Task<bool> ManageStatuesScaningFrist( string Sequence)
        {

            var Result = _context.Coupons.Where(c => c.CouponeId == Sequence).FirstOrDefault();


                Result.Status = "Preapproved";

                 _context.SaveChanges();



            return true;


        }

        public async Task<bool> ManageStatuesScaningsecond(string Sequence)
        {

            var Result = _context.Coupons.Where(c => c.SerialNumber == Sequence).FirstOrDefault();


            Result.Status = "Approved";

            _context.SaveChanges();



            return true;


        }

        public bool DecideToPrintOrNo(string FromSequence, string ToSequence)
        {

            var Result = _context.Coupons.Where(c => string.Compare(c.SerialNumber, FromSequence) >= 0 &&
                                         string.Compare(c.SerialNumber, ToSequence) <= 0).Any(c=>c.FlagToPrint==false);
            return !Result;


        }



    }
}
