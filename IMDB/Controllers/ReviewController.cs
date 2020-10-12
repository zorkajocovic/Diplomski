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
    public class ReviewController : ApiController
    {
        private DALReview _dALReview = new DALReview();

        // GET: api/Review
        public List<Review> GetReviews()
        {
            return _dALReview.AllReviews();
        }

        [ResponseType(typeof(Review))]
        public IHttpActionResult GetReview(int id)
        {
            Review review = _dALReview.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        [HttpGet]
        [Route("api/GetReviewsForMovie")]
        public List<Review> GetReviewsForMovie(int movieId)
        {
            return _dALReview.AllReviewsForMovie(movieId);
        }

        [HttpGet]
        [Route("api/ApproveReview")]
        public void ApproveReview(int reviewId)
        {
            var review = _dALReview.GetReviewById(reviewId);
            if (review != null)
            {
                review.Approved = true;
                review.ApprovalDateTime = DateTime.Now;
       
                _dALReview.UpdateReview(review);
            }
        }
        // POST: api/Review
        [ResponseType(typeof(Review))]
        public IHttpActionResult PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dALReview.AddReview(review);

            return CreatedAtRoute("DefaultApi", new { id = review.Id }, review);
        }

        [HttpGet]
        [Route("api/DeleteReview")]
        public IHttpActionResult DeleteReview(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Review review = _dALReview.GetReviewById(id);

            if (id != review.Id)
            {
                return BadRequest();
            }

            review.Deleted = true;
            _dALReview.DeleteReview(review.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Review/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.Id)
            {
                return BadRequest();
            }

            _dALReview.UpdateReview(review);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
