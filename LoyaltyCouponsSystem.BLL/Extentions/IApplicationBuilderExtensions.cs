using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyCouponsSystem.BLL.Settings;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace LoyaltyCouponsSystem.BLL.Extentions
{
    public static class IApplicationBuilderExtensions
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;


            var context = services.GetRequiredService<ApplicationDbContext>();
            var cloudSetting = services.GetRequiredService<IOptions<MyAppSetting>>();


            if (cloudSetting.Value.AutoMigrateDatabase)
            {
                context.Database.Migrate();
               
                


            }

        }

    }
}
