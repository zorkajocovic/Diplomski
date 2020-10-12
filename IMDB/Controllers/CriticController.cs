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
    public class CriticController : ApiController
    {
        private DALCritic _dALCritic = new DALCritic();

        // GET: api/Critic
        public List<Critic> GetCritics()
        {
            return _dALCritic.AllCritics();
        }

        [ResponseType(typeof(Critic))]
        public IHttpActionResult GetCritic(int id)
        {
            Critic critic = _dALCritic.GetCriticById(id);
            if (critic == null)
            {
                return NotFound();
            }
            return Ok(critic);
        }

        // POST: api/Critic
        [ResponseType(typeof(Critic))]
        public IHttpActionResult PostCritic(Critic critic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALCritic.AddCritic(critic);

            return CreatedAtRoute("DefaultApi", new { id = critic.Id }, critic);
        }

        [HttpGet]
        [Route("api/DeleteCritic")]
        public IHttpActionResult DeleteCritic(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Critic critic = _dALCritic.GetCriticById(id);

            if (id != critic.Id)
            {
                return BadRequest();
            }

            critic.Deleted = true;
            _dALCritic.DeleteCritic(critic.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Critic/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCritic(int id, Critic critic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != critic.Id)
            {
                return BadRequest();
            }

            _dALCritic.UpdateCritic(critic);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
