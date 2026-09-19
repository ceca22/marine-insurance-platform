using InsuranceApp.Models.Cover;
using System.Net.Http.Json;
using Xunit;

namespace Claims.Tests.Controllers
{
    public class CoverControllerTests: IClassFixture<InsuranceAppFactory>
    {
        private readonly HttpClient _httpClient;

        public CoverControllerTests(InsuranceAppFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Cover()
        {

            var response = await _httpClient.GetAsync("/Cover");

            response.EnsureSuccessStatusCode();

            var cover = await response.Content
                .ReadFromJsonAsync<List<CoverModel>>(TestJsonOptions.Default);

            Assert.NotNull(cover);
            Assert.NotEmpty(cover);
        }

    }
}
