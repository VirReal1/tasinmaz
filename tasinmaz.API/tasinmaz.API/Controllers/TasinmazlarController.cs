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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll() //Will Be Deleted
        {
            var allTasinmazlar = await _tasinmazService.GetAllAsync();

            if (allTasinmazlar.Success == false && allTasinmazlar.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when getting taşınmazlar.");
                return StatusCode(500, ModelState);
            }

            return Ok(allTasinmazlar);
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TasinmazDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBySearch([FromBody] TasinmazDto tasinmazDto = null)
        {
            if (tasinmazDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var searchTasinmaz = await _tasinmazService.GetTasinmazlarAsync(tasinmazDto);

            if (searchTasinmaz.Success == false && searchTasinmaz.Message == "No Parameter.")
            {
                ModelState.AddModelError("", searchTasinmaz.Message);
                return StatusCode(404, ModelState);
            }

            if (searchTasinmaz.Success == false && searchTasinmaz.Message == "Parameters does not match with the database.")
            {
                ModelState.AddModelError("", searchTasinmaz.Message);
                return StatusCode(404, ModelState);
            }

            if (searchTasinmaz.Success == false && searchTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when searching taşınmazlar.");
                return StatusCode(500, ModelState);
            }

            return Ok(searchTasinmaz);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TasinmazDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            var createTasinmaz = await _tasinmazService.AddTasinmazAsync(tasinmazDto);

            if (createTasinmaz.Success == false && createTasinmaz.Message == "Taşınmaz exists.")
            {
                return Ok(createTasinmaz);
            }

            if (createTasinmaz.Success == false && createTasinmaz.Message == "Repository error.")
            {
                ModelState.AddModelError("", $"Something went wrong in repository layer when adding taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            if (createTasinmaz.Success == false && createTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when adding taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            return Ok(createTasinmaz);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] TasinmazDto tasinmazDto)
        {
            if (tasinmazDto == null)
            {
                return BadRequest(ModelState);
            }

            var updateTasinmaz = await _tasinmazService.UpdateTasinmazAsync(tasinmazDto);

            if (updateTasinmaz.Success == false && updateTasinmaz.Message == "Not found.")
            {
                return Ok(updateTasinmaz);
            }

            if (updateTasinmaz.Success == false && updateTasinmaz.Message == "Repository error.")
            {
                ModelState.AddModelError("", $"Something went wrong in repository layer when updating taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            if (updateTasinmaz.Success == false && updateTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when updating taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }
            return Ok(updateTasinmaz);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromBody] TasinmazDto tasinmazDto)
        {
            if (tasinmazDto == null)
            {
                return BadRequest(ModelState);
            }

            var removeTasinmaz = await _tasinmazService.DeleteTasinmazAsync(tasinmazDto);

            if (removeTasinmaz.Success == false && removeTasinmaz.Message == "Not found.")
            {
                ModelState.AddModelError("", removeTasinmaz.Message);
                return StatusCode(404, ModelState);

            }

            if (removeTasinmaz.Success == false && removeTasinmaz.Message == "Repository error.")
            {
                ModelState.AddModelError("", $"Something went wrong in repository layer when deleting taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }

            if (removeTasinmaz.Success == false && removeTasinmaz.Message == "Error occured.")
            {
                ModelState.AddModelError("", $"Something went wrong in service layer when deleting taşınmaz {tasinmazDto}.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
