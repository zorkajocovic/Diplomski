using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALActor
    {

        public List<Actor> AllActors()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Actors.Where(s => s.Deleted == false).ToList();
            }
        }

        public Actor GetActorById(int actorId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Actors.Where(b => b.Id == actorId && b.Deleted == false).FirstOrDefault();
            }
        }

        public void DeleteActorForMovie(int actorId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7(true))
            {
                var movie = dbContext.Movies.Where(m => m.Id == movieId).FirstOrDefault();
                var actor = dbContext.Actors.Where(a => a.Id == actorId).FirstOrDefault();
                // dbContext.Entry(movie).Collection("Actors");
                // dbContext.Entry(actor).Collection("Movies");

                 movie.Actors.Remove(actor);
                 actor.Movies.Remove(movie);
                dbContext.SaveChanges();
            }
        }

        public void AddActor(Actor actor)
        {
            if (actor.Id == 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Actors.Where(b => b.Id == actor.Id && b.Deleted == false).FirstOrDefault();
                    if (fest == null)
                    {
                        Actor newActor = new Actor
                        {
                            Name = actor.Name,
                            Surname = actor.Surname,
                            Image = actor.Image,
                            Birthday = actor.Birthday,
                            BirthPlace = actor.BirthPlace,
                            Country = actor.Country,
                            Deleted = false
                        };
                        dbContext.Actors.Add(newActor);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public void UpdateActor(Actor actor)
        {
           if (actor.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var ex_actor = dbContext.Actors.Where(b => b.Id == actor.Id).FirstOrDefault();
                    if (ex_actor != null)
                    {
                        ex_actor.Name = actor.Name;
                        ex_actor.Deleted = actor.Deleted;
                        ex_actor.Surname = actor.Surname;
                        ex_actor.Image = actor.Image;
                        ex_actor.Country = actor.Country;
                        ex_actor.BirthPlace = actor.BirthPlace;
                        ex_actor.Birthday = actor.Birthday;
                        dbContext.Entry(ex_actor).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteActor(int actorId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var actor = dbContext.Actors.Where(m => m.Id == actorId && m.Deleted == false).FirstOrDefault();

                    if (actor != null)
                    {
                        actor.Deleted = true;
                        dbContext.Entry(actor).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }

                    if (actor != null)
                    {
                        List<Movie> movies = new List<Movie>();
                        foreach (var movie in dbContext.Movies)
                        {
                            movie.Actors.ToList().ForEach(m =>
                            {
                                if (m.Id == actorId)
                                {
                                    movies.Add(movie);
                                }
                            });
                        }
                        movies.ForEach(a =>
                        {
                            a.Actors.Remove(actor);
                        });
                        dbContext.SaveChanges();
                    }
                    return true;
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

        public void AddActsProcedure(int actorId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                dbContext.AddActs(actorId, movieId);
            }
        }
    }
}
