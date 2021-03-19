using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperHomework.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public decimal Availability { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
