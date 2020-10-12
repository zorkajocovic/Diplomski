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
    public class DirectorController : ApiController
    {
        private DALDirector _dALDirector = new DALDirector();

        // GET: api/Director
        public List<Director> GetDirectors()
        {
            return _dALDirector.AllDirectors();
        }

        [ResponseType(typeof(Director))]
        public IHttpActionResult GetDirector(int id)
        {
            Director director = _dALDirector.GetDirectorById(id);
            if (director == null)
            {
                return NotFound();
            }
            return Ok(director);
        }

        // POST: api/Director
        [ResponseType(typeof(Director))]
        public IHttpActionResult PostDirector()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/directors");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var director = JsonConvert.DeserializeObject<Director>(HttpContext.Current.Request.Form[0]);

            Validate(director);
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
                director.Image = "http://localhost:50000/Content/images/directors/" + fileName;
            }

            _dALDirector.AddDirector(director);

            return CreatedAtRoute("DefaultApi", new { id = director.Id }, director);
        }

        [HttpGet]
        [Route("api/DeleteDirector")]
        public IHttpActionResult DeleteDirector(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Director director = _dALDirector.GetDirectorById(id);

            if (id != director.Id)
            {
                return BadRequest();
            }

            director.Deleted = true;
            _dALDirector.DeleteDirector(director.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutDirector(int id)
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/directors");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var director = JsonConvert.DeserializeObject<Director>(HttpContext.Current.Request.Form[0]);

            Validate(director);
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
                director.Image = "http://localhost:50000/Content/images/directors/" + fileName;
            }

            if (id != director.Id)
            {
                return BadRequest();
            }

            _dALDirector.UpdateDirector(director);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/PutDirector")]
        public IHttpActionResult PutDirector(int id, Director director)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != director.Id)
            {
                return BadRequest();
            }

            _dALDirector.UpdateDirector(director);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
