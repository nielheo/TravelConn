using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Hotels")]
    public class HotelsController : Controller
    {
        [Route("/{country}/{city}/hotels")]
        public async Task<IActionResult> GetHotel()
        {
            return View();
        }
    }
}