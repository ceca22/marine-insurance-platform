
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.Models;

namespace Claims.Tests
{
    public static class InsuranceAppTestData
    {
        public static List<Claim> Claims => new()
        {
            new Claim()
                {
                    Id = "szdxfcgv",
                    CoverId = "111",
                    Created = DateTime.Now.AddDays(2),
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

        public static List<Cover> Covers => new()
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
    }
}
