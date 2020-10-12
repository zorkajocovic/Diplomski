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
    public class GenreController : ApiController
    {
        private DALGenre _dALGenre = new DALGenre();

        // GET: api/Genre
        public List<Genre> GetGenres()
        {
            return _dALGenre.AllGenres();
        }

        [ResponseType(typeof(Genre))]
        public IHttpActionResult GetGenre(int id)
        {
            Genre genre = _dALGenre.GetGenreById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // POST: api/Genre
        [ResponseType(typeof(Genre))]
        public IHttpActionResult PostGenre(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALGenre.AddGenre(genre);

            return CreatedAtRoute("DefaultApi", new { id = genre.Id }, genre);
        }

        [HttpGet]
        [Route("api/DeleteGenre")]
        public IHttpActionResult DeleteGenre(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Genre genre = _dALGenre.GetGenreById(id);

            if (id != genre.Id)
            {
                return BadRequest();
            }

            genre.Deleted = true;
            _dALGenre.DeleteGenre(genre.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/DeleteGenreFromMovie")]
        public IHttpActionResult DeleteGenreFromMovie(int genreId, int movieId)
        {
            _dALGenre.DeleteGenreForMovie(genreId, movieId);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/GetGenresNotInMovie")]
        public List<Genre> GetGenresNotInMovie(int movieId)
        {
            List<Genre> allGenres = _dALGenre.AllGenres();
            DALMovie dALMovie = new DALMovie();

            var movie = dALMovie.GetMovieById(movieId);

            movie.Genres.Where(a => !a.Deleted).ToList().ForEach(a => {
                allGenres.RemoveAll(c => c.Id == a.Id);
            });

            return allGenres;
        }

        // PUT: api/Genre/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGenre(int id, Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genre.Id)
            {
                return BadRequest();
            }

            _dALGenre.UpdateGenre(genre);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
