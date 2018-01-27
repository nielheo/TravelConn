using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace GeographyService.Test
{
    [TestClass]
    public class GeographyRepositoryShould
    {
        private readonly GeographyRepository _geographyRepository;

        public GeographyRepositoryShould()
        {
            var dbLogger = new Mock<ILogger<GeographyDbContext>>();

            var options = new DbContextOptionsBuilder<GeographyDbContext>()
                .UseInMemoryDatabase(databaseName: "Geography")
                .Options;

            var db = new GeographyDbContext(options, dbLogger.Object);

            var countryCount = db.Countries.CountAsync().Result;

            if (countryCount == 0)
                db.EnsureSeedData();

            _geographyRepository = new GeographyRepository(db);
        }

        [TestMethod]
        public async Task GetCountries()
        {
            var countries = await _geographyRepository.GetCountries();

            Assert.AreEqual(231, countries.Count);
        }

        [TestMethod]
        public async Task GetCountries_TH()
        {
            //Upper case
            var country = await _geographyRepository.GetCountryByCode("TH");

            Assert.IsNotNull(country);
            Assert.AreEqual("TH", country.Code);
            Assert.AreEqual("Thailand", country.Name);
            Assert.AreEqual("thailand", country.Permalink);

            //Lower case
            country = await _geographyRepository.GetCountryByCode("th");

            Assert.IsNotNull(country);
            Assert.AreEqual("TH", country.Code);
            Assert.AreEqual("Thailand", country.Name);
            Assert.AreEqual("thailand", country.Permalink);
        }

        [TestMethod]
        public async Task GetCountries_ID()
        {
            //Upper case
            var country = await _geographyRepository.GetCountryByCode("ID");

            Assert.IsNotNull(country);
            Assert.AreEqual("ID", country.Code);
            Assert.AreEqual("Indonesia", country.Name);
            Assert.AreEqual("indonesia", country.Permalink);

            //Lower case
            country = await _geographyRepository.GetCountryByCode("id");

            Assert.IsNotNull(country);
            Assert.AreEqual("ID", country.Code);
            Assert.AreEqual("Indonesia", country.Name);
            Assert.AreEqual("indonesia", country.Permalink);
        }

        [TestMethod]
        public async Task GetCountries_XX()
        {
            //Upper case
            var country = await _geographyRepository.GetCountryByCode("XX");

            Assert.IsNull(country);
            
            //Lower case
            country = await _geographyRepository.GetCountryByCode("xx");

            Assert.IsNull(country);
        }

        [TestMethod]
        public async Task GetCountries_null()
        {
            //Null
            var country = await _geographyRepository.GetCountryByCode(null);

            Assert.IsNull(country);

            //Empty string
            country = await _geographyRepository.GetCountryByCode(string.Empty);

            Assert.IsNull(country);
        }
        
        [TestMethod]
        public async Task GetCountryByPermalink_Thailand()
        {
            //Upper case
            var country = await _geographyRepository.GetCountryByPermalink("THAILAND");

            Assert.IsNotNull(country);
            Assert.AreEqual("TH", country.Code);
            Assert.AreEqual("Thailand", country.Name);
            Assert.AreEqual("thailand", country.Permalink);

            //Lower case
            country = await _geographyRepository.GetCountryByPermalink("thailand");

            Assert.IsNotNull(country);
            Assert.AreEqual("TH", country.Code);
            Assert.AreEqual("Thailand", country.Name);
            Assert.AreEqual("thailand", country.Permalink);

            //Camel case
            country = await _geographyRepository.GetCountryByPermalink("Thailand");

            Assert.IsNotNull(country);
            Assert.AreEqual("TH", country.Code);
            Assert.AreEqual("Thailand", country.Name);
            Assert.AreEqual("thailand", country.Permalink);

            //Random case
            country = await _geographyRepository.GetCountryByPermalink("thaIlanD");

            Assert.IsNotNull(country);
            Assert.AreEqual("TH", country.Code);
            Assert.AreEqual("Thailand", country.Name);
            Assert.AreEqual("thailand", country.Permalink);
        }

        [TestMethod]
        public async Task GetCountryByPermalink_New_Zealand()
        {
            //Upper case
            var country = await _geographyRepository.GetCountryByPermalink("NEW-ZEALAND");

            Assert.IsNotNull(country);
            Assert.AreEqual("NZ", country.Code);
            Assert.AreEqual("New Zealand", country.Name);
            Assert.AreEqual("new-zealand", country.Permalink);

            //Lower case
            country = await _geographyRepository.GetCountryByPermalink("new-zealand");

            Assert.IsNotNull(country);
            Assert.AreEqual("NZ", country.Code);
            Assert.AreEqual("New Zealand", country.Name);
            Assert.AreEqual("new-zealand", country.Permalink);

            //Camel case
            country = await _geographyRepository.GetCountryByPermalink("New-Zealand");

            Assert.IsNotNull(country);
            Assert.AreEqual("NZ", country.Code);
            Assert.AreEqual("New Zealand", country.Name);
            Assert.AreEqual("new-zealand", country.Permalink);

            //Random case
            country = await _geographyRepository.GetCountryByPermalink("neW-ZealAnD");

            Assert.IsNotNull(country);
            Assert.AreEqual("NZ", country.Code);
            Assert.AreEqual("New Zealand", country.Name);
            Assert.AreEqual("new-zealand", country.Permalink);
        }


        [TestMethod]
        public async Task GetCountryByPermalink_XXX()
        {
            //Upper case
            var country = await _geographyRepository.GetCountryByPermalink("XXX");

            Assert.IsNull(country);

            //Lower case
            country = await _geographyRepository.GetCountryByPermalink("xxx");

            Assert.IsNull(country);

            //Camel case
            country = await _geographyRepository.GetCountryByPermalink("Xxx");

            Assert.IsNull(country);

            //Random case
            country = await _geographyRepository.GetCountryByPermalink("xXXxX");

            Assert.IsNull(country);
        }

        [TestMethod]
        public async Task GetCountryByPermalink_NullOrEmpty()
        {
            //Null
            var country = await _geographyRepository.GetCountryByPermalink(null);

            Assert.IsNull(country);

            //Empty
            country = await _geographyRepository.GetCountryByPermalink(string.Empty);

            Assert.IsNull(country);

            
            //Empty
            country = await _geographyRepository.GetCountryByPermalink("");

            Assert.IsNull(country);
        }

        [TestMethod]
        public async Task GetCitiesInCountry_TH()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountry("TH");

            Assert.IsNotNull(cities);
            Assert.AreEqual(2070, cities.Count);

            Assert.AreEqual(1, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(0, cities.Where(c => c.Name == "Jakarta").Count());

        }

        [TestMethod]
        public async Task GetCitiesInCountry_ID()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountry("ID");

            Assert.IsNotNull(cities);
            Assert.AreEqual(704, cities.Count);

            Assert.AreEqual(0, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(1, cities.Where(c => c.Name == "Jakarta").Count());

        }

        [TestMethod]
        public async Task GetCitiesInCountry_SG()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountry("SG");

            Assert.IsNotNull(cities);
            Assert.AreEqual(1, cities.Count);

            Assert.AreEqual(0, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(0, cities.Where(c => c.Name == "Jakarta").Count());

        }

        [TestMethod]
        public async Task GetCitiesInCountry_XX()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountry("XX");

            Assert.IsNotNull(cities);
            Assert.AreEqual(0, cities.Count);
        }



        [TestMethod]
        public async Task GetCitiesInCountryByPermalink_Thailand()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountryByPermalink("THAILAND");

            Assert.IsNotNull(cities);
            Assert.AreEqual(2070, cities.Count);

            Assert.AreEqual(1, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(0, cities.Where(c => c.Name == "Jakarta").Count());

            //Lower Case
            cities = await _geographyRepository.GetCitiesInCountryByPermalink("thailand");

            Assert.IsNotNull(cities);
            Assert.AreEqual(2070, cities.Count);

            Assert.AreEqual(1, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(0, cities.Where(c => c.Name == "Jakarta").Count());

        }

        [TestMethod]
        public async Task GetCitiesInCountryByPermalink_Indonesia()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountryByPermalink("INDONESIA");

            Assert.IsNotNull(cities);
            Assert.AreEqual(704, cities.Count);

            Assert.AreEqual(0, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(1, cities.Where(c => c.Name == "Jakarta").Count());

            //Lower Case
            cities = await _geographyRepository.GetCitiesInCountryByPermalink("indonesia");

            Assert.IsNotNull(cities);
            Assert.AreEqual(704, cities.Count);

            Assert.AreEqual(0, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(1, cities.Where(c => c.Name == "Jakarta").Count());

        }

        [TestMethod]
        public async Task GetCitiesInCountryByPermalink_New_Zealand()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountryByPermalink("NEW-ZEALAND");

            Assert.IsNotNull(cities);
            Assert.AreEqual(835, cities.Count);

            Assert.AreEqual(0, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(0, cities.Where(c => c.Name == "Jakarta").Count());

            //Lower Case
            cities = await _geographyRepository.GetCitiesInCountryByPermalink("new-zealand");

            Assert.IsNotNull(cities);
            Assert.AreEqual(835, cities.Count);

            Assert.AreEqual(0, cities.Where(c => c.Name == "Bangkok").Count());
            Assert.AreEqual(0, cities.Where(c => c.Name == "Jakarta").Count());

        }

        [TestMethod]
        public async Task GetCitiesInCountryByPermalink_XX()
        {
            //Upper Case
            var cities = await _geographyRepository.GetCitiesInCountryByPermalink("XX");

            Assert.IsNotNull(cities);
            Assert.AreEqual(0, cities.Count);
        }

        [TestMethod]
        public async Task GetCityByCode_TH_Bangkok()
        {
            //Upper Case
            var city = await _geographyRepository.GetCityByCode("604", "TH");

            Assert.IsNotNull(city);
            Assert.AreEqual("604", city.Code);
            Assert.AreEqual("Bangkok", city.Name);
            Assert.AreEqual("TH", city.CountryCode);
        }

        [TestMethod]
        public async Task GetCityByCode_ID_Jakarta()
        {
            //Upper Case
            var city = await _geographyRepository.GetCityByCode("1704", "ID");

            Assert.IsNotNull(city);
            Assert.AreEqual("1704", city.Code);
            Assert.AreEqual("Jakarta", city.Name);
            Assert.AreEqual("ID", city.CountryCode);
            //Assert.AreEqual(0, cities.Count);
        }

        [TestMethod]
        public async Task GetCityByCode_TH_Jakarta()
        {
            //Upper Case
            var city = await _geographyRepository.GetCityByCode("1704", "TH");

            Assert.IsNull(city);
            //Assert.AreEqual(0, cities.Count);
        }

        [TestMethod]
        public async Task GetCityByCode_Null()
        {
            var city = await _geographyRepository.GetCityByCode("1704", null);
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByCode("1704", string.Empty);
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByCode(null, "TH");
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByCode(string.Empty, "TH");
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByCode(null, null);
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByCode(string.Empty, string.Empty);
            Assert.IsNull(city);
        }


        [TestMethod]
        public async Task GetCityByPermalink_TH_Bangkok()
        {
            //Upper Case
            var city = await _geographyRepository.GetCityByPermalink("BANGKOK", "THAILAND");

            Assert.IsNotNull(city);
            Assert.AreEqual("604", city.Code);
            Assert.AreEqual("Bangkok", city.Name);
            Assert.AreEqual("TH", city.CountryCode);

            //Lower Case
            city = await _geographyRepository.GetCityByPermalink("bangkok", "thailand");

            Assert.IsNotNull(city);
            Assert.AreEqual("604", city.Code);
            Assert.AreEqual("Bangkok", city.Name);
            Assert.AreEqual("TH", city.CountryCode);
        }

        [TestMethod]
        public async Task GetCityByPermalink_ID_Jakarta()
        {
            //Upper Case
            var city = await _geographyRepository.GetCityByPermalink("JAKARTA", "INDONESIA");

            Assert.IsNotNull(city);
            Assert.AreEqual("1704", city.Code);
            Assert.AreEqual("Jakarta", city.Name);
            Assert.AreEqual("ID", city.CountryCode);

            //Lower Case
            city = await _geographyRepository.GetCityByPermalink("jakarta", "indonesia");

            Assert.IsNotNull(city);
            Assert.AreEqual("1704", city.Code);
            Assert.AreEqual("Jakarta", city.Name);
            Assert.AreEqual("ID", city.CountryCode);
        }

        [TestMethod]
        public async Task GetCityByPermalink_TH_Jakarta()
        {
            //Upper Case
            var city = await _geographyRepository.GetCityByPermalink("JAKARTA", "THAILAND");

            Assert.IsNull(city);

            //Lower Case
            city = await _geographyRepository.GetCityByPermalink("jakarta", "thailand");

            Assert.IsNull(city);
        }

        [TestMethod]
        public async Task GetCityByPermalink_Null()
        {
            var city = await _geographyRepository.GetCityByPermalink("Jakarta", null);
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByPermalink("bangkok", string.Empty);
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByPermalink(null, "thailand");
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByPermalink(string.Empty, "indonesia");
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByPermalink(null, null);
            Assert.IsNull(city);

            city = await _geographyRepository.GetCityByPermalink(string.Empty, string.Empty);
            Assert.IsNull(city);
        }

    }
}