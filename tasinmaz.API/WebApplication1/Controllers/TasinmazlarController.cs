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
        public async Task<ActionResult> GetAll()
        {
            var tasinmazlar = await _tasinmazService.GetAllAsync();
            return Ok(tasinmazlar);
        }

        [HttpPost("search")]
        public async Task<ActionResult> GetBySearch([FromBody] TasinmazDto tasinmazDto = null)
        {
            var tasinmazlar = await _tasinmazService.GetTasinmazlarAsync(tasinmazDto);
            return Ok(tasinmazlar); //!!!!!!!!!!!!!!!!!!!!
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Add([FromBody] TasinmazDto tasinmazDto)
        {
            if (tasinmazDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _createTasinmaz = await _tasinmazService.AddTasinmazAsync(tasinmazDto);

            if (_createTasinmaz.Success == false && _createTasinmaz.Message == "Taşınmaz exists.")
            {
                return Ok(_createTasinmaz);
            }

            if (_createTasinmaz.Success == false && _createTasinmaz.Message == "Repository error.")
            {
                ModelState.AddModelError("", "Something went wrong in respository layer when adding taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            if (_createTasinmaz.Success == false && _createTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", "Something went wrong in service layer when adding taşınmaz { tasinmazDto}");
                return StatusCode(500, ModelState);
            }

            return Ok(_createTasinmaz);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromBody] TasinmazDto tasinmazDto)
        {
            if (tasinmazDto == null)
            {
                return BadRequest(ModelState);
            }

            var _updateTasinmaz = await _tasinmazService.UpdateTasinmazAsync(tasinmazDto);

            if (_updateTasinmaz.Success == false && _updateTasinmaz.Message == "Not found.")
            {
                return Ok(_updateTasinmaz);
            }

            if (_updateTasinmaz.Success == false && _updateTasinmaz.Message == "Repository error.")
            {
                ModelState.AddModelError("", "Something went wrong in respository layer when adding taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            if (_updateTasinmaz.Success == false && _updateTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", "Something went wrong in service layer when adding taşınmaz { tasinmazDto}");
                return StatusCode(500, ModelState);
            }
            return Ok(_updateTasinmaz);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromBody] TasinmazDto tasinmazDto)
        {
            if (tasinmazDto == null)
            {
                return BadRequest(ModelState);
            }

            var _removeTasinmaz = await _tasinmazService.UpdateTasinmazAsync(tasinmazDto);

            if (_removeTasinmaz.Success == false && _removeTasinmaz.Message == "Not found.")
            {
                ModelState.AddModelError("", _removeTasinmaz.Message);
                return StatusCode(404, ModelState);

            }

            if (_removeTasinmaz.Success == false && _removeTasinmaz.Message == "Repository error.")
            {
                ModelState.AddModelError("", "Something went wrong in respository layer when adding taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            if (_removeTasinmaz.Success == false && _removeTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", "Something went wrong in service layer when adding taşınmaz { tasinmazDto}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
