using InsuranceApp.Auditing.Auditing;
using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Mapper;
using InsuranceApp.Models.Cover;
using InsuranceApp.Services.Interfaces;

namespace InsuranceApp.Services.Implementations
{
    public class CoverService: ICoverService
    {
        private ICoverRepository _coverRepository;
        private readonly IAuditer _auditer;

        public CoverService(ICoverRepository coverRepository, IAuditer auditer)
        {
            _coverRepository = coverRepository;
            _auditer = auditer;
        }

        public async Task<CoverModel> CreateAsync(CoverModel model)
        {
            model.Id = Guid.NewGuid().ToString();

            CoverValidationMethods.CheckCoverDateValidity(model.StartDate);
            CoverValidationMethods.CheckCoverDurabilityValidity(model.StartDate, model.EndDate);

            model.Premium = ComputePremium(model.StartDate, model.EndDate, model.Type);

            var cover = model.ToCover();
            var created = await _coverRepository.CreateAsync(cover);

            await _auditer.AuditCoverAsync(cover.Id, "POST");

            return created.ToCoverModel();
        }

        public async Task<bool> DeleteEntityAsync(string id)
        {
            var cover = await _coverRepository.GetAsync(id);
            if (cover == null)
            {
                return false;
            }

            await _coverRepository.DeleteAsync(cover);
            await _auditer.AuditCoverAsync(cover.Id, "DELETE");

            return true;
        }

        public async Task<IEnumerable<CoverModel>> GetAllAsync()
        {
            var covers = await _coverRepository.GetAllAsync();

            return covers.Select(x => x.ToCoverModel());
        }

        public async Task<CoverModel?> GetByIdAsync(string id)
        {
            var cover = await _coverRepository.GetAsync(id);
            if (cover == null)
            {
                return null;
            }

            return cover.ToCoverModel();
        }

        public decimal GetPremiumValue(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            var result = ComputePremium(startDate, endDate, coverType);

            return result;
        }

        //other
        public decimal ComputePremium(
            DateTime startDate,
            DateTime endDate,
            CoverType coverType)
        {
            var insuranceDays = (endDate - startDate).Days;

            if (insuranceDays <= 0)
            {
                return 0;
            }

            var dailyRate = 1250m * GetTypeMultiplier(coverType);

            var first30Days = Math.Min(insuranceDays, 30);

            var next150Days = Math.Min(
                Math.Max(insuranceDays - 30, 0),
                150);

            var remainingDays = Math.Max(
                insuranceDays - 180,
                0);

            var secondPeriodMultiplier =
                coverType == CoverType.Yacht ? 0.95m : 0.98m;

            var remainingPeriodMultiplier =
                coverType == CoverType.Yacht ? 0.92m : 0.97m;

            return
                first30Days * dailyRate +
                next150Days * dailyRate * secondPeriodMultiplier +
                remainingDays * dailyRate * remainingPeriodMultiplier;
        }

        private decimal GetTypeMultiplier(CoverType coverType)
        {
            return coverType switch
            {
                CoverType.Yacht => 1.10m,
                CoverType.PassengerShip => 1.20m,
                CoverType.Tanker => 1.50m,
                _ => 1.30m
            };
        }
    }
}
