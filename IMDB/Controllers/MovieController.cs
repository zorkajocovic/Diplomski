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
    public class MovieController : ApiController
    {
        private DALMovie _dALMovie = new DALMovie();

        // GET: api/Movie
        public List<Movie> GetMovies()
        {
            return _dALMovie.AllMovies();
        }

        [ResponseType(typeof(Movie))]
        public IHttpActionResult GetMovie(int id)
        {
            Movie movie = _dALMovie.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("api/GetMoviesByGenre")]
        public List<Movie> GetMoviesByGenre(int genreId)
        {
            return _dALMovie.GetMoviesByGenre(genreId);
        }

        [HttpGet]
        [Route("api/GetMoviesByDistributor")]
        public List<Movie> GetMoviesByDistributor(int distributorId)
        {
            return _dALMovie.GetMoviesByDistributor(distributorId);
        }

        // POST: api/Movie
        [ResponseType(typeof(Movie))]
        public IHttpActionResult PostMovie()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/movies");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var movie = JsonConvert.DeserializeObject<Movie>(HttpContext.Current.Request.Form[0]);

            Validate(movie);
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
                movie.Image = "http://localhost:50000/Content/images/movies/" + fileName;
            }

            _dALMovie.AddMovie(movie);

            return CreatedAtRoute("DefaultApi", new { id = movie.Id }, movie);
        }

        [HttpGet]
        [Route("api/DeleteMovie")]
        public IHttpActionResult DeleteMovie(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie movie = _dALMovie.GetMovieById(id);

            if (id != movie.Id)
            {
                return BadRequest();
            }

            movie.Deleted = true;
            _dALMovie.DeleteMovie(movie.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutMovie(int id)
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/movies");

            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["image"];
            var movie = JsonConvert.DeserializeObject<Movie>(HttpContext.Current.Request.Form[0]);

            Validate(movie);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (httpPostedFile != null)
            {
                var extension = new FileInfo(httpPostedFile.FileName).Extension;
                var fileName = Guid.NewGuid() + extension;
                var fileSavePath = Path.Combine(root, fileName);

                while (File.Exists(fileSavePath))
                {
                    fileName = Guid.NewGuid() + extension;
                    fileSavePath = Path.Combine(root, fileName);
                }
                httpPostedFile.SaveAs(fileSavePath);
                movie.Image = "http://localhost:50000/Content/images/movies/" + fileName;
            }

            if (id != movie.Id)
            {
                return BadRequest();
            }

            _dALMovie.UpdateMovie(movie);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/PutMovie")]
        public IHttpActionResult PutMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.Id)
            {
                return BadRequest();
            }

            _dALMovie.UpdateMovie(movie);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/AddActorsForMovie")]
        public void AddActorsForMovie(int actorId, int movieId)
        {
            DALActor dALActor = new DALActor();
            dALActor.AddActsProcedure(actorId, movieId);
        }

        [HttpGet]
        [Route("api/AddGenresForMovie")]
        public void AddGenresForMovie(int genreId, int movieId)
        {
            DALGenre dALGenre = new DALGenre();
            dALGenre.AddBelongToGenreProcedure(genreId, movieId);
        }

        [HttpGet]
        [Route("api/AddFestivalForMovie")]
        public void AddFestivalsForMovie(int festivalId, int movieId)
        {
            DALFestival dALFestival = new DALFestival();
            dALFestival.AddMovieFestivalProcedure(festivalId, movieId);
        }

        [HttpPost]
        [Route("api/AddShowsMovie")]
        public void AddShowsMovie(ShowsMovie showsMovie)
        {
            DALCinema dALCinema = new DALCinema();
            dALCinema.AddShowsMovie(showsMovie);
        }

        [HttpGet]
        [Route("api/SearchMoviesByTitle")]
        public List<Movie> SearchMoviesByTitle(string filterText)
        {
            return _dALMovie.SearchMovies(filterText);
        }

        [HttpPost]
        [Route("api/AddAwardForMovie")]
        public void AddAwardForMovie(MovieFestivalAward movieFestivalAward)
        {
            DALAward dALAward = new DALAward();
            dALAward.AddMovieFestivalAward(movieFestivalAward);
        }

    }
}
