using EanHotel.Domain.Request;
using EanHotel.Domain.Response;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Domain
{
    public interface IEanHotelService : IService
    {
        Task<HotelAvailRs> HotelAvailAsync(HotelAvailRq request);
    }
}
