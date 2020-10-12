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
    public class AddCinemaViewModel : TemplateForProperties
    {
        AddCinema _addCinemaWin;

        private Cinema _cinema = new Cinema();
        List<string> _allPlaces = new List<string>();
        List<Place> allPlaces = new List<Place>();
        private DALCinema _dALCinema = new DALCinema();
        private DALPlace _dALPlace = new DALPlace();
        string _place;
        int selectedPlace;

        public AddCinemaViewModel(AddCinema addCinemaWin)
        {
            _addCinemaWin = addCinemaWin;
            allPlaces = _dALPlace.AllPlaces();
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

        public int SelectedPlace
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlace"));
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

        private ICommand addCinema;
        public ICommand AddCinema
        {
            get
            {
                if (addCinema == null)
                {
                    addCinema = new RelayCommand(param => AddCinemaExecute(), param => CanAddCinemaExecute());
                }
                return addCinema;
            }
        }

        private void AddCinemaExecute()
        {
            try
            {
                Cinema newCinema = new Cinema()
                {
                    Name = Cinema.Name,
                    Place_Id = allPlaces.ElementAt(selectedPlace).Id,
                    Deleted = false
                };
                  
                _dALCinema.AddCinema(newCinema);
                _addCinemaWin.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddCinemaExecute()
        {
            return Place != null ? true : false;
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
    }
}
