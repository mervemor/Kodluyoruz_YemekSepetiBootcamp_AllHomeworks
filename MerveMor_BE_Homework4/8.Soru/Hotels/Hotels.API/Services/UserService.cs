using AutoMapper;
using Hotels.API.Contexts;
using Hotels.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.API.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly HotelApiDbContext _dbContext;
        private IConfiguration _configuration;
        public UserService(HotelApiDbContext dbContext,
                           IMapper mapper,
                           IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        //public async Task<UserInfo> Authenticate(TokenRequest req)
        //{
        //    if(string.IsNullOrWhiteSpace(req.LoginUser) ||
        //        string.IsNullOrWhiteSpace(req.LoginPassword))
        //    {
        //        return null;
        //    }

        //    var user = await _dbContext
        //                      .Users
        //                      .SingleOrDefaultAsync(user => user.LoginName == req.LoginUser &&
        //                                                    user.Password == req.LoginPassword);

        //    if (user == null)
        //        return null;

        //    var secretKey = _configuration.GetValue<string>("JwtTokenKey");
        //    var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); //şifreleme algoritmalarından biri
        //    var tokenDesc = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Id.ToString())
        //        }),
        //        NotBefore = DateTime.Now, //ToUTC tutmak daha mantıklı olabilir belki 
        //        Expires = DateTime.Now.AddHours(1), //1 saat geçerli olsun
        //        SigningCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var newToken = tokenHandler.CreateToken(tokenDesc);
        //    var userInfo = _mapper.Map<UserInfo>(user);
        //    userInfo.ExpireTime = tokenDesc.Expires ?? DateTime.Now.AddHours(1); //valid olduğu tarih
        //    userInfo.Token = tokenHandler.WriteToken(newToken);

        //    return userInfo;
        //}

        public Token CreateAccessToken(UserInfo userInfo)
        {
            Token tokenInstance = new Token();

            //Security  Key'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Oluşturulacak token ayarlarını veriyoruz.
            tokenInstance.Expiration = DateTime.Now.AddMinutes(5);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: tokenInstance.Expiration,//Token süresini 5 dk olarak belirliyorum
                notBefore: DateTime.Now,//Token üretildikten ne kadar süre sonra devreye girsin ayarlıyouz.
                signingCredentials: signingCredentials
                );

            //Token oluşturucu sınıfında bir örnek alıyoruz.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Token üretiyoruz.
            tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);

            //Refresh Token üretiyoruz.
            tokenInstance.RefreshToken = CreateRefreshToken();
            return tokenInstance;
        }

        //Refresh Token üretecek metot.
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }


    }
}
