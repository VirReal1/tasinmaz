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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Il>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll()
        {
            var allIller = await _locationService.GetIllerAsync();

            if (allIller.Error)
            {
                ModelState.AddModelError("", allIller.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(allIller);
        }

        [HttpGet("ilce")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ilce>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetIlceByIlId(int ilId, int kullaniciId)
        {
            var ilceler = await _locationService.GetIlceByIlIdAsync(ilId, kullaniciId);

            if (ilceler.Warning)
            {
                ModelState.AddModelError("", ilceler.Message);
                return StatusCode(404, ModelState);
            }

            if (ilceler.Error)
            {
                ModelState.AddModelError("", ilceler.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(ilceler);
        }

        [HttpGet("mahalle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Mahalle>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetMahalleByIlceId(int ilceId, int kullaniciId)
        {
            var mahalleler = await _locationService.GetMahalleByIlceIdAsync(ilceId, kullaniciId);

            if (mahalleler.Warning)
            {
                ModelState.AddModelError("", mahalleler.Message);
                return StatusCode(404, ModelState);
            }

            if (mahalleler.Error)
            {
                ModelState.AddModelError("", mahalleler.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(mahalleler);
        }
    }
}
