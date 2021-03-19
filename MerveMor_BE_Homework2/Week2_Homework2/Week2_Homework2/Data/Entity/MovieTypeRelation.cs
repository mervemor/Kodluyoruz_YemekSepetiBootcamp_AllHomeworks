using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week2_Homework2.Data.Entity
{
    public class MovieTypeRelation
    {
        public int MovieTypeRelationId { get; set; }
        public int MovieId { get; set; }
        public int MovieTypeId { get; set; }
    }
}
