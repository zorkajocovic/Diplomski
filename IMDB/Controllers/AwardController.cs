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
    public class AwardController : ApiController
    {
        private DALAward _dALAward = new DALAward();

        public List<Award> GetAwards()
        {
            return _dALAward.AllAwards();
        }

        [ResponseType(typeof(Award))]
        public IHttpActionResult GetAward(int id)
        {
            Award award = _dALAward.GetAwardById(id);
            if (award == null)
            {
                return NotFound();
            }
            return Ok(award);
        }

        [HttpGet]
        [Route("api/GetAwardsForMovie")]
        public List<MovieFestivalAward> GetAwardsForMovie(int movieId)
        {
            return _dALAward.GetAwardsForMovie(movieId);
        }

        [HttpGet]
        [Route("api/GetAwardsForFestival")]
        public List<AwardFestival> GetAwardsForFestival(int festivalId)
        {
            return _dALAward.GetAwardForFestival(festivalId);
        }

        [ResponseType(typeof(Award))]
        public IHttpActionResult PostAward(Award award)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALAward.AddAward(award);

            return CreatedAtRoute("DefaultApi", new { id = award.Id }, award);
        }

        [HttpGet]
        [Route("api/AddNewAwardForFestival")]
        public void AddNewAwardForFestival(int festivalId)
        {
            var lastAddedAward = _dALAward.AllAwards().LastOrDefault();
            _dALAward.AddAwardFestival(new AwardFestival()
            {
                Award_Id = lastAddedAward.Id,
                Festival_Id = festivalId
            });
        }

        [HttpGet]
        [Route("api/AddAwardForFestival")]
        public void AddAwardForFestival(int festivalId, int awardId)
        {
            _dALAward.AddAwardFestival(new AwardFestival()
            {
                Award_Id = awardId,
                Festival_Id = festivalId
            });
        }

        [HttpGet]
        [Route("api/DeleteAward")]
        public IHttpActionResult DeleteAward(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Award award = _dALAward.GetAwardById(id);

            if (id != award.Id)
            {
                return BadRequest();
            }

            award.Deleted = true;
            _dALAward.DeleteAward(award.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutAward(int id, Award award)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != award.Id)
            {
                return BadRequest();
            }

            _dALAward.UpdateAward(award);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
