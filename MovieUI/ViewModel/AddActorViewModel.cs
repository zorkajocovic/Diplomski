using DAL;
using DAL.DALHelper;
using MovieUI.Common;
using MovieUI.View;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MovieUI.ViewModel
{
    public class AddActorViewModel : TemplateForProperties
    {
        private Actor _actor = new Actor();
        private DALActor _dALHelper = new DALActor();
        private AddActor actor_window;

        public AddActorViewModel(AddActor window)
        {
            actor_window = window;
        }

        public Actor Actor
        {
            get { return _actor; }
            set
            {
                _actor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Actor"));
            }
        }

        private ICommand addActor;
        public ICommand AddActor
        {
            get
            {
                if (addActor == null)
                {
                    addActor = new RelayCommand(param => AddActorExecute(), param => CanSaveChangesExecute());
                }
                return addActor;
            }
        }

        private void AddActorExecute()
        {
            try
            {
                if(!string.IsNullOrEmpty(Actor.Name) || !string.IsNullOrEmpty(Actor.Surname))
                {
                    Actor newActor = new Actor
                    {
                        Name = Actor.Name,
                        Surname = Actor.Surname,
                        Deleted = false
                    };
                    _dALHelper.AddActor(newActor);
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
            return (Actor.Name != "" && Actor.Surname != "") ? true : false;
        }      
    }
}
