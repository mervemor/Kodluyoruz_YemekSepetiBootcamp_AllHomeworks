using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Data.Context;
using Week2_Homework2.Data.Entity;
using Week2_Homework2.Mapping;

namespace Week2_Homework2.Business
{
    public class MovieDetail
    {
        public List<MovieItem> MovieItemList { get; set; }
        
        public MovieDetail(DatabaseContext databaseContext)
        {
            MovieItemList = new List<MovieItem>();
            var movieList = databaseContext.Movie.ToList();
            foreach (var movie in movieList)
            {
                MovieItem movieItem = MovieToMovieItem.Convert(movie);
                var movieTypeRelationList = databaseContext.MovieTypeRelation.Where(x => x.MovieId == movie.MovieId).ToList();
                foreach (var movieTypeRelationItem in movieTypeRelationList)
                {
                    var movieType = databaseContext.MovieType.FirstOrDefault(x => x.MovieTypeId == movieTypeRelationItem.MovieTypeId);
                    movieItem.MovieTypeList.Add(movieType);
                }
                MovieItemList.Add(movieItem);
            }
        }

    }

    public class MovieItem 
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public decimal MovieScore { get; set; }
        public List<MovieType> MovieTypeList { get; set; }
        public DateTime ReleasedDate { get; set; }
    }
}
