using InsuranceApp.Auditing.Auditing;
using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Domain.Models;
using InsuranceApp.Mapper;
using InsuranceApp.Models.Claim;
using InsuranceApp.Services.Interfaces;
using InsuranceApp.Shared.Exceptions;

namespace InsuranceApp.Services.Implementations
{
    public class ClaimService: IClaimService
    {
        private IClaimRepository _claimRepository;
        private ICoverRepository _coverRepository;
        private readonly IAuditer _auditer; 

        public ClaimService(IClaimRepository claimRepository, ICoverRepository coverRepository, IAuditer auditer)
        {
            _claimRepository = claimRepository;
            _coverRepository = coverRepository;
            _auditer = auditer;
        }

        public async Task<ClaimModel> CreateAsync(ClaimModel model)
        {
            ClaimValidationMethods.EvaluateDamageCost(model.DamageCost);
            
            var coverModel = await GetCover(model.CoverId);
            
            if (coverModel == null) throw new ClaimException("Cover doesn't exist");
            
            ClaimValidationMethods.CheckCoverValidity(
                    model.Created, coverModel.StartDate, coverModel.EndDate);

            model.Id = Guid.NewGuid().ToString();
            var claim = model.ToClaim();
            var created = await _claimRepository.CreateAsync(claim);

            await _auditer.AuditClaimAsync(claim.Id, "POST");

            return created.ToClaimModel();
        }

        public async Task<bool> DeleteEntityAsync(string id)
        {
            var claim = await _claimRepository.GetAsync(id);
            if (claim == null)
            {
                return false;
            }

            await _claimRepository.DeleteAsync(claim);
            await _auditer.AuditClaimAsync(id, "DELETE");
            
            return true;
        }

        public async Task<IEnumerable<ClaimModel>> GetAllAsync()
        {
            var claims = await _claimRepository.GetAllAsync();

            return claims.Select(x => x.ToClaimModel());
        }

        public async Task<ClaimModel?> GetByIdAsync(string id)
        {
            var claim = await _claimRepository.GetAsync(id);
            if (claim == null)
            {
               return null;
            }

            return claim.ToClaimModel();
        }

        //other
        private async Task<Cover?> GetCover(string id)
        {
            return await _coverRepository.GetAsync(id);
        }
    }
}
