using InsuranceApp.DataAccess;
using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Claims.DataAccess.Implementations
{
    public class CoverRepository: ICoverRepository
    {
        private readonly ILogger<CoverRepository> _logger;
        private readonly InsuranceAppDbContext _insuranceAppDbContext;
        public CoverRepository(ILogger<CoverRepository> logger, InsuranceAppDbContext insuranceAppDbContext)
        {
            _logger = logger;
            _insuranceAppDbContext = insuranceAppDbContext;
        }
        public async Task<List<Cover>> GetAllAsync()
        {
            return await _insuranceAppDbContext.Covers
                .ToListAsync();
        }

        public async Task<Cover?> GetAsync(string id)
        {
            return await _insuranceAppDbContext.Covers
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cover> CreateAsync(Cover cover)
        {
            await _insuranceAppDbContext.Covers.AddAsync(cover);
            await _insuranceAppDbContext.SaveChangesAsync();

            return cover;
        }

        public async Task DeleteAsync(Cover cover)
        {
            _insuranceAppDbContext.Covers
               .Remove(cover);
            await _insuranceAppDbContext.SaveChangesAsync();
        }
    }
}
