﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IMDBEntities7 : DbContext
    {
        public IMDBEntities7(bool isLazy) :
                                base("name=IMDBEntities7")
        {
            Configuration.LazyLoadingEnabled = isLazy;
        }

        public IMDBEntities7()
            : this(false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<AwardFestival> AwardFestivals { get; set; }
        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<Critic> Critics { get; set; }
        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<Distributor> Distributors { get; set; }
        public virtual DbSet<Festival> Festivals { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieFestivalAward> MovieFestivalAwards { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ShowsMovie> ShowsMovies { get; set; }
    
        [DbFunction("IMDBEntities7", "SearchMovieByTitle")]
        public virtual IQueryable<SearchMovieByTitle_Result> SearchMovieByTitle(string searchText)
        {
            var searchTextParameter = searchText != null ?
                new ObjectParameter("searchText", searchText) :
                new ObjectParameter("searchText", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<SearchMovieByTitle_Result>("[IMDBEntities7].[SearchMovieByTitle](@searchText)", searchTextParameter);
        }
    
        public virtual int AddActs(Nullable<int> actorId, Nullable<int> movieId)
        {
            var actorIdParameter = actorId.HasValue ?
                new ObjectParameter("actorId", actorId) :
                new ObjectParameter("actorId", typeof(int));
    
            var movieIdParameter = movieId.HasValue ?
                new ObjectParameter("movieId", movieId) :
                new ObjectParameter("movieId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddActs", actorIdParameter, movieIdParameter);
        }
    
        public virtual int AddBelongsToGenre(Nullable<int> genreId, Nullable<int> movieId)
        {
            var genreIdParameter = genreId.HasValue ?
                new ObjectParameter("genreId", genreId) :
                new ObjectParameter("genreId", typeof(int));
    
            var movieIdParameter = movieId.HasValue ?
                new ObjectParameter("movieId", movieId) :
                new ObjectParameter("movieId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddBelongsToGenre", genreIdParameter, movieIdParameter);
        }
    
        public virtual int AddMovieFestival(Nullable<int> festivalId, Nullable<int> movieId)
        {
            var festivalIdParameter = festivalId.HasValue ?
                new ObjectParameter("festivalId", festivalId) :
                new ObjectParameter("festivalId", typeof(int));
    
            var movieIdParameter = movieId.HasValue ?
                new ObjectParameter("movieId", movieId) :
                new ObjectParameter("movieId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddMovieFestival", festivalIdParameter, movieIdParameter);
        }
    }
}