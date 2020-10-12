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
using System.Windows;
using System.Windows.Input;

namespace MovieUI.ViewModel
{
    public class AddMovieViewModel : TemplateForProperties
    {
        private Movie _movie = new Movie();
        private DALMovie _dALMovie = new DALMovie();
        private DALFestival _dALFestival = new DALFestival();
        private DALActor _dALActor = new DALActor();
        private DALGenre _dALGenre = new DALGenre();
        private DALAward _dALAward = new DALAward();
        private DALCinema _dALCinema = new DALCinema();
        private DALDistributor _dALDistributor = new DALDistributor();
        private DALDirector _dALDirector = new DALDirector();
        private AddMovie actor_window;
        private List<string> _allDirectors = new List<string>();
        private List<string> _allDistributors = new List<string>();
        private List<string> _allActors = new List<string>();
        private List<string> _allGenres = new List<string>();
        private List<string> _allCinemas = new List<string>();
        private List<string> _allCritics = new List<string>();
        private List<string> _allCriticism = new List<string>();
        private List<string> _allFestivals = new List<string>();
        private List<string> _allAwards = new List<string>();
        private List<Actor> _movieActors = new List<Actor>();
        private List<Genre> _movieGenres = new List<Genre>();
        private List<ShowsMovie> _showsMovie = new List<ShowsMovie>();
        private List<Review> _movieReviews = new List<Review>();
        private List<MovieFestivalAward> _movieFestivalAward = new List<MovieFestivalAward>();
        private List<Festival> _movieFestivals = new List<Festival>();
        private string _director;
        private string _distributor;
        private string _actor;
        private string _genre;
        private string _cinema;
        private string _festival;
        private string _award;
        private string _toDate;
        private string _fromDate;
        private int _selectedActor;
        private int _selectedDistributor;
        private int _selectedDirector;
        private int _selectedCinema;
        private int _selectedFestival;
        private int _selectedGenre;
        private int _selectedAward;
        private bool notAwarded;

        public AddMovieViewModel(AddMovie window)
        {
            actor_window = window;
            _festival = null;
            notAwarded = false;
        }

        public Movie Movie
        {
            get { return _movie; }
            set
            {
                _movie = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Movie"));
            }
        }

        public string Actor
        {
            get { return _actor; }
            set
            {
                _actor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Actor"));
            }
        }

        public string Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Genre"));
            }
        }

        public string Festival
        {
            get { return _festival; }
            set
            {
                _festival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festival"));
            }
        }

        public string Award
        {
            get { return _award; }
            set
            {
                _award = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Award"));
            }
        }

        public string Distributor
        {
            get { return _distributor; }
            set
            {
                _distributor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distributor"));
            }
        }

        public bool NotAwarded
        {
            get { return notAwarded; }
            set
            {
                notAwarded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NotAwarded"));
            }
        }

        public int SelectedActor
        {
            get { return _selectedActor; }
            set
            {
                _selectedActor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActor"));
            }
        }

        public int SelectedDistributor
        {
            get { return _selectedDistributor; }
            set
            {
                _selectedDistributor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDistributor"));
            }
        }

        public int SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGenre"));
            }
        }

        public int SelectedDirector
        {
            get { return _selectedDirector; }
            set
            {
                _selectedDirector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDirector"));
            }
        }

        public int SelectedCinema
        {
            get { return _selectedCinema; }
            set
            {
                _selectedCinema = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCinema"));
            }
        }

        public int SelectedFestival
        {
            get { return _selectedFestival; }
            set
            {
                _selectedFestival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFestival"));
            }
        }

        public int SelectedAward
        {
            get { return _selectedAward; }
            set
            {
                _selectedAward = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAward"));
            }
        }

        public List<string> Festivals
        {
            get { return AllFestivals(); }
            set
            {
                _allFestivals = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festivals"));
            }
        }

        public List<string> Awards
        {
            get { return _allAwards; }
            set
            {
                _allAwards = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Awards"));
            }
        }

        public void AllAwards()
        {
            Awards = AllAwardsForFestival();
        }

        private List<string> AllFestivals()
        {
            _allFestivals = new List<string>();
            var allFestivals = _dALFestival.AllFestivals();
            foreach (var festival in allFestivals)
            {
                _allFestivals.Add(festival.Name);
            }
            return _allFestivals;
        }

        private List<string> AllAwardsForFestival()
        {
            if(Festival != null && !NotAwarded)
            {
                int festivalId = _dALFestival.AllFestivals().ElementAt(SelectedFestival).Id;
                _allAwards = new List<string>();
               
                var awardFestival = _dALAward.GetAwardForFestival(festivalId);        
                awardFestival.ForEach(a => {
                    var awardForFestival = _dALAward.GetAwardById(a.Award_Id);
                    _allAwards.Add(awardForFestival.Name);
                });    
            }
          
            return _allAwards;
        }
        
        public string FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }

        public string ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }

        public string Director
        {
            get { return _director; }
            set
            {
                _director = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Director"));
            }
        }

        public string Cinema
        {
            get { return _cinema; }
            set
            {
                _cinema = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cinema"));
            }
        }

        public List<string> Genres
        {
            get { return AllGenres(); }
            set
            {
                _allGenres = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Genres"));
            }
        }

        public List<string> Cinemas
        {
            get { return AllCinemas(); }
            set
            {
                _allCinemas = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cinemas"));
            }
        }

        public List<string> MovieActors
        {
            get { return AllActors(); }
            set
            {
                _allActors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MovieActors"));
            }
        }
        

        public List<string> Directors
        {
            get { return AllDirectors(); }
            set
            {
                _allDirectors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Directors"));
            }
        }

        public List<string> Distributors
        {
            get { return AllDistributors(); }
            set
            {
                _allDistributors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distributors"));
            }
        }
      
        private List<string> AllActors()
        {        
            _allActors = new List<string>();
            var allActors = _dALActor.AllActors();
            foreach (var actor in allActors)
            {
                _allActors.Add(actor.Name + " " + actor.Surname);
            }
            return _allActors;
        }

        private List<string> AllGenres()
        {
            var allGenres = _dALGenre.AllGenres();
            foreach (var genre in allGenres)
            {
                _allGenres.Add(genre.Name);
            }
            return _allGenres;          
        }

        private List<string> AllCinemas()
        {
            _allCinemas = new List<string>();
            var allCinemas = _dALCinema.AllCinemas();
            foreach (var cinema in allCinemas)
            {
                _allCinemas.Add(cinema.Name);
            }
            return _allCinemas;
        }

        private List<string> AllDirectors()
        {
            try
            {
                var allDirectors = _dALDirector.AllDirectors();
                foreach (var dir in allDirectors)
                {
                    _allDirectors.Add(dir.Name + " " + dir.Surname);
                }

                return _allDirectors;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<string> AllDistributors()
        {
            try
            {
                var allDistributors = _dALDistributor.AllDistributors();
                foreach (var dis in allDistributors)
                {
                    _allDistributors.Add(dis.Name);
                }

                return _allDistributors;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private ICommand addGenre;
        public ICommand AddGenre
        {
            get
            {
                if (addGenre == null)
                {
                    addGenre = new RelayCommand(param => AddGenreExecute(), param => CanAddGenreExecute());
                }
                return addGenre;
            }
        }

        private void AddGenreExecute()
        {
            try
            {
                int selectedGenreId = _dALGenre.AllGenres().ElementAt(SelectedGenre).Id;
                var genre = _dALGenre.GetGenreById(selectedGenreId);

                if (!_movieGenres.Exists(g => g.Id == genre.Id))
                {
                    _movieGenres.Add(genre);
                }
                Genre = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddGenreExecute()
        {
            return Genre != null ? true : false;
        }

        private ICommand addCinema;
        public ICommand AddCinema
        {
            get
            {
                if (addCinema == null)
                {
                    addCinema = new RelayCommand(param => AddCinemaExecute(), param => CanACinemaExecute());
                }
                return addCinema;
            }
        }

       
        private void AddCinemaExecute()
        {
            try
            {
                int selectedCinemaId = _dALCinema.AllCinemas().ElementAt(SelectedCinema).Id;
                var cinema = _dALCinema.GetCinemaById(selectedCinemaId);

                _showsMovie.Add(new ShowsMovie()
                {
                    Cinema_Id = selectedCinemaId,
                    FromDate = DateTime.Parse(FromDate).ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Parse(ToDate).ToString("dd/MM/yyyy"),
                });
                Cinema = null;
                FromDate = null;
                ToDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanACinemaExecute()
        {
            return (Cinema != null && FromDate != null && ToDate != null) ? true : false;
        }

        private ICommand addActor;
        public ICommand AddActor
        {
            get
            {
                if (addActor == null)
                {
                    addActor = new RelayCommand(param => AddActorExecute(), param => CanAddActorExecute());
                }
                return addActor;
            }
        }

        private void AddActorExecute()
        {
            try
            {
                int selectedActorId = _dALActor.AllActors().ElementAt(SelectedActor).Id;
                var actor = _dALActor.GetActorById(selectedActorId);

                if (!_movieActors.Exists(a => a.Id == actor.Id))
                {
                    _movieActors.Add(actor);
                }
                Actor = null;
            }
           
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddActorExecute()
        {
            return Actor != null ? true : false;
        }

        private ICommand addMovie;
        public ICommand AddMovie
        {
            get
            {
                if (addMovie == null)
                {
                    addMovie = new RelayCommand(param => AddMovieExecute(), param => CanSaveChangesExecute());
                }
                return addMovie;
            }
        }

        private void AddMovieExecute()
        {
            if (!string.IsNullOrEmpty(Movie.Title))
            {
                Movie newMovie = new Movie
                {
                    Duration = Movie.Duration,
                    Title = Movie.Title,
                    Desctiprion = Movie.Desctiprion,
                    Distributor_Id = _dALDistributor.AllDistributors().ElementAt(SelectedDistributor).Id,
                    Director_Id = _dALDirector.AllDirectors().ElementAt(SelectedDirector).Id,
                    MetaScore = 0,
                    Festivals = new List<Festival>(),
                    Reviews = new List<Review>(),
                    Actors = new List<Actor>(),
                    Genres = new List<Genre>(),
                    ShowsMovies = new List<ShowsMovie>(),
                    Deleted = false
                };

                _dALMovie.AddMovie(newMovie);
                var lastAddedMovie = _dALMovie.AllMovies().LastOrDefault();

                _movieActors.ForEach(a =>
                {
                    _dALActor.AddActsProcedure(a.Id, lastAddedMovie.Id);
                });

                _movieGenres.ForEach(g =>
                {
                    _dALGenre.AddBelongToGenreProcedure(g.Id, lastAddedMovie.Id);
                });
                
                _movieFestivals.ForEach(f => {
                    _dALFestival.AddMovieFestivalProcedure(f.Id, lastAddedMovie.Id);
                });

                _movieFestivalAward.ForEach(a => {
                    a.MovieFestival_Movie_Id = lastAddedMovie.Id;
                    _dALAward.AddMovieFestivalAward(a);
                });
                        
                _showsMovie.ForEach(a =>
                {
                    a.Movie_Id = lastAddedMovie.Id;
                    _dALCinema.AddShowsMovie(a);
                });

                actor_window.Close();
            }         
        }

        private bool CanSaveChangesExecute()
        {
            if (Movie.Title != "" 
                && Movie.Duration != 0
                && Director != null 
                && Distributor != null
                && _movieActors.Count != 0
                && _movieGenres.Count != 0)
                return true;
            else
                return false;
        }

      
        private ICommand festivalsAndAwards;
        public ICommand FestivalsAndAwards
        {
            get
            {
                if (festivalsAndAwards == null)
                {
                    festivalsAndAwards = new RelayCommand(param => FestivalsAndAwardsExecute(), param => true);
                }
                return festivalsAndAwards;
            }
        }

        private void FestivalsAndAwardsExecute()
        {
            try
            {
                FestivalAndAwards window = new FestivalAndAwards();
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand addFestivalAwards;
        public ICommand AddFestivalAwards
        {
            get
            {
                if (addFestivalAwards == null)
                {
                    addFestivalAwards = new RelayCommand(param => AddFestivalAwardsExecute(), param => CanAddFestivalAwardsExecute());
                }
                return addFestivalAwards;
            }
        }

        private void AddFestivalAwardsExecute()
        {
            try
            {
                var festival = _dALFestival.AllFestivals().ElementAt(SelectedFestival);
                var awardsForFestival = _dALAward.AllAwardsForFestival(festival.Id);
                int awardId = 0;
               if (awardsForFestival.Count > 0)
                {
                     awardId = awardsForFestival.ElementAt(SelectedAward).Award_Id;
                }

                if (festival != null && awardId != 0 && !NotAwarded)
                {
                    MovieFestivalAward movieFestivalAward = new MovieFestivalAward()
                    {
                        AwardFestival_Festival_Id = festival.Id,
                        AwardFestival_Award_Id = awardId,
         
                    };
                    _movieFestivalAward.Add(movieFestivalAward);
                }
                else if (festival != null && NotAwarded)
                {
                    _movieFestivals.Add(festival);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddFestivalAwardsExecute()
        {
            return Festival != null ? true : false;
        }
    }
}
