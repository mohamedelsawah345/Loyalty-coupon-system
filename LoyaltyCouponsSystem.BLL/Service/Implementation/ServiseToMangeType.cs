using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class ServiseToMangeType
    {
        public string TypeOfCoupoun(string type)
        {
            string typ = type;
            switch (type)
            {
                case "1":
                    {
                        typ = "راك ثيرم";
                        break;
                    }
                case "2":
                    {
                        typ = "صرف جي تكس";
                        break;
                    }
                case "3":
                    {
                        typ = "اقطار كبيرة وهودذا";
                        break;
                    }
                case "4":
                    {
                        typ = "كعب راك ثيرم";
                        break;
                    }
                case "5":
                    {
                        typ = "كعب صرف جي تكس";
                        break;
                    }
                case "6":
                    {
                        typ = "كعب اقطار كبيرة وهودذ";
                        break;
                    }

            }
            return typ;

        }
    }
}
