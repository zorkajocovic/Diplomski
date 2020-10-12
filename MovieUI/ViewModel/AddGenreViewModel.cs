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
    public class AddGenreViewModel : TemplateForProperties
    {
        private Genre _genre = new Genre();
        private DALGenre _dALHelper = new DALGenre();
        private AddGenre actor_window;

        public AddGenreViewModel(AddGenre window)
        {
            actor_window = window;
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
        
        private ICommand addGenre;
        public ICommand AddGenre
        {
            get
            {
                if (addGenre == null)
                {
                    addGenre = new RelayCommand(param => AddZanrExecute(), param => CanSaveChangesExecute());
                }
                return addGenre;
            }
        }

        private void AddZanrExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Genre.Name))
                {
                    Genre newGenre = new Genre
                    {
                        Name = Genre.Name,
                        Deleted = false
                    };
                    _dALHelper.AddGenre(newGenre);
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
            return Genre.Name != "" ? true : false;
        }
    }
}
