using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation
{
    public class ServiceToMangeCounters
    {
        private readonly ApplicationDbContext _context;

        public ServiceToMangeCounters(ApplicationDbContext context)
        {
            _context = context;
        }

        // تحويل هذه الدالة لتعمل بشكل غير متزامن
        public async Task<long> GetNextNumInYearAsync()
        {
            int currentYear = DateTime.Now.Year;

            // Check if the year exists
            var globalCounter = await _context.GlobalCounters
                .SingleOrDefaultAsync(gc => gc.YearNotId == currentYear);

            if (globalCounter == null)
            {
                // If not exists, create a new row for the current year
                globalCounter = new GlobalCounter
                {
                    YearNotId = currentYear,
                    MaXNumberInYear = 1
                };
                await _context.GlobalCounters.AddAsync(globalCounter);
            }
            else
            {
                // Increment the counter
                globalCounter.MaXNumberInYear++;
            }

            await _context.SaveChangesAsync(); // Save changes to the database asynchronously

            return globalCounter.MaXNumberInYear;
        }

        // تحويل هذه الدالة لتعمل بشكل غير متزامن
        public async Task<long> GetNextSerialNumInYearAsync(string typeOfCoupon)
        {
            int currentYear = DateTime.Now.Year;

            // البحث عن السجل الخاص بالسنة الحالية
            var globalCounter = await _context.GlobalCounters
                .FirstOrDefaultAsync(c => c.YearNotId == currentYear);

            // إذا لم يكن السجل موجودًا، نقوم بإنشائه
            if (globalCounter == null)
            {
                globalCounter = new GlobalCounter
                {
                    YearNotId = currentYear,
                    MaxSerialNumber1 = 0,
                    MaxSerialNumber2 = 0,
                    MaxSerialNumber3 = 0,
                    MaxSerialNumber4 = 0,
                    MaxSerialNumber5 = 0,
                    MaxSerialNumber6 = 0,
                };
                await _context.GlobalCounters.AddAsync(globalCounter);
            }

            long nextSerialNumber = 0;

            // تحديث القيمة بناءً على نوع الكوبون
            switch (typeOfCoupon)
            {
                case "1":
                    nextSerialNumber = globalCounter.MaxSerialNumber1;
                    break;
                case "2":
                    nextSerialNumber = globalCounter.MaxSerialNumber2;
                    break;
                case "3":
                    nextSerialNumber = globalCounter.MaxSerialNumber3;
                    break;
                case "4":
                    nextSerialNumber = globalCounter.MaxSerialNumber4;
                    break;
                case "5":
                    nextSerialNumber = globalCounter.MaxSerialNumber5;
                    break;
                case "6":
                    nextSerialNumber = globalCounter.MaxSerialNumber6;
                    break;
                
            }

            // حفظ التغييرات في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع الرقم التالي
            return nextSerialNumber;
        }

        // تحويل هذه الدالة لتعمل بشكل غير متزامن
        public async Task<long> UpdateMaxSerialNumAsync(string BeginSerialNum,string TypeOfCoupon,int count)
        {
            int currentYear = DateTime.Now.Year;
            long LongSerialNum=long.Parse(BeginSerialNum);
            // Check if the year exists
            var globalCounter = await _context.GlobalCounters
                .SingleOrDefaultAsync(gc => gc.YearNotId == currentYear);
            long nextSerialNumber = 0;

            if (globalCounter != null)
            {

                switch (TypeOfCoupon)
                {
                    case "1":
                        {
                            globalCounter.MaxSerialNumber1 = LongSerialNum + count;
                            nextSerialNumber = globalCounter.MaxSerialNumber1;
                            break;
                        }
                    case "2":
                        {
                            globalCounter.MaxSerialNumber2 = LongSerialNum + count;
                            nextSerialNumber = globalCounter.MaxSerialNumber2;
                            break;
                        }
                    case "3":
                        {
                            globalCounter.MaxSerialNumber3 = LongSerialNum + count;
                            nextSerialNumber = globalCounter.MaxSerialNumber3;
                            break;
                        }
                    case "4":
                        {
                            globalCounter.MaxSerialNumber4 = LongSerialNum + count;
                            nextSerialNumber = globalCounter.MaxSerialNumber4;
                            break;

                        }
                    case "5":
                        {
                            globalCounter.MaxSerialNumber5 = LongSerialNum + count;
                            nextSerialNumber = globalCounter.MaxSerialNumber5;
                            break;

                        }
                    case "6":
                        {
                            globalCounter.MaxSerialNumber6 = LongSerialNum + count;
                            nextSerialNumber = globalCounter.MaxSerialNumber6;

                            break;

                        }
                    default:
                        
                        break;

                }

                // Increment the counter
               
                await _context.SaveChangesAsync(); // Save changes to the database asynchronously
            }

            return nextSerialNumber; // Ensure we return a valid number
        }
    }
}
