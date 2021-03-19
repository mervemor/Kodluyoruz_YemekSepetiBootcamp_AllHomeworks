using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotels.API.Models;
using Hotels.API.Models.Derived;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace Hotels.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly HotelInfo _hotelInfo;
        public InfoController(IOptions<HotelInfo> hotelInfoOption)
        {
            _hotelInfo = hotelInfoOption.Value;
        }

        [HttpGet(Name = nameof(GetInfo))]
        [ProducesResponseType(200)]
        public ActionResult<HotelInfo> GetInfo()
        {
            _hotelInfo.Href = Url.Link(nameof(GetInfo), null);
            return _hotelInfo;
        }
    }
}