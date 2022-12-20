using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        ILocationService _locationService;
        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService; ;
        }

        [HttpGet("il")]
        public async Task<ActionResult> GetAll()
        {
            var allIller = await _locationService.GetIllerAsync();

            return Ok(allIller);
        }

        [HttpGet("ilce/{ilAdi}")]
        public async Task<ActionResult> GetIlceByIlId(string ilAdi)
        {
            var ilceler = await _locationService.GetIlceByIlIdAsync(ilAdi);

            return Ok(ilceler);
        }

        [HttpGet("mahalle/{ilceAdi}")]
        public async Task<ActionResult> GetMahalleByIlceId(string ilceAdi)
        {
            var mahalleler = await _locationService.GetMahalleByIlceIdAsync(ilceAdi);

            return Ok(mahalleler);
        }
    }
}