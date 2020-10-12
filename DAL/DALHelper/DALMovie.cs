using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DAL.DALHelper
{
    public class DALMovie
    {

        public List<Movie> AllMovies()
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {

                    return dbContext.Movies
                                    .Where(s => s.Deleted == false)
                                    .Include(m => m.Director)
                                    .Include(m => m.Director1)
                                    .Include(m => m.Distributor)
                                    .Include(m => m.Actors).Where(a => a.Deleted == false)
                                    .Include(m => m.Genres).Where(a => a.Deleted == false)
                                    .Include(m => m.ShowsMovies).Where(a => a.Deleted == false)
                                    .ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
                return null;
            }
        }

        public Movie GetMovieById(int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Movies.Where(b => b.Id == movieId && b.Deleted == false)
                                            .Include(b => b.Director)
                                            .Include(b => b.Distributor)
                                            .Include(b => b.Actors)
                                            .Include(b => b.Genres)
                                            .Include(b => b.ShowsMovies)
                                            .Include(b => b.Festivals)
                                            .Include(b => b.Reviews.Select(s => s.Critic))
                                            .Include(b => b.ShowsMovies.Select(s => s.Cinema))
                                            .Include(b => b.Festivals.Select(s => s.AwardFestivals
                                                          .Select(t => t.MovieFestivalAwards)))
                                            .FirstOrDefault();
            }
        }

        public List<Movie> GetMoviesByGenre(int genreId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                DALGenre dALGenre = new DALGenre();
                List<Movie> movies = new List<Movie>();

                var genre = dALGenre.GetGenreById(genreId);
                if(genre != null)
                {
                    foreach (var movie in AllMovies())
                    {
                        foreach (var genree in movie.Genres.Where(g => !g.Deleted).ToList())
                        {
                            if (genree.Id == genreId)
                            {
                                movies.Add(movie);
                            }
                        }
                    }
                }
               
                return movies;
            }
        }

        public List<Movie> GetMoviesByDistributor(int distributorId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                DALDistributor dALDistributor = new DALDistributor();
                var distributor = dALDistributor.GetDistributorById(distributorId);
                List<Movie> movies = new List<Movie>();

                if (distributor != null)
                {
                    foreach (var movie in AllMovies())
                    {
                        if (movie.Distributor_Id == distributor.Id)
                        {
                            movies.Add(movie);
                        }                       
                    }
                }
                return movies;
            }
        }

        public void AddMovie(Movie movie)
        {
            try
            {
                if (movie.Id == 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var fest = dbContext.Movies.Where(b => b.Id == movie.Id).FirstOrDefault();
                        if (fest == null)
                        {
                            Movie newMovie = new Movie
                            {
                                Title = movie.Title,
                                Desctiprion = movie.Desctiprion,
                                Duration = movie.Duration,
                                Distributor_Id = movie.Distributor_Id,
                                Director_Id = movie.Director_Id,
                                MetaScore = movie.MetaScore,
                                Image = movie.Image,
                                Deleted = false
                            };

                            dbContext.Movies.Add(newMovie);
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

        public void UpdateMovie(Movie movie)
        {
            try
            {
                if (movie.Id != 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var ex_movie = dbContext.Movies.Where(b => b.Id == movie.Id).FirstOrDefault();
                        if (ex_movie != null)
                        {
                            ex_movie.Title = movie.Title;
                            ex_movie.Duration = movie.Duration;
                            ex_movie.Desctiprion = movie.Desctiprion;
                            ex_movie.Distributor_Id = movie.Distributor_Id;
                            ex_movie.Director_Id = movie.Director_Id;
                            ex_movie.MetaScore = movie.MetaScore;
                            ex_movie.Image = movie.Image;
                            ex_movie.Deleted = movie.Deleted;
                            dbContext.Entry(ex_movie).State = EntityState.Modified;
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

        public bool DeleteMovie(int movieId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var movie = dbContext.Movies.Where(b => b.Id == movieId && b.Deleted == false).FirstOrDefault();
                    if (movie != null)
                    {
                        movie.Deleted = true;
                        dbContext.Entry(movie).State = EntityState.Modified;
                        dbContext.SaveChanges();
                        //kad brisem film, brisem i njegov Id iz tabele Acts
                        List<Actor> acts = new List<Actor>();
                        List<Genre> belongsToGenre = new List<Genre>();
                        List<Festival> movieFestivals = new List<Festival>();
                       
                        foreach (var act in dbContext.Actors)
                        {
                            act.Movies.ToList().ForEach(a =>
                            {
                                if (a.Id == movieId)
                                {
                                    acts.Add(act);
                                }
                            });
                        }
                        acts.ForEach(a =>
                        {
                            a.Movies.Remove(movie);
                        });
                        dbContext.SaveChanges();
                        //brisemo iz zanrova koji su povezani sa njim
                        foreach (var genre in dbContext.Genres)
                        {
                            genre.Movies.ToList().ForEach(a =>
                            {
                                if (a.Id == movieId)
                                {
                                    belongsToGenre.Add(genre);
                                }
                            });
                        }
                        belongsToGenre.ForEach(a =>
                        {
                            a.Movies.Remove(movie);
                        });
                        dbContext.SaveChanges();

                        //brisemo ga ako je ucestvovao na festivalu iz tabele MovieFestival
                        foreach (var festival in dbContext.Festivals)
                        {
                            festival.Movies.ToList().ForEach(a =>
                            {
                                if (a.Id == movieId)
                                {
                                    movieFestivals.Add(festival);
                                }
                            });
                        }
                        movieFestivals.ForEach(a =>
                        {
                            a.Movies.Remove(movie);
                        });
                        dbContext.SaveChanges();

                        //brisemo ga iz tabele MovieFestivalAward ako je dobio neku nagradu
                        var awarded = dbContext.MovieFestivalAwards.Where(s => s.MovieFestival_Movie_Id == movieId && s.Deleted == false).ToList();
                        awarded.ForEach(s =>
                        {
                            s.Deleted = true;
                            dbContext.Entry(s).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        });

                        DALReview dALReview = new DALReview();
                        //brisemo kritike za taj film
                        var reviews = dbContext.Reviews.Where(c => c.Movie_Id == movieId && c.Deleted == false).ToList();
                        reviews.ForEach(c =>
                        {
                            dALReview.DeleteReview(c.Id);
                        });

                        //brisemo i gdje se prikazuje
                        var shows = dbContext.ShowsMovies.Where(s => s.Movie_Id == movieId && s.Deleted == false).ToList();
                        shows.ForEach(s =>
                        {
                            s.Deleted = true;
                            dbContext.Entry(s).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        });

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("Sql Exception:" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
                return false;
            }
        }

        public List<Movie> SearchMovies(string searchText)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                try
                {
                    var movies = dbContext.SearchMovieByTitle(searchText).ToList();
                    if (movies.Count > 0)
                    {
                        List<Movie> findedMovies = new List<Movie>();
                        movies.ForEach(m =>
                        {
                            findedMovies.Add(GetMovieById(m.Id));
                        });
                           
                        return findedMovies;
                    }
                    else
                        return null;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
        }
    }
}
