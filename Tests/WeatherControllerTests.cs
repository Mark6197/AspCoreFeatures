using AspCoreFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class WeatherControllerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();// Initializes the webHost
        private HttpClient _httpClient;//Http client used to send requests to the contoller
        [TestInitialize]
        public async Task SetUp()
        {
            _httpClient = _testHostFixture.Client;
        }

        [TestMethod]
        public async Task Search_Flight_With_No_Query_Parameters()
        {
            var response = await _httpClient.GetAsync("WeatherForecast");
            var responseContent = await response.Content.ReadAsStringAsync();
            List<WeatherForecast> weatherForecastListResult = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.AreEqual(weatherForecastListResult.Count, 5);
        }
    }
}
