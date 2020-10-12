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
    public class AddPlaceViewModel : TemplateForProperties
    {
        private Place _place = new Place();
        private DALPlace _dALHelper = new DALPlace();
        private AddPlace window;

        public AddPlaceViewModel(AddPlace window)
        {
            this.window = window;
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

        private ICommand addPlace;
        public ICommand AddPlace
        {
            get
            {
                if (addPlace == null)
                {
                    addPlace = new RelayCommand(param => AddPlaceExecute(), param => CanSaveChangesExecute());
                }
                return addPlace;
            }
        }

        private void AddPlaceExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Place.Name))
                {
                    Place newPlace = new Place
                    {
                        Name = Place.Name,
                        Deleted = false
                    };
                    _dALHelper.AddPlace(newPlace);
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveChangesExecute()
        {
            return (Place.Name != "") ? true : false;
        }
    }
}
