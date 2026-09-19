using InsuranceApp.Auditing.Auditing;

namespace InsuranceApp.Auditing
{
    public class Auditer: IAuditer
    {
        private readonly AuditContext _auditContext;

        public Auditer(AuditContext auditContext)
        {
            _auditContext = auditContext;
        }

        public async Task AuditClaimAsync(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            await _auditContext.AddAsync(claimAudit);
            await _auditContext.SaveChangesAsync();
        }
        
        public async Task AuditCoverAsync(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            await _auditContext.AddAsync(coverAudit);
            await _auditContext.SaveChangesAsync();
        }
    }
}
