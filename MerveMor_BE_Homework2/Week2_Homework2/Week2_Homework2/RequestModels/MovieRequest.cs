using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Week2_Homework2.Enums;
using Week2_Homework2.Validation;

namespace Week2_Homework2.RequestModels
{
    public class MovieRequest
    {
        [StringLength(15, ErrorMessage = "Movie ismi maksimum 15 karakter olabilir.")]
        public string MovieName { get; set; }
        
        [MovieTypeValidation(ErrorMessage = "Lütfen geçerli bir movie type giriniz.")]
        public int MovieTypeId { get; set; }
        
        [Range(1, 10, ErrorMessage = "Movie puanı belirtilen aralıkta olmalıdır.")]
        public decimal MovieScore { get; set; }

        public int UserId { get; set; }
    }
}
