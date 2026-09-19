using InsuranceApp.Domain.Models;
using InsuranceApp.Models.Claim;

namespace InsuranceApp.Mapper
{
    public static class ClaimMapper
    {
        public static Claim ToClaim(this ClaimModel claimModel)
        {

            return new Claim
            {
                Id = claimModel.Id,
                CoverId = claimModel.CoverId,       
                Created = claimModel.Created,
                Name = claimModel.Name,
                Type = claimModel.Type,
                DamageCost = claimModel.DamageCost
            };
        }

        public static ClaimModel ToClaimModel(this Claim claim)
        {
            return new ClaimModel
            {
                Id = claim.Id,
                CoverId = claim.CoverId,
                Created = claim.Created,
                Name = claim.Name,
                Type = claim.Type,
                DamageCost = claim.DamageCost
            };
        }
    }
}
