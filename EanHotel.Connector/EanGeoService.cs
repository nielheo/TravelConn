using EanHotel.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Connector
{
    public class EanGeoService : BaseService, IEanGeoService
    {
        public Task<string> GetDescendats(long id)
        {
            throw new NotImplementedException();
        }
    }
}
