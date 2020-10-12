using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALHelper
{
    public class DALReview
    {

        public List<Review> AllReviews()
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Reviews
                                .Where(s => s.Deleted == false)
                                 .Include(s => s.Critic)
                                 .Include(s => s.Movie).ToList();
            }
        }

        public List<Review> AllReviewsForMovie(int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Reviews
                                .Where(s => s.Deleted == false && s.Movie_Id == movieId)
                                 .Include(s => s.Critic)
                                 .Include(s => s.Movie).ToList();
            }
        }

        public Review GetReviewById(int reviewId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Reviews
                                .Where(b => b.Id == reviewId && b.Deleted == false)
                                .Include(b => b.Critic).FirstOrDefault();
            }
        }

        public List<Review> GetApprovedReviewsForMovie(int reviewId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Reviews
                                .Where(b => b.Id == reviewId && b.Approved == true && !b.Deleted)
                                .Include(b => b.Critic).ToList();
            }

        }

        public List<Review> GetNotApprovedReviewsForMovie(int reviewId, int movieId)
        {
            using (IMDBEntities7 dbContext = new IMDBEntities7())
            {
                return dbContext.Reviews
                                .Where(b => b.Id == reviewId && b.Approved == false && !b.Deleted)
                                .Include(b => b.Critic).ToList();
            }

        }

        public void AddReview(Review review)
        {

            if (review.Id == 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Reviews.Where(b => b.Id == review.Id && b.Deleted == false).FirstOrDefault();
                    if (fest == null)
                    {
                        Review newReview = new Review()
                        {
                            Text = review.Text,
                            Critic_Id = review.Critic_Id,
                            Movie_Id = review.Movie_Id,
                            Rate = review.Rate,
                            Approved = false,
                            Deleted = false
                        };
                        dbContext.Reviews.Add(newReview);
                        dbContext.SaveChanges();

                    }
                }
            }
        }

        public void UpdateReview(Review review)
        {

            if (review.Id != 0)
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7())
                {
                    var fest = dbContext.Reviews.Where(b => b.Id == review.Id).FirstOrDefault();
                    if (fest != null)
                    {
                        fest.Text = review.Text;
                        fest.Critic_Id = review.Critic_Id;
                        fest.Rate = review.Rate;
                        fest.Deleted = review.Deleted;
                        fest.Approved = review.Approved;
                        fest.ApprovalDateTime = review.ApprovalDateTime;
                        dbContext.Entry(fest).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteReview(int reviewId)
        {
            try
            {
                using (IMDBEntities7 dbContext = new IMDBEntities7(true))
                {
                    var review = dbContext.Reviews.Where(b => b.Id == reviewId && b.Deleted == false).FirstOrDefault();
                    if (review != null)
                    {
                        review.Deleted = true;
                        dbContext.Entry(review).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        var movies = dbContext.Movies.Where(c => c.Deleted == false).ToList();

                        movies.ForEach(c =>
                        {
                            c.Reviews.Remove(review);
                        });
                      //  dbContext.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message);
                return false;
            }
        }
    }
}
