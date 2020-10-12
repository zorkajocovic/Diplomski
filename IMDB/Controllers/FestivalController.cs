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
    public class FestivalController : ApiController
    {
        private DALFestival _dALFestival = new DALFestival();

        // GET: api/Festival
        public List<Festival> GetFestivals()
        {
            return _dALFestival.AllFestivals();
        }

        [ResponseType(typeof(Festival))]
        public IHttpActionResult GetFestival(int id)
        {
            Festival festival = _dALFestival.GetFestivalById(id);
            if (festival == null)
            {
                return NotFound();
            }
            return Ok(festival);
        }

        [HttpGet]
        [Route("api/GetFestivalsForMovie")]
        public List<Festival> GetFestivalsForMovie(int movieId)
        {
            return _dALFestival.GetFestivalsForMovie(movieId);
        }

        // POST: api/Festival
        [ResponseType(typeof(Festival))]
        public IHttpActionResult PostFestival(Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALFestival.AddFestival(festival);

            return CreatedAtRoute("DefaultApi", new { id = festival.Id }, festival);
        }

        [HttpGet]
        [Route("api/DeleteFestival")]
        public IHttpActionResult DeleteFestival(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Festival festival = _dALFestival.GetFestivalById(id);

            if (id != festival.Id)
            {
                return BadRequest();
            }

            festival.Deleted = true;
            _dALFestival.DeleteFestival(festival.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Festival/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFestival(int id, Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != festival.Id)
            {
                return BadRequest();
            }

            _dALFestival.UpdateFestival(festival);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
