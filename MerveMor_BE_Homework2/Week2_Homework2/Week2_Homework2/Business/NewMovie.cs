using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Data.Context;


namespace Week2_Homework2.Business
{
    public class NewMovie : MovieDetail
    {
        public NewMovie(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        //son üç aylık filmleri döner 
        public List<MovieItem> GetList()
        {
            return MovieItemList.Where(x => x.ReleasedDate > DateTime.Now.AddDays(-90)).OrderBy(o => o.ReleasedDate).ToList();
        }
    }
}

