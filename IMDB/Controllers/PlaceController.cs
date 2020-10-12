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
    public class PlaceController : ApiController
    {
        private DALPlace _dALPlace = new DALPlace();

        // GET: api/Place
        public List<Place> GetPlaces()
        { 
            return _dALPlace.AllPlaces();
        }

        [ResponseType(typeof(Place))]
        public IHttpActionResult GetPlace(int id)
        {
            Place place = _dALPlace.GetPlaceById(id);
            if (place == null)
            {
                return NotFound();
            }
            return Ok(place);
        }

        // POST: api/Place
        [ResponseType(typeof(Place))]
        public IHttpActionResult PostPlace(Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALPlace.AddPlace(place);

            return CreatedAtRoute("DefaultApi", new { id = place.Id }, place);
        }

        [HttpGet]
        [Route("api/DeletePlace")]
        public IHttpActionResult DeletePlace(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Place place = _dALPlace.GetPlaceById(id);

            if (id != place.Id)
            {
                return BadRequest();
            }

            place.Deleted = true;
            _dALPlace.DeletePlace(place.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Place/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlace(int id, Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.Id)
            {
                return BadRequest();
            }

            _dALPlace.UpdatePlace(place);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
