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
    public class UpdateCinemaViewModel : TemplateForProperties
    {
        UpdateCinema window;
        private DALCinema _dALCinema = new DALCinema();
        private DALPlace _dALPlace = new DALPlace();
        private Cinema _currentCinema;
        private List<string> _allPlaces = new List<string>();
        private List<Place> allPlaces = new List<Place>();
        private string _place;
        private int selectedPlace;

        public UpdateCinemaViewModel(UpdateCinema updateWindow, Cinema cinema)
        {
            window = updateWindow;
            _currentCinema = cinema;
            _place = _currentCinema.Place.Name;
            allPlaces = _dALPlace.AllPlaces();
        }

        public Cinema Cinema
        {
            get { return _currentCinema; }
            set
            {
                _currentCinema = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cinema"));
            }
        }

        public string Place
        {
            get { return _place; }
            set
            {
                _place = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Place"));
            }
        }

        public List<string> Places
        {
            get { return AllPlaces(); }
            set
            {
                _allPlaces = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Places"));
            }
        }

        private List<string> AllPlaces()
        {
            _allPlaces = new List<string>();
            var allPlaces = _dALPlace.AllPlaces();
            foreach (var place in allPlaces)
            {
                _allPlaces.Add(place.Name);
            }

            return _allPlaces;
        }

        public int SelectedPlace
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlace"));
            }
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
            try
            {
                if (!string.IsNullOrEmpty(Cinema.Name) && Place != null)
                {
                    _currentCinema.Place_Id = allPlaces.ElementAt(selectedPlace).Id;
                    _dALCinema.UpdateCinema(_currentCinema);
                    window.Close();
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateCinemaExecute()
        {
            return (Cinema.Name != "" && Place != null) ? true : false;
        }

    }
}
