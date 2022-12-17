using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tasinmaz.API.Data;
using tasinmaz.API.Dtos;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Models;
using tasinmaz.API.Models.Concrete;
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
        public async Task<ActionResult> GetAll(int logKullaniciId)
        {
            var allUsers = await _userService.GetAllAsync(logKullaniciId);

            return Ok(allUsers);
        }

        [HttpPost("search")]
        public async Task<ActionResult> GetBySearch([FromBody] KullaniciForShowDto kullaniciForShowDeleteDto)
        {
            var searchUsers = await _userService.GetUsersAsync(kullaniciForShowDeleteDto);

            return Ok(searchUsers);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] KullaniciForLoginDto kullaniciForLoginDto)
        {
            var kullaniciToken = await _userService.LoginUserAsync(kullaniciForLoginDto);

            return Ok(kullaniciToken);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] KullaniciForUpdateDto kullaniciForAddUpdateDto)
        {
            var createUser = await _userService.AddUserAsync(kullaniciForAddUpdateDto);

            return Ok(createUser);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] KullaniciForUpdateDto kullaniciForAddUpdateDto)
        {
            var updateUser = await _userService.UpdateUserAsync(kullaniciForAddUpdateDto);

            return Ok(updateUser);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] KullaniciForShowDto kullaniciForShowDeleteDto)
        {
            var removeUser = await _userService.DeleteUserAsync(kullaniciForShowDeleteDto);

            return Ok(removeUser);
        }
    }
}
