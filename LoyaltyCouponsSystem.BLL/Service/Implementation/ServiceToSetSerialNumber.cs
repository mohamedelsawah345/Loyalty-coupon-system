using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class ServiceToSetSerialNumber
    {

        private string SserialNumber;

        private long LongSerialNumber;

        public string GetSerialNumber(string serialNumber,string TypeOfCoupone,int Count)
        {
            int flagforType = 0;

            switch(TypeOfCoupone)
            {
                case "1":
                    {
                        flagforType = 1;
                        break;
                    }
                case "2":
                    {
                        flagforType = 2;
                        break;
                    }
                case "3":
                    {
                        flagforType = 3;
                        break;
                    }
                case "4":
                    {
                        flagforType = 4;
                        break;

                    }
                case "5":
                    {
                        flagforType = 5;
                        break;

                    }
                case "6":
                    {
                        flagforType = 6;
                        break;

                    }
                default:
                    flagforType = 0;
                    break;



            }


            int year = DateTime.Now.Year;
            int RemendarOfYear = year % 100;
            SserialNumber = Convert.ToString(flagforType) + Convert.ToString(RemendarOfYear) + serialNumber;
            LongSerialNumber = long.Parse(SserialNumber)+Count;
            return Convert.ToString( LongSerialNumber);


        }
    }
}
