using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Models;
using tasinmaz.API.Services.Abstract;
using tasinmaz.API.Services.Concrete;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;;
        }

        [HttpGet("il")]
        public async Task<ActionResult> GetAll()
        {
            var allIller = await _locationService.GetIllerAsync();

            return Ok(allIller);
        }

        [HttpGet("ilce")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ilce>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetIlceByIlId(int ilId, int kullaniciId)
        {
            var ilceler = await _locationService.GetIlceByIlIdAsync(ilId, kullaniciId);

            return Ok(ilceler);
        }

        [HttpGet("mahalle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Mahalle>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetMahalleByIlceId(int ilceId, int kullaniciId)
        {
            var mahalleler = await _locationService.GetMahalleByIlceIdAsync(ilceId, kullaniciId);

            return Ok(mahalleler);
        }
    }
}
