using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Domain.Request
{
    

    public class HotelAvailRq : BaseRequest
    {
        public string Locale { get; set; }
        public string Currency { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public List<RoomOccupancy> Occupancies { get; set; }
    }
}
