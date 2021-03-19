
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Week2_Homework2.RequestModels
{
    public class MovieTypeRequest
    {
        [Required(ErrorMessage = "Lütfen movie type giriniz.")]
        public string Name { get; set; }

    }
}
