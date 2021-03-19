using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.API.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime ExpireTime { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; } //kullanıcı için üretilmiş olan refresh token değerini tutacak
        public DateTime? RefreshTokenEndDate { get; set; } //üretilen refresh token değerinin kullanım süresini belirleyecek olan zaman bilgisini tutar

    }
}
