using System;
using System.Threading.Tasks;
using EanHotel.Domain;
using EanHotel.Domain.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EanHotel.Connector.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            EanHotelService eanHotelService = new EanHotelService();
            var res = await eanHotelService.HotelAvailAsync(new HotelAvailRq
            {
                City = "bangkok",
                Country = "TH",
                CheckIn = DateTime.Today.AddDays(90),
                CheckOut = DateTime.Today.AddDays(92),
                Currency = "THB",
                Occupancies = new System.Collections.Generic.List<RoomOccupancy>
                { 
                    new RoomOccupancy
                    {
                        AdultCount = 1,
                    }
                }
            });
        }
    }
}
