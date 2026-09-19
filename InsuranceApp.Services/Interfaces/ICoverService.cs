

using InsuranceApp.Domain.Enums;
using InsuranceApp.Models.Cover;

namespace InsuranceApp.Services.Interfaces
{
    public interface ICoverService:IService<CoverModel>
    {
        decimal GetPremiumValue(DateTime startDate, DateTime endDate, CoverType coverType);
    }
}
