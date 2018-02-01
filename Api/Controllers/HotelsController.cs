namespace Api.Controllers
{
    using EanHotel.Domain;
    using EanHotel.Domain.Request;
    using EanHotel.Domain.Response;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.ServiceFabric.Services.Remoting.Client;
    
    using System;
    using System.Fabric;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/Hotels")]
    public class HotelsController : Controller
    {
        private readonly ConfigSettings configSettings;
        private readonly StatelessServiceContext serviceContext;

        public HotelsController(StatelessServiceContext serviceContext, ConfigSettings settings)
        {
            this.serviceContext = serviceContext;
            this.configSettings = settings;
        }

        [Route("/{country}/{city}/hotels")]
        public async Task<IActionResult> GetHotel(string country, string city, 
            DateTime? checkIn, DateTime? checkOut)
        {
            try
            {
                string serviceUri = this.serviceContext.CodePackageActivationContext.ApplicationName + "/" + this.configSettings.EanHotelServiceName;

                IEanHotelService proxy = ServiceProxy.Create<IEanHotelService>(new Uri(serviceUri));

                HotelAvailRq request = new HotelAvailRq
                {
                    City = city,
                    Country = country,
                    CheckIn = checkIn ?? DateTime.Today.AddDays(90),
                    CheckOut = checkOut ?? DateTime.Today.AddDays(92),
                    Occupancies = new System.Collections.Generic.List<RoomOccupancy>
                {
                    new RoomOccupancy
                    {
                        AdultCount = 2
                    }
                }
                };

                var result = await proxy.HotelAvailAsync(request);

                return this.Json(result);
            }
            catch(Exception ex)
            {
                var a = ex.Message;
                return View(ex.Message);
            }
        }
    }
}