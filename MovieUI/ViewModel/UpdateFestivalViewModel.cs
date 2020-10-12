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
    public class UpdateFestivalViewModel : TemplateForProperties
    {
        UpdateFestival window;
        private readonly DALFestival _dALFestival = new DALFestival();
        private readonly DALPlace _dALPlace = new DALPlace();
        private Festival _currentFestival;
        private List<string> _allPlaces = new List<string>();
        private List<Place> allPlaces = new List<Place>();
        string _place;
        string _date;
        int selectedPlace;

        public UpdateFestivalViewModel(UpdateFestival window, Festival festival)
        {
            this.window = window;
            _currentFestival = festival;
            allPlaces = _dALPlace.AllPlaces();
        }

        public Festival Festival
        {
            get { return _currentFestival; }
            set
            {
                _currentFestival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festival"));
            }
        }

        public int SelectedPlaceIndex
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlaceIndex"));
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

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Date"));
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

        private ICommand updateFestival;
        public ICommand UpdateFestival
        {
            get
            {
                if (updateFestival == null)
                {
                    updateFestival = new RelayCommand(param => UpdateFestivalExecute(), param => CanUpdateFestivalExecute());
                }
                return updateFestival;
            }
        }

        private void UpdateFestivalExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Festival.Name) && Place != null)
                {
                    _currentFestival.Name = Festival.Name;
                    _currentFestival.Date = DateTime.Parse(Date).ToString("dd/MM/yyyy");
                    _currentFestival.Place_Id = allPlaces.ElementAt(selectedPlace).Id;

                    _dALFestival.UpdateFestival(_currentFestival);
                    window.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateFestivalExecute()
        {
            return Festival.Name != "" && Place != null && Date != null ? true : false;
        }
    }
}
