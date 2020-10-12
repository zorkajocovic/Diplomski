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
    public class MainWindowViewModel : TemplateForProperties
    {
        private Movie _movie = null;
        private Genre _genre = null;
        private Actor _actor = null;
        private Director _director = null;
        private Distributor _distributor = null;
        private Cinema _cinema = null;
        private Place _place = null;
        private List<Director> _allDirectors;
        private List<Distributor> _allDistributors;
        private List<Actor> _allActors;
        private List<Movie> _allMovies;
        private List<Genre> _allGenres;
        private List<Cinema> _allCinemas;
        private List<Place> _allPlaces;
        private DALDirector _dALDirector = new DALDirector();
        private DALDistributor _dALDistributor = new DALDistributor();
        private DALActor _dALActor = new DALActor();
        private DALGenre _dALGenre = new DALGenre();
        private DALCinema _dALCinema = new DALCinema();
        private DALPlace _dALPlace = new DALPlace();
        private DALMovie _dALMovie = new DALMovie();
        private string searchText;

        public MainWindowViewModel()
        {
            Directors = _dALDirector.AllDirectors();
            Distributors = _dALDistributor.AllDistributors();
            Actors = _dALActor.AllActors();
            Genres = _dALGenre.AllGenres();
            Movies = _dALMovie.AllMovies();
            Cinemas = _dALCinema.AllCinemas();
            Places = _dALPlace.AllPlaces();
        }
        
        #region Binding Properties
        public Movie Movie
        {
            get { return _movie; }
            set
            {
                _movie = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Movie"));
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchText"));
            }
        }

        public Genre Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Genre"));
            }
        }

        public Actor Actor
        {
            get { return _actor; }
            set
            {
                _actor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Actor"));
            }
        }

        public Director Director
        {
            get { return _director; }
            set
            {
                _director = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Director"));
            }
        }

        public Distributor Distributor
        {
            get { return _distributor; }
            set
            {
                _distributor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distributor"));
            }
        }

        public Cinema Cinema
        {
            get { return _cinema; }
            set
            {
                _cinema = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cinema"));
            }
        }

        public Place Place
        {
            get { return _place; }
            set
            {
                _place = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Place"));
            }
        }

        public List<Actor> Actors
        {
            get { return _allActors; }
            set
            {
                _allActors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Actors"));
            }
        }

        public List<Place> Places
        {
            get { return _allPlaces; }
            set
            {
                _allPlaces = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Places"));
            }
        }

        public List<Cinema> Cinemas
        {
            get { return _allCinemas; }
            set
            {
                _allCinemas = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cinemas"));
            }
        }
        //public List<string> NameAndSurnameDir()
        //{
        //    List<string> rezisers = new List<string>();

        //    foreach (var dir in _sviFilmovi)
        //    {
        //        rezisers.Add(_dALHelper.ReziserByID(dir.REZISER_IDRezisera).ImeRezisera +
        //                                                                     " " +
        //            _dALHelper.ReziserByID(dir.REZISER_IDRezisera).PrezimeRezisera);
        //    }
        //    return rezisers;
        //}

        //public string NameDis()
        //{
        //    return _dALHelper.DistributerByID(Film.DISTRIBUTER_IDDistributera).NazivDistributera;
        //}

        public List<Distributor> Distributors
        {
            get { return _allDistributors; }
            set
            {
                _allDistributors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distributors"));
            }
        }

        public List<Director> Directors
        {
            get { return _allDirectors; }
            set
            {
                _allDirectors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Directors"));
            }
        }

        public List<Movie> Movies
        {
            get { return _allMovies; }
            set
            {
                _allMovies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Movies"));
            }
        }

        public List<Genre> Genres
        {
            get { return _allGenres; }
            set
            {
                _allGenres = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Genres"));
            }
        }
        #endregion

        private ICommand addActor;
        public ICommand AddActor
        {
            get
            {
                if (addActor == null)
                {
                    addActor = new RelayCommand(param => AddActorExecute(), param => true);
                }
                return addActor;
            }
        }

        private void AddActorExecute()
        {
            try
            {
                AddActor addActorWindow = new AddActor();
                addActorWindow.ShowDialog();
                Actors = _dALActor.AllActors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand details;
        public ICommand MovieDetails
        {
            get
            {
                if (details == null)
                {
                    details = new RelayCommand(param => AddMovieDetailsExecute(), param => CanMovieDetailsExecute());
                }
                return details;
            }
        }

        private void AddMovieDetailsExecute()
        {
            try
            {
                MovieDetails details = new MovieDetails(Movie);
                details.ShowDialog();
      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanMovieDetailsExecute()
        {
            return Movie == null ? false : true;
        }

        private ICommand addDirector;
        public ICommand AddDirector
        {
            get
            {
                if (addDirector == null)
                {
                    addDirector = new RelayCommand(param => AddDirectorExecute(), param => true);
                }
                return addDirector;
            }
        }

        private void AddDirectorExecute()
        {
            try
            {
                AddDirector addDirectorWindow = new AddDirector();
                addDirectorWindow.ShowDialog();
                Directors = _dALDirector.AllDirectors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand addDistributor;
        public ICommand AddDistributor
        {
            get
            {
                if (addDistributor == null)
                {
                    addDistributor = new RelayCommand(param => AddDistributorExecute(), param => true);
                }
                return addDistributor;
            }
        }

        private void AddDistributorExecute()
        {
            try
            {
                AddDistributor addWindow = new AddDistributor();
                addWindow.ShowDialog();
                Distributors = _dALDistributor.AllDistributors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand addPlace;
        public ICommand AddPlace
        {
            get
            {
                if (addPlace == null)
                {
                    addPlace = new RelayCommand(param => AddPlaceExecute(), param => true);
                }
                return addPlace;
            }
        }

        private void AddPlaceExecute()
        {
            try
            {
                AddPlace addWindow = new AddPlace();
                addWindow.ShowDialog();
                Places = _dALPlace.AllPlaces();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private ICommand addCinema;
        public ICommand AddCinema
        {
            get
            {
                if (addCinema == null)
                {
                    addCinema = new RelayCommand(param => AddCinemaExecute(), param => true);
                }
                return addCinema;
            }
        }

        private void AddCinemaExecute()
        {
            try
            {
                AddCinema addCinemaWindow = new AddCinema();
                addCinemaWindow.ShowDialog();
                Cinemas = _dALCinema.AllCinemas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand addGenre;
        public ICommand AddGenre
        {
            get
            {
                if (addGenre == null)
                {
                    addGenre = new RelayCommand(param => AddGenreExecute(), param => true);
                }
                return addGenre;
            }
        }

        private void AddGenreExecute()
        {
            try
            {
                AddGenre addGenreWindow = new AddGenre();
                addGenreWindow.ShowDialog();
                Genres = _dALGenre.AllGenres();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand addMovie;
        public ICommand AddMovie
        {
            get
            {
                if (addMovie == null)
                {
                    addMovie = new RelayCommand(param => AddMovieExecute(), param => true);
                }
                return addMovie;
            }
        }

        private void AddMovieExecute()
        {
            try
            {
                AddMovie addGenreWindow = new AddMovie();
                addGenreWindow.ShowDialog();
                Movies = _dALMovie.AllMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand deleteActor;
        public ICommand DeleteActor
        {
            get
            {
                if (deleteActor == null)
                {
                    deleteActor = new RelayCommand(param => DeleteActorExecute(), param => CanDeleteActorExecute());
                }
                return deleteActor;
            }
        }

        private void DeleteActorExecute()
        {
            try
            {
                Actor.Deleted = true;
                _dALActor.DeleteActor(Actor.Id);
                Actors = _dALActor.AllActors();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeleteActorExecute()
        {
            return Actor != null ? true : false;
        }

        private ICommand deleteMovie;
        public ICommand DeleteMovie
        {
            get
            {
                if (deleteMovie == null)
                {
                    deleteMovie = new RelayCommand(param => DeleteMovieExecute(), param => CanDeleteMovieExecute());
                }
                return deleteMovie;
            }
        }

        private void DeleteMovieExecute()
        {
            try
            {
                if (Movie == null)
                    MessageBox.Show("Morate selektovati film za brisanje!");

                bool okBrisanje = _dALMovie.DeleteMovie(Movie.Id);
                if (okBrisanje == false)
                    MessageBox.Show("Nije moguce izbrisati film!");
                else
                    Movies = _dALMovie.AllMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeleteMovieExecute()
        {
            return Movie != null ? true : false;
        }

        private ICommand deleteDirector;
        public ICommand DeleteDirector
        {
            get
            {
                if (deleteDirector == null)
                {
                    deleteDirector = new RelayCommand(param => DeleteDirectorExecute(), param => CanDeleteDirectorExecute());
                }
                return deleteDirector;
            }
        }

        private void DeleteDirectorExecute()
        {
            try
            {
                if (Director == null)
                    MessageBox.Show("Morate selektovati rezisera za brisanje!");
                else
                {
                    bool okBrisanje = _dALDirector.DeleteDirector(Director.Id);
                    if (okBrisanje == false)
                        MessageBox.Show("Nije moguce izbrisati rezisera!");
                    else
                    {
                        Directors = _dALDirector.AllDirectors();
                        Movies = _dALMovie.AllMovies();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeleteDirectorExecute()
        {
            return Director != null ? true : false;
        }

        private ICommand deleteDistributor;
        public ICommand DeleteDistributor
        {
            get
            {
                if (deleteDistributor == null)
                {
                    deleteDistributor = new RelayCommand(param => DeleteDistributorExecute(), param => CanDeleteDistributorExecute());
                }
                return deleteDistributor;
            }
        }

        private void DeleteDistributorExecute()
        {
            try
            {
                if (Distributor == null)
                    MessageBox.Show("Morate selektovati distributer za brisanje!");

                bool okBrisanje = _dALDistributor.DeleteDistributor(Distributor.Id);
                if (okBrisanje == false)
                    MessageBox.Show("Nije moguce izbrisati distributera filma!");
                else
                {
                    Distributors = _dALDistributor.AllDistributors();
                    Movies = _dALMovie.AllMovies();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeleteDistributorExecute()
        {
            return Distributor != null ? true : false;
        }

        private ICommand deleteGenre;
        public ICommand DeleteGenre
        {
            get
            {
                if (deleteGenre == null)
                {
                    deleteGenre = new RelayCommand(param => DeleteGenreExecute(), param => CanDeleteGenreExecute());
                }
                return deleteGenre;
            }
        }

        private void DeleteGenreExecute()
        {
            try
            {
                if (Genre == null)
                    MessageBox.Show("Morate selektovati zanr za brisanje!");
                else
                {
                    bool okBrisanje = _dALGenre.DeleteGenre(Genre.Id);
                    if (okBrisanje == false)
                        MessageBox.Show("Nije moguce izbrisati zanr filma!");
                    else
                        Genres = _dALGenre.AllGenres();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeleteGenreExecute()
        {
            return Genre != null ? true : false;
        }

        private ICommand deletePlace;
        public ICommand DeletePlace
        {
            get
            {
                if (deletePlace == null)
                {
                    deletePlace = new RelayCommand(param => DeletePlaceExecute(), param => CanDeletePlaceExecute());
                }
                return deletePlace;
            }
        }

        private void DeletePlaceExecute()
        {
            try
            {
                _dALPlace.DeletePlace(Place.Id);
                Places = _dALPlace.AllPlaces();
                Cinemas = _dALCinema.AllCinemas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeletePlaceExecute()
        {
            return Place != null ? true : false;
        }

        private ICommand deleteCinema;
        public ICommand DeleteCinema
        {
            get
            {
                if (deleteCinema == null)
                {
                    deleteCinema = new RelayCommand(param => DeleteCinemaExecute(), param => CanDeleteCinemaExecute());
                }
                return deleteCinema;
            }
        }

        private void DeleteCinemaExecute()
        {
            try
            {
                _dALCinema.DeleteCinema(Cinema.Id);
                Cinemas = _dALCinema.AllCinemas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDeleteCinemaExecute()
        {
            return Cinema != null ? true : false;
        }

        private ICommand updateGenre;
        public ICommand UpdateGenre
        {
            get
            {
                if (updateGenre == null)
                {
                    updateGenre = new RelayCommand(param => UpdateGenreExecute(), param => CanUpdateGenreExecute());
                }
                return updateGenre;
            }
        }

        private void UpdateGenreExecute()
        {
            UpdateGenre update = new UpdateGenre(Genre);
            update.ShowDialog();             
        }

        private bool CanUpdateGenreExecute()
        {
            return Genre != null ? true : false;
        }

        private ICommand updatePlace;
        public ICommand UpdatePlace
        {
            get
            {
                if (updatePlace == null)
                {
                    updatePlace = new RelayCommand(param => UpdatePlaceExecute(), param => CanUpdatePlaceExecute());
                }
                return updatePlace;
            }
        }

        private void UpdatePlaceExecute()
        {
            UpdatePlace update = new UpdatePlace(Place);
            update.ShowDialog();     
        }

        private bool CanUpdatePlaceExecute()
        {
            return Place != null ? true : false;
        }

        private ICommand updateActor;
        public ICommand UpdateActor
        {
            get
            {
                if (updateActor == null)
                {
                    updateActor = new RelayCommand(param => UpdateActorExecute(), param => CanUpdateActorExecute());
                }
                return updateActor;
            }
        }

        private void UpdateActorExecute()
        {
            UpdateActor update = new UpdateActor(Actor);
            update.ShowDialog();        
        }

        private bool CanUpdateActorExecute()
        {
            return Actor != null ? true : false;
        }

        private ICommand updateDirector;
        public ICommand UpdateDirector
        {
            get
            {
                if (updateDirector == null)
                {
                    updateDirector = new RelayCommand(param => UpdateDirectorExecute(), param => CanUpdateDirectorExecute());
                }
                return updateDirector;
            }
        }

        private void UpdateDirectorExecute()
        {
            UpdateDirector update = new UpdateDirector(Director);
            update.ShowDialog();
        }

        private bool CanUpdateDirectorExecute()
        {
            return Director != null ? true : false;
        }

        private ICommand updateDistributor;
        public ICommand UpdateDistributor
        {
            get
            {
                if (updateDistributor == null)
                {
                    updateDistributor = new RelayCommand(param => UpdateDistributorExecute(), param => CanUpdateDistributorExecute());
                }
                return updateDistributor;
            }
        }

        private void UpdateDistributorExecute()
        {         
            UpdateDistributor update = new UpdateDistributor(Distributor);
            update.ShowDialog();
        }

        private bool CanUpdateDistributorExecute()
        {
            return Distributor != null ? true : false;
        }

        public void AllMovies()
        {
            Movies = _dALMovie.AllMovies();
        }

        private ICommand updateMovie;
        public ICommand UpdateMovie
        {
            get
            {
                if (updateMovie == null)
                {
                    updateMovie = new RelayCommand(param => UpdateMovieExecute(), param => CanUpdateMovieExecute());
                }
                return updateMovie;
            }
        }

        private void UpdateMovieExecute()
        {      
            UpdateMovie update = new UpdateMovie(Movie);
            update.ShowDialog();
            Movies = _dALMovie.AllMovies();            
        }

        private bool CanUpdateMovieExecute()
        {
            return Movie != null ? true : false;
        }

        private ICommand updateCinema;
        public ICommand UpdateCinema
        {
            get
            {
                if (updateCinema == null)
                {
                    updateCinema = new RelayCommand(param => UpdateCinemaExecute(), param => CanUpdateCinemaExecute());
                }
                return updateCinema;
            }
        }

        private void UpdateCinemaExecute()
        {         
            UpdateCinema update = new UpdateCinema(Cinema);
            update.ShowDialog();
            Cinemas = _dALCinema.AllCinemas();                       
        }

        private bool CanUpdateCinemaExecute()
        {
            return Cinema != null ? true : false;
        }

        private ICommand searchMovies;
        public ICommand SearchMovies
        {
            get
            {
                if (searchMovies == null)
                {
                    searchMovies = new RelayCommand(param => SearchMoviesExecute(), param => true);
                }
                return searchMovies;
            }
        }

        private void SearchMoviesExecute()
        {
           Movies = _dALMovie.SearchMovies(SearchText);
            if (Movies == null)
                MessageBox.Show("No results!");
            else
                SearchText = string.Empty;
        }



    }
}
