using InsuranceApp.Shared.Exceptions;

namespace InsuranceApp.Services
{
    public static class CoverValidationMethods
    {
        public static void CheckCoverDateValidity(DateTime startDate)
        {
            if (startDate < DateTime.Now.Date) throw new CoverException("Invalid start date");
        }

        public static void CheckCoverDurabilityValidity(DateTime startDate, DateTime endDate)
        {
            if (endDate > startDate.AddYears(1)) throw new CoverException("Cover durability should be 1 year");
        }
    }
}
