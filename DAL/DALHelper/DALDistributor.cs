using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALDistributor
    {

        public List<Distributor> AllDistributors()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Distributors.Where(s => s.Deleted == false).ToList();
            }
        }

        public Distributor GetDistributorById(int disID)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Distributors.Where(b => b.Id == disID && b.Deleted == false).FirstOrDefault();

            }
        }

        public void AddDistributor(Distributor distributor)
        {
            try
            {
                if (distributor.Id == 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var dis = dbContext.Distributors.Where(b => b.Id == distributor.Id).FirstOrDefault();
                        if (dis == null)
                        {
                            Distributor newDis = new Distributor
                            {
                                Name = distributor.Name,
                                Deleted = false
                            };
                            dbContext.Distributors.Add(newDis);
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

        public void UpdateDistributor(Distributor distributor)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Distributors.Where(b => b.Id == distributor.Id).FirstOrDefault();
                    if (fest != null)
                    {
                        fest.Name = distributor.Name;
                        fest.Deleted = distributor.Deleted;
                        dbContext.Entry(fest).State = EntityState.Modified;
                        dbContext.SaveChanges();
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

        public bool DeleteDistributor(int distributorId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var distributor = dbContext.Distributors.Where(b => b.Id == distributorId && b.Deleted == false).FirstOrDefault();
                    if (distributor != null)
                    {
                        distributor.Deleted = true;
                        dbContext.Entry(distributor).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        DALMovie dALMovie = new DALMovie();
                        var distributedMovies = dbContext.Movies.Where(m => m.Distributor_Id == distributorId && m.Deleted == false).ToList();
                        //kad brisemo distributera, nece postojati vise ni filmovi vezani za njega
                        if (distributedMovies != null)
                        {
                            distributedMovies.ForEach(m =>
                            {
                                dALMovie.DeleteMovie(m.Id);
                            });
                        }

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
    }
}
