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
    public class DistributorController : ApiController
    {
        private DALDistributor _dALDistributor = new DALDistributor();

        // GET: api/Distributor
        public List<Distributor> GetDistributors()
        {
            return _dALDistributor.AllDistributors();
        }

        [ResponseType(typeof(Distributor))]
        public IHttpActionResult GetDistributor(int id)
        {
            Distributor distributor = _dALDistributor.GetDistributorById(id);
            if (distributor == null)
            {
                return NotFound();
            }
            return Ok(distributor);
        }

        // POST: api/Distributor
        [ResponseType(typeof(Distributor))]
        public IHttpActionResult PostDistributor(Distributor distributor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALDistributor.AddDistributor(distributor);

            return CreatedAtRoute("DefaultApi", new { id = distributor.Id }, distributor);
        }

        [HttpGet]
        [Route("api/DeleteDistributor")]
        public IHttpActionResult DeleteDistributor(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Distributor distributor = _dALDistributor.GetDistributorById(id);

            if (id != distributor.Id)
            {
                return BadRequest();
            }

            distributor.Deleted = true;
            _dALDistributor.DeleteDistributor(distributor.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Distributor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDistributor(int id, Distributor distributor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != distributor.Id)
            {
                return BadRequest();
            }

            _dALDistributor.UpdateDistributor(distributor);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
