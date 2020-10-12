using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALGenre
    {
        private DALMovie _dALMovie = new DALMovie();

        public List<Genre> AllGenres()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Genres.Where(s => s.Deleted == false).ToList();
            }
        }

        public Genre GetGenreById(int genreId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Genres.Where(b => b.Id == genreId && b.Deleted == false).FirstOrDefault();
            }
        }

        public void DeleteGenreForMovie(int genreId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7(true))
            {
                var movie = dbContext.Movies.Where(m => m.Id == movieId).FirstOrDefault();
                var genre = dbContext.Genres.Where(a => a.Id == genreId).FirstOrDefault();
                movie.Genres.Remove(genre);
                dbContext.SaveChanges();
            }
        }

        public void AddGenre(Genre genre)
        {
            try
            {
                if (genre.Id == 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var fest = dbContext.Genres.Where(b => b.Id == genre.Id).FirstOrDefault();
                        if (fest == null)
                        {
                            Genre newZanr = new Genre
                            {
                                Name = genre.Name,
                                Deleted = false
                            };
                            dbContext.Genres.Add(newZanr);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("Sql Exception:" + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
            }
        }

        public void UpdateGenre(Genre genre)
        {
            try
            {
                if (genre.Id != 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var fest = dbContext.Genres.Where(b => b.Id == genre.Id).FirstOrDefault();
                        if (fest != null)
                        {
                            fest.Deleted = genre.Deleted;
                            fest.Name = genre.Name;
                            dbContext.Entry(fest).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("Sql Exception:" + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
            }
        }

        public bool DeleteGenre(int genreId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var genre = dbContext.Genres.Where(b => b.Id == genreId && b.Deleted == false).FirstOrDefault();
                    if (genre != null)
                    {
                        genre.Deleted = true;
                        dbContext.Entry(genre).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        List<Movie> movies = new List<Movie>();
                        foreach (var movie in _dALMovie.AllMovies())
                        {
                            movie.Genres.ToList().ForEach(m =>
                            {
                                if (m.Id == genreId)
                                {
                                    movies.Add(movie);
                                }
                            });
                        }
                        movies.ForEach(a =>
                        {
                            a.Genres.Remove(genre);
                        });
                      //  dbContext.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
           
        }

        public void AddBelongToGenreProcedure(int genreId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                dbContext.AddBelongsToGenre(genreId, movieId);
            }
        }
    }
}
