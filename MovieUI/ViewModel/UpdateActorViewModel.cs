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
    class UpdateActorViewModel : TemplateForProperties
    {
        private DALActor _dALHelper = new DALActor();
        private Actor _actor;
        UpdateActor zanrWindow;

        public UpdateActorViewModel(UpdateActor zanrWin, Actor selectedActor)
        {
            zanrWindow = zanrWin;
            _actor = selectedActor;
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
        
        private ICommand updateActor;
        public ICommand UpdateActor
        {
            get
            {
                if (updateActor == null)
                {
                    updateActor = new RelayCommand(param => UpdateActorExecute(), param => CanUpdateActorExecute());
                }
                return updateActor;
            }
        }

        private void UpdateActorExecute()
        {
            if (!string.IsNullOrEmpty(Actor.Name) && !string.IsNullOrEmpty(Actor.Surname))
            {
                _dALHelper.UpdateActor(_actor);
                zanrWindow.Close();
            }
        }

        private bool CanUpdateActorExecute()
        {
           return (Actor.Name != "" && Actor.Surname != "") ? true : false;
        }
    }
}
