using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week2_Homework2.Data.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
