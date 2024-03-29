﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Helpers;
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

        [HttpGet("all/{logKullaniciId}")]
        //[Authorize(Policy = Policies.Admin)]
        //[Authorize(Policy = Policies.User)]
        public async Task<ActionResult> GetAllById(int logKullaniciId)
        {
            var allTasinmazlar = await _tasinmazService.GetAllAsync(logKullaniciId);

            return Ok(allTasinmazlar);
        }

        [HttpPost("search")]
        //[Authorize(Policy = Policies.Admin)]
        //[Authorize(Policy = Policies.User)]
        public async Task<ActionResult> GetBySearch([FromBody] TasinmazDto tasinmazDto)
        {
            var searchTasinmaz = await _tasinmazService.GetTasinmazlarAsync(tasinmazDto);

            return Ok(searchTasinmaz);
        }

        [HttpPost]
        //[Authorize(Policy = Policies.Admin)]
        //[Authorize(Policy = Policies.User)]
        public async Task<ActionResult> Add([FromBody] TasinmazDto tasinmazDto)
        {
            var createTasinmaz = await _tasinmazService.AddTasinmazAsync(tasinmazDto);

            return Ok(createTasinmaz);
        }

        [HttpPut]
        //[Authorize(Policy = Policies.Admin)]
        //[Authorize(Policy = Policies.User)]
        public async Task<ActionResult> Update([FromBody] TasinmazDto tasinmazDto)
        {
            var updateTasinmaz = await _tasinmazService.UpdateTasinmazAsync(tasinmazDto);

            return Ok(updateTasinmaz);
        }

        [HttpDelete("{logKullaniciId}/{tasinmazId}")]
        //[Authorize(Policy = Policies.Admin)]
        //[Authorize(Policy = Policies.User)]
        public async Task<ActionResult> Delete(int logKullaniciId, int tasinmazId)
        {
            var removeTasinmaz = await _tasinmazService.DeleteTasinmazAsync(logKullaniciId, tasinmazId);

            return Ok(removeTasinmaz);
        }
    }
}
