using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Enums;

namespace Week2_Homework2.Data.Entity
{// base film sınıfı 
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public decimal MovieScore { get; set; }
        public MovieStatus MovieStatus { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int UserId { get; set; }
    }
}
