using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            //var a = new string[] { "value1", "value2" };

            //return this.Json(a);

            string serviceUri = $"{this.serviceContext.CodePackageActivationContext.ApplicationName}/{this.configSettings.GeographyServiceName}".Replace("fabric:/", "");

            string proxyUrl = $"http://localhost:{this.configSettings.ReverseProxyPort}/{serviceUri}/api/countries";

            HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return this.StatusCode((int)response.StatusCode);
            }

            return this.Ok(await response.Content.ReadAsStringAsync());
        }
    }
}