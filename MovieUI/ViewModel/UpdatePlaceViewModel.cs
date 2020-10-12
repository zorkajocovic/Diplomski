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
    public class UpdatePlaceViewModel : TemplateForProperties
    {
        private DALPlace _dALHelper = new DALPlace();
        private Place _place;
        UpdatePlace placeWindow;

        public UpdatePlaceViewModel(UpdatePlace window, Place place)
        {
            placeWindow = window;
            _place = place;
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
            try
            {
                if (!string.IsNullOrEmpty(Place.Name))
                {
                    _dALHelper.UpdatePlace(_place);
                    placeWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdatePlaceExecute()
        {
            return Place.Name != "" ? true : false;
        }
    }
}
