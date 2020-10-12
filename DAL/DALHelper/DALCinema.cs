using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALCinema
    {

        public List<Cinema> AllCinemas()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Cinemas.Where(s => s.Deleted == false)
                    .Include(c => c.ShowsMovies)
                    .Include(c => c.Place).ToList();
            }
        }

        public List<ShowsMovie> AllShowsMovies()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.ShowsMovies.Where(s => s.Deleted == false)
                                            .Include(c => c.Movie)
                                            .Include(c => c.Cinema)
                                            .ToList();
            }
        }

        public Cinema GetCinemaById(int cinemaId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Cinemas.Where(b => b.Id == cinemaId && b.Deleted == false).FirstOrDefault();
            }
        }

        public List<ShowsMovie> GetMoviesInCinema(int cinemaId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.ShowsMovies.Where(b => b.Cinema_Id == cinemaId && b.Deleted == false)
                                                    .Include(m => m.Movie)
                                                    .ToList();
            }
        }

        public void AddCinema(Cinema cinema)
        {
            if (cinema.Id == 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    Cinema newCinema = new Cinema()
                    {
                        Name = cinema.Name,
                        Place_Id = cinema.Place_Id,
                        Deleted = false
                    };

                    dbContext.Cinemas.Add(newCinema);
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateCinema(Cinema cinema)
        {       
            if (cinema.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Cinemas.Where(b => b.Id == cinema.Id).FirstOrDefault();
                    if (fest != null)
                    {
                        fest.Name = cinema.Name;
                        fest.Place_Id = cinema.Place_Id;
                        fest.Deleted = cinema.Deleted;
                        dbContext.Entry(fest).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteCinema(int cinemaId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var cinema = dbContext.Cinemas.Where(b => b.Id == cinemaId && b.Deleted == false).FirstOrDefault();
                    if (cinema != null)
                    {
                        cinema.Deleted = true;
                        dbContext.Entry(cinema).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        //brisemo i gdje se prikazuje
                        var shows = dbContext.ShowsMovies.Where(s => s.Cinema_Id == cinemaId && s.Deleted == false).ToList();
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
                return false;
            }
        }

        public void AddShowsMovie(ShowsMovie showsMovie)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                var exists = dbContext.ShowsMovies.Where(s => s.Deleted == false
                                                                    && s.Cinema_Id == showsMovie.Cinema_Id
                                                                    && s.Movie_Id == showsMovie.Movie_Id).FirstOrDefault();
                if (exists == null)
                {
                    ShowsMovie newShows = new ShowsMovie
                    {
                        FromDate = showsMovie.FromDate,
                        ToDate = showsMovie.ToDate,
                        Cinema_Id = showsMovie.Cinema_Id,
                        Movie_Id = showsMovie.Movie_Id,
                        Deleted = false
                    };
                    dbContext.ShowsMovies.Add(newShows);
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateShowsMovie(ShowsMovie showsMovie)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                var exists = dbContext.ShowsMovies.Where(s => s.Deleted == false
                                                                    && s.Cinema_Id == showsMovie.Cinema_Id
                                                                    && s.Movie_Id == showsMovie.Movie_Id).FirstOrDefault();
                if (exists != null)
                {
                    exists.Movie_Id = showsMovie.Movie_Id;
                    exists.Cinema_Id = showsMovie.Cinema_Id;
                    exists.ToDate = showsMovie.ToDate;
                    exists.FromDate = showsMovie.FromDate;
                    dbContext.Entry(exists).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
