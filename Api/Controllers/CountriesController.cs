using GeographyModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Fabric;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Countries")]
    public class CountriesController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly StatelessServiceContext serviceContext;
        private readonly ConfigSettings configSettings;
        private readonly FabricClient fabricClient;

        public CountriesController(
            StatelessServiceContext serviceContext,
            HttpClient httpClient,
            FabricClient fabricClient,
            ConfigSettings settings
            )
        {
            this.serviceContext = serviceContext;
            this.httpClient = httpClient;
            this.configSettings = settings;
            this.fabricClient = fabricClient;
        }

        private async Task<IActionResult> Get<T>(string proxyUrl)
        {
            this.httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return this.StatusCode((int)response.StatusCode);
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<T>(jsonString);
            return this.Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            string serviceUri = $"{this.serviceContext.CodePackageActivationContext.ApplicationName}/{this.configSettings.GeographyServiceName}".Replace("fabric:/", "");

            string proxyUrl = $"http://localhost:{this.configSettings.ReverseProxyPort}/{serviceUri}/api/countries";

            return await Get<List<Country>>(proxyUrl);
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<IActionResult> GetCountyByCodeAsync(string code)
        {
            string serviceUri = $"{this.serviceContext.CodePackageActivationContext.ApplicationName}/{this.configSettings.GeographyServiceName}".Replace("fabric:/", "");

            string proxyUrl = $"http://localhost:{this.configSettings.ReverseProxyPort}/{serviceUri}/api/countries/{code}";

            return await Get<Country>(proxyUrl);
        }

        [HttpGet]
        [Route("{code}/cities")]
        public async Task<IActionResult> GetCitiesAsync(string code)
        {
            string serviceUri = $"{this.serviceContext.CodePackageActivationContext.ApplicationName}/{this.configSettings.GeographyServiceName}".Replace("fabric:/", "");

            string proxyUrl = $"http://localhost:{this.configSettings.ReverseProxyPort}/{serviceUri}/api/countries/{code}/cities";

            return await Get<List<City>>(proxyUrl);
        }

        [HttpGet]
        [Route("permalink/{permalink}/cities")]
        public async Task<IActionResult> GetCountryPermalinkAsync(string permalink)
        {
            string serviceUri = $"{this.serviceContext.CodePackageActivationContext.ApplicationName}/{this.configSettings.GeographyServiceName}".Replace("fabric:/", "");

            string proxyUrl = $"http://localhost:{this.configSettings.ReverseProxyPort}/{serviceUri}/api/countries/permalink/{permalink}/cities";

            return await Get<List<City>>(proxyUrl);
        }

        [HttpGet]
        [Route("permalink/{countryPermalink}/cities/{cityPermalink}")]
        public async Task<IActionResult> GetCityPermalinkAsync(string countryPermalink, string cityPermalink)
        {
            string serviceUri = $"{this.serviceContext.CodePackageActivationContext.ApplicationName}/{this.configSettings.GeographyServiceName}".Replace("fabric:/", "");

            string proxyUrl = $"http://localhost:{this.configSettings.ReverseProxyPort}/{serviceUri}/api/countries/permalink/{countryPermalink}/cities/{cityPermalink}";

            return await Get<City>(proxyUrl);
        }
    }
}