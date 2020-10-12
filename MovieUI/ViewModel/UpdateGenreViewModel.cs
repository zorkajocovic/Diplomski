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
    class UpdateGenreViewModel : TemplateForProperties
    {
        private DALGenre _dALHelper = new DALGenre();
        private Genre _genre;
        UpdateGenre zanrWindow;

        public UpdateGenreViewModel(UpdateGenre genreWin, Genre selectedGenre)
        {
            zanrWindow = genreWin;
            _genre = selectedGenre;
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
            try
            {
                if (!string.IsNullOrEmpty(Genre.Name))
                {
                    _dALHelper.UpdateGenre(_genre);
                    zanrWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateGenreExecute()
        {
            return Genre.Name != "" ? true : false;
        }
    }
}
