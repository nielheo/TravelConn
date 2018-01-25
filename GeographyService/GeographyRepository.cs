using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeographyModel;
using Microsoft.EntityFrameworkCore;

namespace GeographyService
{
    public class GeographyRepository : IGeographyRepository
    {
        private GeographyDbContext _db { get; set; }
        //private readonly ILogger _logger;

        //protected LogService _LogService = null;

        public GeographyRepository(GeographyDbContext db)
        {
            _db = db;
            //    _logger = logger;
        }

        public async Task<List<City>> GetCitiesInCountry(string countryCode)
        {
            return await _db.Cities.Where(c => c.CountryCode.ToLower() == countryCode.ToLower()).ToListAsync();
        }

        public async Task<City> GetCityByCode(string code, string countryCode)
        {
            return await _db.Cities.Where(c => 
                c.Code.ToLower() == code.ToLower()
                && c.Country.Code.ToLower() == countryCode.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<City> GetCityByPermalink(string permalink, string countryPermalink)
        {
            return await _db.Cities.Where(c =>
                c.Permalink == permalink
                && c.Country.Permalink.ToLower() == countryPermalink.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<List<Country>> GetCountries()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country> GetCountryByPermalink(string permalink)
        {
            return await _db.Countries.Where(c =>
                c.Permalink.ToLower() == permalink.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryByCode(string code)
        {
            return await _db.Countries.Where(c =>
                c.Code.ToLower() == code.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<List<CityAutocomplete>> GetCityAutocomplete(string name)
        {
            name = name.ToLower();
            return await _db.Cities.Include(c => c.Country)
                .Where(c => c.Name.ToLower().IndexOf(name) > -1)
                .Select(c => new CityAutocomplete
                {
                    City = c.Permalink,
                    Country = c.Country.Permalink,
                    Display = $"{c.Name}, {c.Country.Name}"
                })
                .OrderBy(c => c.Display.ToLower().IndexOf(name))
                .ThenBy(c => c.Display)
                .ToListAsync();
        }
    }
}
