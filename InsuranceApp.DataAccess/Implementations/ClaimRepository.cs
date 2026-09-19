using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.DataAccess.Implementations
{
    public class ClaimRepository: IClaimRepository
    {
        private readonly ILogger<ClaimRepository> _logger; 
        private readonly InsuranceAppDbContext _insuranceAppDbContext;

        public ClaimRepository(ILogger<ClaimRepository> logger, InsuranceAppDbContext insuranceAppDbContext)
        {
            _logger = logger;
            _insuranceAppDbContext = insuranceAppDbContext;
        } 

        public async Task<List<Claim>> GetAllAsync()
        {
            return await _insuranceAppDbContext.Claims
                .ToListAsync();
        }

        public async Task<Claim?> GetAsync(string id)
        {
            return await _insuranceAppDbContext.Claims
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Claim> CreateAsync(Claim claim)
        {
            await _insuranceAppDbContext.Claims.AddAsync(claim);
            await _insuranceAppDbContext.SaveChangesAsync();
            
            return claim;
        }

        public async Task DeleteAsync(Claim claim)
        {
            _insuranceAppDbContext.Claims
               .Remove(claim);
            
            await _insuranceAppDbContext.SaveChangesAsync();
        }
    }
}
