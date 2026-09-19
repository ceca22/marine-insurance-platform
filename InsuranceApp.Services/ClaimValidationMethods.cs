
using InsuranceApp.Shared.Exceptions;

namespace InsuranceApp.Services
{
    public static class ClaimValidationMethods
    {
        public static void EvaluateDamageCost(decimal damageCost)
        {
            var damageCostApproved = damageCost <= 100000;
            if (!damageCostApproved) throw new ClaimException("Damage cost exceeds damage cost limit");
        }

        public static void CheckCoverValidity(
            DateTime claimCreatedDate, 
            DateTime coverStartDate, 
            DateTime coverEndDate)
        {
            var validity = claimCreatedDate >= coverStartDate && claimCreatedDate <= coverEndDate;
            if (!validity) throw new ClaimException("Cover expired");
        }
    }
}
