using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALCritic
    {

        public List<Critic> AllCritics()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Critics.Where(s => s.Deleted == false).ToList();
            }
        }

        public Critic GetCriticById(int criticId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Critics.Where(b => b.Id == criticId && b.Deleted == false).FirstOrDefault();
            }
        }

        public void AddCritic(Critic critic)
        {
            if (critic.Id == 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Actors.Where(b => b.Id == critic.Id && b.Deleted == false).FirstOrDefault();
                    if (fest == null)
                    {
                        Critic newCritic = new Critic
                        {
                            Name = critic.Name,
                            Surname = critic.Surname,
                            ReliabilityScore = critic.ReliabilityScore,
                            Deleted = false
                        };
                        dbContext.Critics.Add(newCritic);
                        dbContext.SaveChanges();
                    }
                }
            }       
        }

        public void UpdateCritic(Critic critic)
        {
            if (critic.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Critics.Where(b => b.Id == critic.Id).FirstOrDefault();
                    if (fest != null)
                    {
                        fest.Name = critic.Name;
                        fest.Surname = critic.Surname;
                        fest.Deleted = critic.Deleted;
                        fest.ReliabilityScore = critic.ReliabilityScore;
                        dbContext.Entry(fest).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }         
        }

        public bool DeleteCritic(int criticId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var critic = dbContext.Critics.Where(b => b.Id == criticId && b.Deleted == false).FirstOrDefault();

                    //brisemo kriticara za taj film
                    if (critic != null)
                    {
                        critic.Deleted = true;
                        dbContext.Entry(critic).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        var reviews = dbContext.Reviews.Where(c => c.Critic_Id == criticId && c.Deleted == false).ToList();
                        DALReview dALReview = new DALReview();
                        reviews.ForEach(c =>
                        {
                            dALReview.DeleteReview(c.Id);
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


    }
}
