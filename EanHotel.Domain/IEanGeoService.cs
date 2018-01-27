using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Domain
{
    public interface IEanGeoService : IService
    {
        Task<string> GetDescendats(Int64 id);
    }
}
