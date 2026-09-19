
using InsuranceApp.Auditing.Auditing;
using InsuranceApp.DataAccess.Implementations;
using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.Models;
using InsuranceApp.Mapper;
using InsuranceApp.Models.Claim;
using InsuranceApp.Services.Implementations;
using InsuranceApp.Services.Interfaces;
using InsuranceApp.Shared.Exceptions;
using Moq;
using Xunit;

namespace Claims.Tests.Controllers
{
    public class CoverServiceControllerTests
    {
        private readonly CoverService _coverService;
        private readonly Mock<ICoverRepository> _coverRepository;
        private readonly Mock<IAuditer> _auditer;

        public CoverServiceControllerTests()
        {
            _coverRepository = new Mock<ICoverRepository>();
            _auditer = new Mock<IAuditer>();

            _coverService = new CoverService(
                _coverRepository.Object,
                _auditer.Object
            );
        }

        [Fact]
        public async Task GetCoverById_ReturnsCover()
        {

            _coverRepository.Setup(x => x.GetAsync("111")).ReturnsAsync(InsuranceAppTestData.Covers[0]);

            var result = await _coverService.GetByIdAsync("111");

            Assert.NotNull(result);
            Assert.Equal("111", result.Id);
        }

        [Fact]
        public async Task GetById_WhenCoverDoesntExist_ReturnsNull()
        {

            _coverRepository.Setup(x => x.GetAsync("1112")).ReturnsAsync((Cover?)null);

            var result = await _coverService.GetByIdAsync("1112");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfCovers()
        {

            _coverRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(InsuranceAppTestData.Covers);

            var result = (await _coverService.GetAllAsync()).ToList();

            Assert.Equal(InsuranceAppTestData.Covers.Count, result.Count);
        }

        [Fact]
        public async Task DeleteEntity_WhenCoverExists_ReturnsTrue()
        {

            _coverRepository.Setup(x => x.GetAsync("111")).ReturnsAsync(InsuranceAppTestData.Covers[0]);

            var result = await _coverService.DeleteEntityAsync("111");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEntity_WhenCoverDoesntExists_ReturnsFalse()
        {

            _coverRepository.Setup(x => x.GetAsync("1113")).ReturnsAsync((Cover?)null);

            var result = await _coverService.DeleteEntityAsync("1113");

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_WhenCoverObjectIsValid_ReturnsCoverObject()
        {
            var coverModel = InsuranceAppTestData.Covers[0].ToCoverModel();

            _coverRepository.Setup(x => x.CreateAsync(It.IsAny<Cover>())).ReturnsAsync((Cover cover) => cover);

            var result = await _coverService.CreateAsync(coverModel);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_WhenCoverStartDateIsInThePast_ThrowsCoverException()
        {
            var coverModel = InsuranceAppTestData.Covers[0].ToCoverModel();
            coverModel.StartDate = DateTime.Now.AddMonths(-10);

            var exception = await Assert.ThrowsAsync<CoverException>(() => _coverService.CreateAsync(coverModel));

            //checking the result
            Assert.Equal("Invalid start date", exception.Message);
            //making sure the service stopped and never made a call to create a cover since the object is not valid
            _coverRepository.Verify(x => x.CreateAsync(It.IsAny<Cover>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenTotalInsurancePeriodExceedsOneYear_ThrowsCoverException()
        {
            var coverModel = InsuranceAppTestData.Covers[0].ToCoverModel();
            coverModel.EndDate = coverModel.StartDate.AddYears(2);

            var exception = await Assert.ThrowsAsync<CoverException>(() => _coverService.CreateAsync(coverModel));

            //checking the result
            Assert.Equal("Cover durability should be 1 year", exception.Message);
            //making sure the service stopped and never made a call to create a claim since the object is not valid
            _coverRepository.Verify(x => x.CreateAsync(It.IsAny<Cover>()), Times.Never);
        }

        //[Fact]
        //public async Task CreateAsync_WhenCoverIsExpired_ThrowsClaimException()
        //{
        //    var cover = InsuranceAppTestData.Covers[0];
        //    var claimObject = new ClaimModel()
        //    {
        //        CoverId = cover.Id,
        //        Created = DateTime.Now.AddMonths(8),
        //        Name = "Claim 1",
        //        Type = ClaimType.BadWeather,
        //        DamageCost = 33000
        //    };

        //    _coverRepository.Setup(x => x.GetAsync(cover.Id)).ReturnsAsync(cover);

        //    var exception = await Assert.ThrowsAsync<ClaimException>(() => _claimService.CreateAsync(claimObject));

        //    //checking the result
        //    Assert.Equal("Cover is expired", exception.Message);
        //    //making sure the service stopped and never made a call to create a claim since the object is not valid
        //    _claimRepository.Verify(x => x.CreateAsync(It.IsAny<Claim>()), Times.Never);
        //}
    }
}
