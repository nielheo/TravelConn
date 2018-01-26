using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeographyModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeographyService.Controllers
{
    [Produces("application/json")]
    [Route("api/Countries")]
    public class CountriesController : Controller
    {
        IGeographyRepository _GeoRep;

        public CountriesController(IGeographyRepository _GeoRep)
        {
            this._GeoRep = _GeoRep;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<Country>> GetAll()
        {
            return await _GeoRep.GetCountries();
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<Country> GetByCode(string code)
        {
            return await _GeoRep.GetCountryByCode(code);
        }

        [HttpGet]
        [Route("{countryCode}/cities")]
        public async Task<List<City>> GetCities(string countryCode)
        {
            return await _GeoRep.GetCitiesInCountry(countryCode);
        }

        [HttpGet]
        [Route("permalink/{permalink}")]
        public async Task<Country> GetCountryPermalink(string permalink)
        {
            return await _GeoRep.GetCountryByPermalink(permalink);
        }

        [HttpGet]
        [Route("permalink/{permalink}/cities")]
        public async Task<List<City>> GetCitiesPermalink(string permalink)
        {
            return await _GeoRep.GetCitiesInCountryByPermalink(permalink);
        }

        [HttpGet]
        [Route("permalink/{countrypermalink}/cities/{cityPermalink}")]
        public async Task<City> GetCityPermalink(string countrypermalink, string cityPermalink)
        {
            return await _GeoRep.GetCityByPermalink(countrypermalink, cityPermalink);
        }


        [HttpGet]
        [Route("/cityAutocomplete/{name}")]
        public async Task<List<CityAutocomplete>> CityAutocomplete(string name)
        {
            return await _GeoRep.GetCityAutocomplete(name);
        }
    }
}