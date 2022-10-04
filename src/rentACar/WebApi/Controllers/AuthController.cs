using Application.Features.Auths.Commands.Register;
using Application.Features.Auths.Dtos;
using Core.Security.Dtos;
using Core.Security.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Migrations;
using System.Reflection.Metadata.Ecma335;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserForRegisterDto register)
        {
            RegisterCommand registerCommand = new() { UserForRegisterDto = register, IpAddress = GetIpAddress() };

            RegisteredDto result =await Mediator.Send(registerCommand);
            SetRefresTokenToCookie(result.RefreshToken);
            return Created("",result.AccessToken);
        }

        private void SetRefresTokenToCookie(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7)};
            Response.Cookies.Append("refreshToken",refreshToken.Token,cookieOptions);
        }
    }
}
