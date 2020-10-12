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
    public class AddDirectorViewModel : TemplateForProperties
    {
        private Director _director = new Director();
        private DALDirector _dALHelper = new DALDirector();
        private AddDirector actor_window;

        public AddDirectorViewModel(AddDirector window)
        {
            actor_window = window;
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
        
        private ICommand addDirector;
        public ICommand AddDirector
        {
            get
            {
                if (addDirector == null)
                {
                    addDirector = new RelayCommand(param => AddDirectorExecute(), param => CanSaveChangesExecute());
                }
                return addDirector;
            }
        }

        private void AddDirectorExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Director.Name) || !string.IsNullOrEmpty(Director.Surname))
                {
                    Director newDirector = new Director
                    {
                        Name = Director.Name,
                        Surname = Director.Surname,
                        Deleted = false
                    };
                    _dALHelper.AddDirector(newDirector);
                    actor_window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveChangesExecute()
        {
            return (Director.Name != "" && Director.Surname != "") ? true : false;
        }
    }
}
