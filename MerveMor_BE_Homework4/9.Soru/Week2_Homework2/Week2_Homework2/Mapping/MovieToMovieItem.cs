using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Business;
using Week2_Homework2.Data.Entity;

namespace Week2_Homework2.Mapping
{
    public static class MovieToMovieItem
    {
        public static MovieItem Convert(Movie movie )
        {
            MovieItem movieItem = new MovieItem();
            movieItem.MovieId = movie.MovieId;
            movieItem.MovieName = movie.MovieName;
            movieItem.MovieScore = movie.MovieScore;
            movieItem.ReleasedDate = movie.ReleaseDate;
            movieItem.MovieTypeList = new List<MovieType>();
            return movieItem;
        }
    }
}
