using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALFestival
    {

        public List<Festival> AllFestivals()
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    return dbContext.Festivals.Where(s => s.Deleted == false).Include(c => c.Place).ToList();
                }
            }
             catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
                return null;
            }
        }

        public List<Festival> GetFestivalsForMovie(int movieId)
        {
            List<Festival> festivals = new List<Festival>();
            DALMovie dALMovie = new DALMovie();
            var movie = dALMovie.GetMovieById(movieId);

            movie.Festivals.Where(a => !a.Deleted).ToList().ForEach(a => {
                festivals.Add(GetFestivalById(a.Id));
            });
         
            return festivals;
        }

        public Festival GetFestivalById(int festivalId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Festivals.Where(b => b.Id == festivalId && b.Deleted == false)
                                                .Include(b => b.Place)
                                                .FirstOrDefault();
            }
        }

        public void AddFestival(Festival festival)
        {
            if (festival.Id == 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Festivals.Where(b => b.Id == festival.Id && b.Deleted == false).FirstOrDefault();
                    if (fest == null)
                    {
                        Festival newfestival = new Festival
                        {
                            Name = festival.Name,
                            Date = festival.Date,
                            Place_Id = festival.Place_Id,
                            Deleted = false
                        };

                        dbContext.Festivals.Add(newfestival);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public void UpdateFestival(Festival festival)
        {
            if (festival.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Festivals.Where(b => b.Id == festival.Id).FirstOrDefault();
                    if (fest != null)
                    {
                        fest.Place_Id = festival.Place_Id;
                        fest.Name = festival.Name;
                        fest.Date = festival.Date;
                        fest.Deleted = festival.Deleted;
                        dbContext.Entry(fest).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteFestival(int festivalId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var festival = dbContext.Festivals.Where(b => b.Id == festivalId && b.Deleted == false).FirstOrDefault();
                    if (festival != null)
                    {
                        festival.Deleted = true;
                        dbContext.Entry(festival).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        //brisemo iz veze sa nagradom
                        var festivalAward = dbContext.AwardFestivals.Where(s => s.Festival_Id == festivalId && s.Deleted == false).ToList();
                        festivalAward.ForEach(s =>
                        {
                            s.Deleted = true;
                            dbContext.Entry(s).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        });

                        //brisemo iz veze sa nagradom i filmom
                        var movieAwarded = dbContext.MovieFestivalAwards
                                             .Where(s => s.AwardFestival_Festival_Id == festivalId && s.Deleted == false).ToList();
                        movieAwarded.ForEach(s =>
                        {
                            s.Deleted = true;
                            dbContext.Entry(s).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        });

                        //brisemo festival iz filmova na kojima su ucestvovali
                        List<Movie> movies = new List<Movie>();
                        foreach (var movie in dbContext.Movies)
                        {
                            movie.Festivals.ToList().ForEach(m =>
                            {
                                if (m.Id == festivalId)
                                {
                                    movies.Add(movie);
                                }
                            });
                        }
                        movies.ForEach(a =>
                        {
                            a.Festivals.Remove(festival);
                        });
                        dbContext.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
                return false;
            }
        }

        public void AddMovieFestivalProcedure(int festivalId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                dbContext.AddMovieFestival(festivalId, movieId);
            }
        }
    }
}
