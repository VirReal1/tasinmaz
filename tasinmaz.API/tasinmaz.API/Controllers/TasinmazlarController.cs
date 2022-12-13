using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasinmaz.API.Data;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TasinmazDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll()
        {
            var allTasinmazlar = await _tasinmazService.GetAllAsync();

            if (allTasinmazlar.Warning)
            {
                ModelState.AddModelError("", allTasinmazlar.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(allTasinmazlar);
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TasinmazDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBySearch([FromBody] TasinmazDto tasinmazDto = null)
        {
            var searchTasinmaz = await _tasinmazService.GetTasinmazlarAsync(tasinmazDto);

            if (searchTasinmaz.Warning)
            {
                ModelState.AddModelError("", searchTasinmaz.Message);
                return StatusCode(404, ModelState);
            }

            if (searchTasinmaz.Error)
            {
                ModelState.AddModelError("", searchTasinmaz.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(searchTasinmaz);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TasinmazDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Add([FromBody] TasinmazDto tasinmazDto)
        {
            var createTasinmaz = await _tasinmazService.AddTasinmazAsync(tasinmazDto);

            if (createTasinmaz.Warning)
            {
                ModelState.AddModelError("", createTasinmaz.Message);
                return StatusCode(403, ModelState);
            }

            if (createTasinmaz.Error)
            {
                ModelState.AddModelError("", createTasinmaz.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(createTasinmaz);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] TasinmazDto tasinmazDto)
        {
            var updateTasinmaz = await _tasinmazService.UpdateTasinmazAsync(tasinmazDto);

            if (updateTasinmaz.Warning)
            {
                ModelState.AddModelError("", updateTasinmaz.Message);
                return StatusCode(404, ModelState);
            }

            if (updateTasinmaz.Error)
            {
                ModelState.AddModelError("", updateTasinmaz.Message);
                return StatusCode(500, ModelState);
            }

            return Ok(updateTasinmaz);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromBody] TasinmazDto tasinmazDto)
        {
            var removeTasinmaz = await _tasinmazService.DeleteTasinmazAsync(tasinmazDto);

            if (removeTasinmaz.Warning)
            {
                ModelState.AddModelError("", removeTasinmaz.Message);
                return StatusCode(404, ModelState);
            }

            if (removeTasinmaz.Error)
            {
                ModelState.AddModelError("", removeTasinmaz.Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
