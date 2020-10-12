using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALAward
    {

        public Award GetAwardById(int awardId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Awards.Where(b => b.Id == awardId && b.Deleted == false).FirstOrDefault();
            }
        }

        public List<AwardFestival> GetAwardForFestival(int festivalId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.AwardFestivals.Where(b => b.Festival_Id == festivalId && b.Deleted == false)
                                                      .Include(b => b.Award).ToList();
            }
        }

        public List<MovieFestivalAward> GetAwardsForMovie(int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.MovieFestivalAwards.Where(b => b.MovieFestival_Movie_Id == movieId
                                                                && b.Deleted == false)
                                                                .Include(b => b.AwardFestival.Award)
                                                                .Include(b => b.AwardFestival.Festival).ToList();
            }
        }

        public List<AwardFestival> AllAwardFestival()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.AwardFestivals
                                    .Where(s => s.Deleted == false)
                                    .Include(c => c.Festival)
                                    .Include(c => c.Award).ToList();
            }
        }

        public List<AwardFestival> AllAwardsForFestival(int festivalId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.AwardFestivals.Where(s => s.Deleted == false && s.Festival_Id == festivalId)
                                                     .Include(f => f.MovieFestivalAwards).ToList();
            }
        }

        public List<Award> AllAwards()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Awards.Where(s => s.Deleted == false)
                                            .Include(s => s.AwardFestivals).ToList();
            }
        }

        public void AddAward(Award award)
        {
            if (award.Id == 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Awards.Where(b => b.Id == award.Id).FirstOrDefault();
                    if (fest == null)
                    {
                        Award newAward = new Award
                        {
                            Name = award.Name,
                            Deleted = false
                        };
                        dbContext.Awards.Add(newAward);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public void UpdateAward(Award award)
        {
            if (award.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Awards.Where(b => b.Id == award.Id).FirstOrDefault();
                    if (fest != null)
                    {

                        fest.Name = award.Name;
                        dbContext.Entry(fest).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteAward(int awardId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var award = dbContext.Awards.Where(b => b.Id == awardId && b.Deleted == false).FirstOrDefault();
                    if (award != null)
                    {
                        award.Deleted = true;
                        dbContext.Entry(award).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        //brisemo nagradu iz dodjele na festivalima
                        var festivalAward = dbContext.AwardFestivals.Where(s => s.Award_Id == awardId && s.Deleted == false).ToList();
                        festivalAward.ForEach(s =>
                        {
                            s.Deleted = true;
                            dbContext.Entry(s).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        });

                        //brisemo i ako je nekom filmu dodijeljena
                        var movieAwarded = dbContext.MovieFestivalAwards
                                                     .Where(s => s.AwardFestival_Award_Id == awardId && s.Deleted == false).ToList();
                        movieAwarded.ForEach(s =>
                        {
                            s.Deleted = true;
                            dbContext.Entry(s).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        });
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

        public void AddAwardFestival(AwardFestival awardFestival)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                AwardFestival newAwardFestival = new AwardFestival
                {
                    Award_Id = awardFestival.Award_Id,
                    Festival_Id = awardFestival.Festival_Id,
                    Deleted = false
                };
                dbContext.AwardFestivals.Add(newAwardFestival);
                dbContext.SaveChanges();
            }
        }

        public void AddMovieFestivalAward(MovieFestivalAward movieFestivalAward)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                MovieFestivalAward newMovieFestivalAward = new MovieFestivalAward
                {
                    AwardFestival_Award_Id = movieFestivalAward.AwardFestival_Award_Id,
                    AwardFestival_Festival_Id = movieFestivalAward.AwardFestival_Festival_Id,
                    MovieFestival_Movie_Id = movieFestivalAward.MovieFestival_Movie_Id,
                    Deleted = false
                };
                dbContext.MovieFestivalAwards.Add(newMovieFestivalAward);
                dbContext.SaveChanges();
            }
        }
    }
}
