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
    class UpdateDirectorViewModel : TemplateForProperties
    {
        private DALDirector _dALHelper = new DALDirector();
        private Director _director;
        UpdateDirector zanrWindow;

        public UpdateDirectorViewModel(UpdateDirector zanrWin, Director selectedRez)
        {
            zanrWindow = zanrWin;
            _director = selectedRez;
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
            try
            {
                if (!string.IsNullOrEmpty(Director.Name) && !string.IsNullOrEmpty(Director.Surname))
                {
                    _dALHelper.UpdateDirector(_director);
                    zanrWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateDirectorExecute()
        {
            return (Director.Name != "" && Director.Surname != "") ? true : false;
        }

    }
}
