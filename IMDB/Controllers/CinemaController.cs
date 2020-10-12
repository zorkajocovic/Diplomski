using DAL;
using DAL.DALHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace IMDB.Controllers
{
    public class CinemaController : ApiController
    {
        private DALCinema _dALCinema = new DALCinema();

        // GET: api/Cinema
        public List<Cinema> GetCinemas()
        {
            return _dALCinema.AllCinemas();
        }

        [HttpGet]
        [Route("api/GetMoviesInCinema")]
        public List<ShowsMovie> GetMoviesInCinema()
        {
            return _dALCinema.AllShowsMovies();
        }

        [ResponseType(typeof(Cinema))]
        public IHttpActionResult GetCinema(int id)
        {
            Cinema cinema = _dALCinema.GetCinemaById(id);
            if (cinema == null)
            {
                return NotFound();
            }
            return Ok(cinema);
        }

        // POST: api/Cinema
        [ResponseType(typeof(Cinema))]
        public IHttpActionResult PostCinema(Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALCinema.AddCinema(cinema);

            return CreatedAtRoute("DefaultApi", new { id = cinema.Id }, cinema);
        }

        [HttpGet]
        [Route("api/DeleteCinema")]
        public IHttpActionResult DeleteCinema(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cinema cinema = _dALCinema.GetCinemaById(id);

            if (id != cinema.Id)
            {
                return BadRequest();
            }

            cinema.Deleted = true;
            _dALCinema.DeleteCinema(cinema.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Cinema/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCinema(int id, Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cinema.Id)
            {
                return BadRequest();
            }

            _dALCinema.UpdateCinema(cinema);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/GetCinemasNotShowingMovie")]
        public List<Cinema> GetCinemasNotShowingMovie(int movieId)
        {
            List<Cinema> showingCinemas = new List<Cinema>();
            List<Cinema> noShowinginemas = new List<Cinema>();
            List<Cinema> allCinemas = _dALCinema.AllCinemas();

            var showing = _dALCinema.AllShowsMovies().Where(s => s.Movie_Id == movieId).ToList();

            showing.ForEach(c => {
                if (!showingCinemas.Contains(_dALCinema.GetCinemaById(c.Cinema_Id)))
                {
                    showingCinemas.Add(_dALCinema.GetCinemaById(c.Cinema_Id));
                }
            });

            if(showingCinemas.Count == 0)
            {
                noShowinginemas.AddRange(_dALCinema.AllCinemas());
            }
            else
            {
                showingCinemas.ForEach(s => {
                     allCinemas.RemoveAll(c => c.Id == s.Id);
                });
                noShowinginemas = allCinemas;
            }

            return noShowinginemas;
        }
    }
}
