using GeographyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeographyService
{
    public interface IGeographyRepository
    {
        Task<Country> GetCountryByCode(string code);
        Task<Country> GetCountryByPermalink(string permalink);
        Task<List<Country>> GetCountries();

        Task<List<City>> GetCitiesInCountry(string countryCode);
        Task<City> GetCityByCode(string code, string countryCode);
        Task<City> GetCityByPermalink(string permalink, string countryPermalink);

        Task<List<CityAutocomplete>> GetCityAutocomplete(string name);
    }
}
