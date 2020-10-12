using DAL;
using DAL.DALHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace IMDB.Controllers
{
    public class ActorController : ApiController
    {
        private DALActor _dALActor = new DALActor();

        // GET: api/Actor
        public List<Actor> GetActors()
        {
            return _dALActor.AllActors();
        }

        [ResponseType(typeof(Actor))]
        public IHttpActionResult GetActor(int id)
        {
            Actor actor = _dALActor.GetActorById(id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(actor);
        }

        // POST: api/Actor
        [ResponseType(typeof(Actor))]
        public IHttpActionResult PostActor()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/actors");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var actor = JsonConvert.DeserializeObject<Actor>(HttpContext.Current.Request.Form[0]);

            Validate(actor);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (httpPostedFile != null)
            {
                // Validate the uploaded image(optional)
                var extension = new FileInfo(httpPostedFile.FileName).Extension;
                var fileName = Guid.NewGuid() + extension;
                // Get the complete file path
                var fileSavePath = Path.Combine(root, fileName);

                while (File.Exists(fileSavePath))
                {
                    fileName = Guid.NewGuid() + extension;
                    fileSavePath = Path.Combine(root, fileName);
                }
                // Save the uploaded file to "UploadedFiles" folder
                httpPostedFile.SaveAs(fileSavePath);
                actor.Image = "http://localhost:50000/Content/images/actors/" + fileName;
            }

            _dALActor.AddActor(actor);
            
            return CreatedAtRoute("DefaultApi", new { id = actor.Id }, actor);
        }

        [HttpGet]
        [Route("api/DeleteActor")]
        public IHttpActionResult DeleteActor(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Actor actor = _dALActor.GetActorById(id);
          
            if (id != actor.Id)
            {
                return BadRequest();
            }

            actor.Deleted = true;
            _dALActor.DeleteActor(actor.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/DeleteActorFromMovie")]
        public void DeleteActorFromMovie(int actorId, int movieId)
        {
            _dALActor.DeleteActorForMovie(actorId, movieId);
        }

        [Route("api/GetActorsNotInMovie")]
        public List<Actor> GetActorsNotInMovie(int movieId)
        {
            List<Actor> allActors = _dALActor.AllActors();
            DALMovie dALMovie = new DALMovie();

            var movie = dALMovie.GetMovieById(movieId);

            movie.Actors.Where(a => !a.Deleted).ToList().ForEach(a => {
                allActors.RemoveAll(c => c.Id == a.Id);
            });

            return allActors;
        }

        // PUT: api/Actor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutActor(int id)
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/actors");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var actor = JsonConvert.DeserializeObject<Actor>(HttpContext.Current.Request.Form[0]);

            Validate(actor);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (httpPostedFile != null)
            {
                // Validate the uploaded image(optional)
                var extension = new FileInfo(httpPostedFile.FileName).Extension;
                var fileName = Guid.NewGuid() + extension;
                // Get the complete file path
                var fileSavePath = Path.Combine(root, fileName);

                while (File.Exists(fileSavePath))
                {
                    fileName = Guid.NewGuid() + extension;
                    fileSavePath = Path.Combine(root, fileName);
                }
                // Save the uploaded file to "UploadedFiles" folder
                httpPostedFile.SaveAs(fileSavePath);
                actor.Image = "http://localhost:50000/Content/images/actors/" + fileName;
            }


            if (id != actor.Id)
            {
                return BadRequest();
            }
           
             _dALActor.UpdateActor(actor);
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/PutActor")]
        public IHttpActionResult PutActor(int id, Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actor.Id)
            {
                return BadRequest();
            }

            _dALActor.UpdateActor(actor);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
