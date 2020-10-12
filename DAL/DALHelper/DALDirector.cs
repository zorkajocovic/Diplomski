using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALDirector
    {

        public List<Director> AllDirectors()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Directors.Where(s => s.Deleted == false).ToList();
            }
        }

        public void AddDirector(Director director)
        {
            try
            {
                if (director.Id == 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var dir = dbContext.Directors.Where(b => b.Id == director.Id).FirstOrDefault();
                        if (dir == null)
                        {
                            Director newDirector = new Director
                            {
                                Name = director.Name,
                                Surname = director.Surname,
                                Image = director.Image,
                                Birthday = director.Birthday,
                                BirthPlace = director.BirthPlace,
                                Country = director.Country,
                                Deleted = false
                            };
                            dbContext.Directors.Add(newDirector);
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

        public void UpdateDirector(Director director)
        {
            if (director.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var ex_director = dbContext.Directors.Where(b => b.Id == director.Id).FirstOrDefault();
                    if (ex_director != null)
                    {
                        ex_director.Name = director.Name;
                        ex_director.Surname = director.Surname;
                        ex_director.Deleted = director.Deleted;
                        ex_director.Image = director.Image;
                        ex_director.Country = director.Country;
                        ex_director.BirthPlace = director.BirthPlace;
                        ex_director.Birthday = director.Birthday;
                        dbContext.Entry(ex_director).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteDirector(int directorId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                var director = dbContext.Directors.Where(b => b.Id == directorId && b.Deleted == false).FirstOrDefault();
                if (director != null)
                {
                    director.Deleted = true;
                    dbContext.Entry(director).State = EntityState.Modified;
                    dbContext.SaveChanges();

                    DALMovie dALMovie = new DALMovie();
                    //kad brisemo rezisera, nece postojati vise ni filmovi koji su vezani za tog rezisera
                    var directedMovies = dbContext.Movies.Where(m => m.Director_Id == directorId && m.Deleted == false).ToList();
                    if (directedMovies != null)
                    {
                        directedMovies.ForEach(d =>
                        {
                            dALMovie.DeleteMovie(d.Id);
                        });
                    }
                    return true;
                }
                else
                    return false;
            }
        }
        
        public Director GetDirectorById(int directorId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Directors.Where(b => b.Id == directorId && b.Deleted == false).FirstOrDefault();
            }
        }
    }
}
