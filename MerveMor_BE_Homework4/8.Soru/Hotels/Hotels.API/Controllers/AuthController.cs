using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotels.API.Contexts;
using Hotels.API.Models;
using Hotels.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hotels.API.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly HotelApiDbContext _hotelApiDbContext;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, HotelApiDbContext hotelApiDbContext, IConfiguration configuration)
        {
            _userService = userService;
            _hotelApiDbContext = hotelApiDbContext;
            _configuration = configuration;
        }

        [HttpPost(Name = nameof(Authenticate))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] TokenRequest req)
        {
            if (req == null)
                return BadRequest();

            var result = await _userService.Authenticate(req);
            if (result == null)
                return Unauthorized();

            return Ok(result);
        }


        [HttpGet("[action]")]
        public async Task<Token> RefreshTokenLogin([FromForm] string refreshToken)
        {
            UserInfo user = await _hotelApiDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.Now)
            {
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _hotelApiDbContext.SaveChangesAsync();

                return token;
            }
            return null;
        }


    }
}