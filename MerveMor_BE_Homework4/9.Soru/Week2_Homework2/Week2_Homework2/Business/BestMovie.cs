using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Data.Context;


namespace Week2_Homework2.Business
{
    public class BestMovie : MovieDetail
    {
        public BestMovie(DatabaseContext databaseContext) :base(databaseContext)
        {
            
        }

        //score 9 dan yüksek olanları alır 
        public List<MovieItem> GetList()
        {
            return MovieItemList.Where(x => x.MovieScore > 9).OrderBy(o => o.MovieScore).ToList();
        }
    }
}
