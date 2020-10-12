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
    public class UpdateCriticViewModel : TemplateForProperties
    {
        private readonly DALCritic _dALHelper = new DALCritic();
        private Critic _currentCritic;
        private UpdateCritic _criticWindow;
        private int? selectedScore;
        private int[] scores = new int[5];

        public UpdateCriticViewModel(UpdateCritic window, Critic critic)
        {
            _criticWindow = window;
            _currentCritic = critic;

            for (int i = 0; i < 5; i++)
            {
                scores[i] = i + 1;
            }
            selectedScore = _currentCritic.ReliabilityScore;
        }

        public int? SelectedScore
        {
            get { return selectedScore; }
            set
            {
                selectedScore = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScore"));
            }
        }

        public int[] Scores
        {
            get { return scores; }
            set
            {
                scores = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Scores"));
            }
        }

        public Critic Critic
        {
            get { return _currentCritic; }
            set
            {
                _currentCritic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Critic"));
            }
        }
        
        private ICommand updateCritic;
        public ICommand UpdateCritic
        {
            get
            {
                if (updateCritic == null)
                {
                    updateCritic = new RelayCommand(param => UpdateCriticExecute(), param => CanUpdateCriticExecute());
                }
                return updateCritic;
            }
        }

        private void UpdateCriticExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Critic.Name) && !string.IsNullOrEmpty(Critic.Surname))
                {
                    _dALHelper.UpdateCritic(_currentCritic);
                    _criticWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateCriticExecute()
        {
            return Critic.Name != "" 
                && Critic.Surname != ""
                && Critic.ReliabilityScore != null ? true : false;
        }
    }
}
