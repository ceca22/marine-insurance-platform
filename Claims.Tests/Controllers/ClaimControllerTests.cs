using InsuranceApp.Models.Claim;
using System.Net.Http.Json;
using Xunit;

namespace Claims.Tests.Controllers
{
    public class ClaimControllerTests: IClassFixture<InsuranceAppFactory>
    {
        private readonly HttpClient _httpClient;

        public ClaimControllerTests(InsuranceAppFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Claim()
        {

            var response = await _httpClient.GetAsync("/Claim");

            response.EnsureSuccessStatusCode();

            var claim = await response.Content
                .ReadFromJsonAsync<List<ClaimModel>>(TestJsonOptions.Default);


            //TODO: Apart from ensuring 200 OK being returned, what else can be asserted?
            //Response: we can check the response object
            Assert.NotNull(claim);
            Assert.NotEmpty(claim);
        }

    }
}
