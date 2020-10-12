using DAL;
using DAL.DALHelper;
using MovieUI.Common;
using MovieUI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieUI.ViewModel
{
    public class MovieDetailsViewModel : TemplateForProperties
    {
        MovieDetails _movieDetailsWin;
        Movie _currentMovie;
        DALActor _dALActor = new DALActor();
        DALGenre _dALGenre = new DALGenre();
        DALDistributor _dALDistributor = new DALDistributor();
        DALDirector _dALDirector = new DALDirector();
        DALMovie _dALMovie = new DALMovie();
        DALReview _dALReview = new DALReview();
        DALAward _dALAward = new DALAward();
        Distributor _currentDistributor;
        Director _currentDirector;
        List<ShowsMovie> _showsMovies = new List<ShowsMovie>();
        List<Review> _movieReviews = new List<Review>();
        List<MovieFestivalAward> _movieFestivalAwards = new List<MovieFestivalAward>();
        string director;
        string distributor;
        string duration;
        string genres;
        string actors;
        string description;
        string title;
        decimal? metaScore;

        public MovieDetailsViewModel(MovieDetails window, Movie currentMovie)
        {
            _movieDetailsWin = window;
            _currentMovie = _dALMovie.GetMovieById(currentMovie.Id);
            _currentDistributor = _dALDistributor.GetDistributorById(_currentMovie.Distributor_Id);
            _currentDirector = _dALDirector.GetDirectorById(_currentMovie.Director_Id);
            director = _currentDirector.Name + " " + _currentDirector.Surname;
            distributor = _currentDistributor.Name;
            duration = _currentMovie.Duration.ToString() + "min";
            description = _currentMovie.Desctiprion;
            title = _currentMovie.Title;
            _showsMovies = _currentMovie.ShowsMovies.ToList();
            _movieReviews = _dALReview.AllReviewsForMovie(_currentMovie.Id);
            _movieFestivalAwards = _dALAward.GetAwardsForMovie(_currentMovie.Id);
            metaScore = _currentMovie.MetaScore;
        }

        private string ActorsForMovie()
        {
            int iterator = 0;

            foreach (var actor in _currentMovie.Actors)
            {
                var movieActor = _dALActor.GetActorById(actor.Id);
                if (movieActor != null)
                {
                    actors += movieActor.Name + " " + movieActor.Surname;
                    iterator++;
                    if (iterator < _currentMovie.Actors.Count)
                    {
                        actors += ", ";
                    }
                }
            }      
            return actors;
        }

        private string GenresForMovie()
        {
            int iterator = 0;
            foreach (var genre in _currentMovie.Genres)
            {
                var movieGenre = _dALGenre.GetGenreById(genre.Id);
                if (movieGenre != null)
                {
                    genres += movieGenre.Name;
                    iterator++;
                    if (iterator < _currentMovie.Genres.Count)
                    {
                        genres += ", ";
                    }
                }
            }
            return genres;
        }

        public List<Review> MovieReviews
        {
            get { return _movieReviews; }
            set
            {
                _movieReviews = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MovieReviews"));
            }
        }

        public decimal? MetaScore
        {
            get { return metaScore; }
            set
            {
                metaScore = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MetaScore"));
            }
        }

        public List<MovieFestivalAward> AwardsAndFestivals
        {
            get { return _movieFestivalAwards; }
            set
            {
                _movieFestivalAwards = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AwardsAndFestivals"));
            }
        }

        public Movie Movie
        {
            get { return _currentMovie; }
            set
            {
                _currentMovie = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Movie"));
            }
        }

        public string Actors
        {
            get { return ActorsForMovie(); }
            set
            {
                actors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Actors"));
            }
        }

        public string Genres
        {
            get { return GenresForMovie(); }
            set
            {
                actors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Genres"));
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public string Director
        {
            get { return director; }
            set
            {
                director = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Director"));
            }
        }

        public string Distributor
        {
            get { return distributor; }
            set
            {
                distributor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distributor"));
            }
        }

        public string Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Duration"));
            }
        }

        public List<ShowsMovie> ShowingCinemas
        {
            get { return _showsMovies; }
            set
            {
                _showsMovies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowingCinemas"));
            }
        }

        private ICommand addReview;
        public ICommand AddReview
        {
            get
            {
                if (addReview == null)
                {
                    addReview = new RelayCommand(param => AddMovieReviewExecute(), param => true);
                }
                return addReview;
            }
        }

        private void AddMovieReviewExecute()
        {
            AddReview command = new AddReview(_currentMovie.Id);
            command.ShowDialog();
            MovieReviews = _dALReview.AllReviewsForMovie(_currentMovie.Id);
            MetaScore = _currentMovie.MetaScore;
        }
    }
}
