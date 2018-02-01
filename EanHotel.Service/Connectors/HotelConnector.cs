using EanHotel.Domain;
using EanHotel.Domain.Request;
using EanHotel.Domain.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Service.Connectors
{
    public class HotelConnector : BaseConnector, IEanHotelService
    {
        public async Task<HotelAvailRs> HotelAvailAsync(HotelAvailRq request)
        {
            var result = await SubmitAsync($"locale={request.Locale ?? "en_US"}" +
                        $"&numberOfResults=200" +
                        $"&currencyCode={request.Currency ?? "USD"}" +
                        $"&city={request.City}" +
                        $"&countryCode={request.Country}" +
                        $"&arrivalDate={request.CheckIn.ToString("MM/dd/yyyy")}" +
                        $"&departureDate={request.CheckOut.ToString("MM/dd/yyyy")}" +
                        $"&{OccupancyToString(request.Occupancies)}" +
                        $"&maxRatePlanCount=3", RequestType.HotelAvail);

            return JsonConvert.DeserializeObject<HotelAvailRs>(result, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        private string OccupancyToString(List<RoomOccupancy> occupancies)
        {
            List<string> sRooms = new List<string>();

            int idx = 1;

            foreach (RoomOccupancy room in occupancies)
            {
                sRooms.Add($"Room{idx++}={room.AdultCount}{(((room.ChildAges?.Count ?? 0) > 0) ? "," + string.Join(",", room.ChildAges) : "")}");
            }

            return string.Join("&", sRooms);
        }
    }
}
