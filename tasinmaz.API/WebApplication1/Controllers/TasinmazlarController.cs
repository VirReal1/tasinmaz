using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Models;
using tasinmaz.API.Services.Abstract;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasinmazlarController : ControllerBase
    {
        ITasinmazService _tasinmazService;
        public TasinmazlarController(ITasinmazService tasinmazService)
        {
            _tasinmazService = tasinmazService;
        }
        [HttpGet]
        public async Task<ActionResult> GetTasinmazlar(TasinmazDto tasinmazDto)
        {
            var tasinmazlar = await _tasinmazService.GetTasinmazlarAsync(tasinmazDto);

            return Ok(tasinmazlar);
        }

        [HttpPost]
        public ActionResult Add([FromBody] TasinmazDto tasinmazDto)
        {
            return Ok(tasinmazDto);
        }
    }
}
