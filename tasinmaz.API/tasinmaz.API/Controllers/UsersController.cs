using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Helpers;
using tasinmaz.API.Models;
using tasinmaz.API.Services.Abstract;
using tasinmaz.API.Services.Concrete;

namespace tasinmaz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all/{logKullaniciId}")]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> GetAll(int logKullaniciId)
        {
            var allUsers = await _userService.GetAllAsync(logKullaniciId);

            return Ok(allUsers);
        }

        [HttpPost("search")]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> GetBySearch([FromBody] KullaniciForShowDto? kullaniciForShowDto = null)
        {
            var searchUsers = await _userService.GetUsersAsync(kullaniciForShowDto);

            return Ok(searchUsers);
        }
        
        [HttpPost("login")]
        //[AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] KullaniciForLoginDto kullaniciForLoginDto)
        {
            var kullaniciToken = await _userService.LoginUserAsync(kullaniciForLoginDto);

            return Ok(kullaniciToken);
        }

        [HttpPost]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> Add([FromBody] KullaniciForAddDto kullaniciForAddDto)
        {
            var createUser = await _userService.AddUserAsync(kullaniciForAddDto);

            return Ok(createUser);
        }

        [HttpPut]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> Update([FromBody] KullaniciForUpdateDto kullaniciForUpdateDto)
        {
            var updateUser = await _userService.UpdateUserAsync(kullaniciForUpdateDto);

            return Ok(updateUser);
        }

        [HttpDelete("{logKullaniciId}/{kullaniciId}")]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> Delete(int logKullaniciId, int kullaniciId)
        {
            var removeUser = await _userService.DeleteUserAsync(logKullaniciId, kullaniciId);

            return Ok(removeUser);
        }
    }
}
