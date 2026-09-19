using InsuranceApp.Domain.Models;
using InsuranceApp.Models.Cover;

namespace InsuranceApp.Mapper
{
    public static class CoverMapper
    {
        public static Cover ToCover(this CoverModel coverModel)
        {

            return new Cover
            {
                Id = coverModel.Id,
                StartDate = coverModel.StartDate,
                EndDate = coverModel.EndDate,
                Type = coverModel.Type,
                Premium = coverModel.Premium
            };
        }

        public static CoverModel ToCoverModel(this Cover cover)
        {
            return new CoverModel
            {
                Id = cover.Id,
                StartDate = cover.StartDate,
                EndDate = cover.EndDate,
                Type = cover.Type,
                Premium = cover.Premium
            };
        }
    }
}
