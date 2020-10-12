using DAL;
using DAL.DALHelper;
using MovieUI.Common;
using MovieUI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MovieUI.ViewModel
{
    public class AddFestivalViewModel : TemplateForProperties 
    {
        private AddFestival _addWin;
        private Festival _festival = new Festival();
        private readonly DALFestival _dALFestival = new DALFestival();
        private readonly DALPlace _dALPlace = new DALPlace();
        private List<string> _allPlaces = new List<string>();
        private List<Place> allPlaces = new List<Place>();
        string _place;
        string _date;
        int selectedPlace;

        public AddFestivalViewModel(AddFestival window)
        {
            _addWin = window;
            allPlaces = _dALPlace.AllPlaces();
        }

        public Festival Festival
        {
            get { return _festival; }
            set
            {
                _festival = value;
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

      
        private ICommand addFestival;
        public ICommand AddFestival
        {
            get
            {
                if (addFestival == null)
                {
                    addFestival = new RelayCommand(param => AddFestivalExecute(), param => CanAddFestivalExecute());
                }
                return addFestival;
            }
        }

        private void AddFestivalExecute()
        {
            try
            {
                Festival newFestival = new Festival
                {
                    Name = Festival.Name,
                    Place_Id = allPlaces.ElementAt(selectedPlace).Id,
                    Date = DateTime.Parse(Date).ToString("dd/MM/yyyy"),
                    Deleted = false
                };
                newFestival.Place = _dALPlace.GetPlaceById(newFestival.Place_Id);

                _dALFestival.AddFestival(newFestival);
                _addWin.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddFestivalExecute()
        {
            return Place != null && Date != null && Festival.Name != ""? true : false;
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
