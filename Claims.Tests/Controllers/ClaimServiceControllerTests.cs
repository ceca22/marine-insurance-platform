
using InsuranceApp.Auditing.Auditing;
using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.Models;
using InsuranceApp.Mapper;
using InsuranceApp.Models.Claim;
using InsuranceApp.Services.Implementations;
using InsuranceApp.Shared.Exceptions;
using Moq;
using Xunit;

namespace Claims.Tests.Controllers
{
    public class ClaimServiceControllerTests
    {
        private readonly ClaimService _claimService;
        private readonly Mock<IClaimRepository> _claimRepository;
        private readonly Mock<ICoverRepository> _coverRepository;
        private readonly Mock<IAuditer> _auditer;

        public ClaimServiceControllerTests()
        {
            _claimRepository = new Mock<IClaimRepository>();
            _coverRepository = new Mock<ICoverRepository>();
            _auditer = new Mock<IAuditer>();

            _claimService = new ClaimService(
                _claimRepository.Object,
                _coverRepository.Object,
                _auditer.Object
            );
        }

        [Fact]
        public async Task GetClaimById_ReturnsClaim()
        {

            _claimRepository.Setup(x => x.GetAsync("szdxfcgv")).ReturnsAsync(InsuranceAppTestData.Claims[0]);

            var result = await _claimService.GetByIdAsync("szdxfcgv");

            Assert.NotNull(result);
            Assert.Equal("szdxfcgv", result.Id);
        }

        [Fact]
        public async Task GetById_WhenClaimDoesntExist_ReturnsNull()
        {

            _claimRepository.Setup(x => x.GetAsync("szdxfcgv1")).ReturnsAsync((Claim?)null);

            var result = await _claimService.GetByIdAsync("szdxfcgv1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfClaims()
        {

            _claimRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(InsuranceAppTestData.Claims);

            var result = (await _claimService.GetAllAsync()).ToList();

            Assert.Equal(InsuranceAppTestData.Claims.Count, result.Count);
        }

        [Fact]
        public async Task DeleteEntity_WhenClaimExists_ReturnsTrue()
        {

            _claimRepository.Setup(x => x.GetAsync("szdxfcgv")).ReturnsAsync(InsuranceAppTestData.Claims[0]);

            var result = await _claimService.DeleteEntityAsync("szdxfcgv");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEntity_WhenClaimDoesntExists_ReturnsFalse()
        {

            _claimRepository.Setup(x => x.GetAsync("szdxfcgv1")).ReturnsAsync((Claim?)null);

            var result = await _claimService.DeleteEntityAsync("szdxfcgv1");

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_WhenClaimObjectIsValid_ReturnsClaimObject()
        {
            var cover = InsuranceAppTestData.Covers[0];

            var claimObject = new ClaimModel()
            {
                CoverId = cover.Id,
                Created = DateTime.Now.AddDays(1),
                Name = "Claim 1",
                Type = ClaimType.BadWeather,
                DamageCost = 33000
            };

            _coverRepository.Setup(x => x.GetAsync(cover.Id)).ReturnsAsync(InsuranceAppTestData.Covers[0]);
            _claimRepository.Setup(x => x.CreateAsync(It.IsAny<Claim>())).ReturnsAsync((Claim claim) => claim);

            var result = await _claimService.CreateAsync(claimObject);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_WhenCoverDoesntExist_ThrowsClaimException()
        {
            var claimModel = InsuranceAppTestData.Claims[0].ToClaimModel();
            claimModel.CoverId = "555";

            _coverRepository.Setup(x => x.GetAsync(claimModel.CoverId)).ReturnsAsync((Cover?)null);

            var exception = await Assert.ThrowsAsync<ClaimException>(() => _claimService.CreateAsync(claimModel));

            //checking the result
            Assert.Equal("Cover doesn't exist", exception.Message);
            //making sure the service stopped and never made a call to create a claim since the object is not valid
            _claimRepository.Verify(x => x.CreateAsync(It.IsAny<Claim>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenDamageCostExceedsDamageCostLimit_ThrowsClaimException()
        {
            var claimModel = InsuranceAppTestData.Claims[0].ToClaimModel();
            claimModel.DamageCost = 101000;

            var exception = await Assert.ThrowsAsync<ClaimException>(() => _claimService.CreateAsync(claimModel));

            //checking the result
            Assert.Equal("Damage cost exceeds damage cost limit", exception.Message);
            //making sure the service stopped and never made a call to create a claim since the object is not valid
            _claimRepository.Verify(x => x.CreateAsync(It.IsAny<Claim>()), Times.Never);
            //making sure the service stopped and never made a call to get the cover since the object is not valid
            _coverRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenCoverIsExpired_ThrowsClaimException()
        {
            var cover = InsuranceAppTestData.Covers[0];
            var claimModel = InsuranceAppTestData.Claims[0].ToClaimModel();
            claimModel.Created = DateTime.Now.AddYears(2);

            _coverRepository.Setup(x => x.GetAsync(cover.Id)).ReturnsAsync(cover);

            var exception = await Assert.ThrowsAsync<ClaimException>(() => _claimService.CreateAsync(claimModel));

            //checking the result
            Assert.Equal("Cover expired", exception.Message);
            //making sure the service stopped and never made a call to create a claim since the object is not valid
            _claimRepository.Verify(x => x.CreateAsync(It.IsAny<Claim>()), Times.Never);
        }
    }
}
