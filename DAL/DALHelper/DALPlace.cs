using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALPlace
    {

        public List<Place> AllPlaces()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Places.Where(s => s.Deleted == false).ToList();
            }
        }

        public Place GetPlaceById(int placeId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Places.Where(b => b.Id == placeId && b.Deleted == false).FirstOrDefault();
            }
        }

        public void AddPlace(Place place)
        {
            try
            {
                if (place.Id == 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var dis = dbContext.Places.Where(b => b.Id == place.Id).FirstOrDefault();
                        if (dis == null)
                        {
                            Place newPlace = new Place
                            {
                                Name = place.Name,
                                Deleted = false
                            };
                            dbContext.Places.Add(newPlace);
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

        public void UpdatePlace(Place place)
        {
            try
            {
                if (place.Id != 0)
                {
                    using (IMDBEntities7 dbContext = new IMDBEntities7())
                    {
                        var fest = dbContext.Places.Where(b => b.Id == place.Id).FirstOrDefault();
                        if (fest != null)
                        {
                            fest.Name = place.Name;
                            fest.Deleted = place.Deleted;
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

        public bool DeletePlace(int placeId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var place = dbContext.Places.Where(b => b.Id == placeId && b.Deleted == false).FirstOrDefault();

                    if (place != null)
                    {
                        place.Deleted = true;
                        dbContext.Entry(place).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        DALFestival dALFestival = new DALFestival();
                        DALCinema dALCinema = new DALCinema();

                        var festivals = dbContext.Festivals.Where(c => c.Place_Id == placeId && c.Deleted == false).ToList();
                        festivals.ForEach(c =>
                        {
                            dALFestival.DeleteFestival(c.Id);
                        });

                        var cinemas = dbContext.Cinemas.Where(c => c.Place_Id == placeId && c.Deleted == false).ToList();
                        cinemas.ForEach(c =>
                        {
                            dALCinema.DeleteCinema(c.Id);
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

    }
}
