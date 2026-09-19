
using InsuranceApp.DataAccess;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Helper
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(InsuranceAppDbContext context)
        {
            if (await context.Claims.AnyAsync() && await context.Covers.AnyAsync())
            {
                return;
            }

            var covers = new List<Cover>()
            {
                new Cover()
                {
                    Id = "111",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(7),
                    Type = CoverType.PassengerShip,
                    Premium = 0
                },
                new Cover()
                {
                    Id = "222",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(3),
                    Type = CoverType.Yacht,
                    Premium = 0
                }
            };

            var claims = new List<Claim>()
            {
                new Claim()
                {
                    Id = "szdxfcgv",
                    CoverId = "111",
                    Created = DateTime.Now,
                    Name = "Claim 1",
                    Type = ClaimType.BadWeather,
                    DamageCost = 33000
                },
                new Claim()
                {
                    Id = "dfgh",
                    CoverId = "222",
                    Created = DateTime.Now.AddDays(3),
                    Name = "Claim 2",
                    Type = ClaimType.Collision,
                    DamageCost = 80000
                }
            };

            await context.Claims.AddRangeAsync(claims);
            await context.Covers.AddRangeAsync(covers);

            await context.SaveChangesAsync();
        }
    }
}
