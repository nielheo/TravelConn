using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Domain
{
    public class RoomOccupancy
    {
        public int AdultCount { get; set; }
        public List<int> ChildAges { get; set; }
    }
}
