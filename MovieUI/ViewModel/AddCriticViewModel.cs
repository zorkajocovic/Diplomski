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
    public class AddCriticViewModel : TemplateForProperties
    {

        private Critic _critic = new Critic();
        private DALCritic _dALCritic = new DALCritic();
        private AddCritic critic_window;
        private int selectedScore;
        private int []scores = new int[5];

        public AddCriticViewModel(AddCritic window)
        {
            critic_window = window;
            for(int i=0; i<5; i++)
            {
                scores[i] = i + 1;
            }
        }

        public int SelectedScore
        {
            get { return selectedScore; }
            set
            {
                selectedScore = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScore"));
            }
        }

        public int []Scores
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
            get { return _critic; }
            set
            {
                _critic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Critic"));
            }
        }

        private ICommand addCritic;
        public ICommand AddCritic
        {
            get
            {
                if (addCritic == null)
                {
                    addCritic = new RelayCommand(param => AddCriticExecute(), param => CanCriticExecute());
                }
                return addCritic;
            }
        }

        private void AddCriticExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Critic.Name) || !string.IsNullOrEmpty(Critic.Surname))
                {
                    Critic newCritic = new Critic
                    {
                        Name = Critic.Name,
                        Surname = Critic.Surname,
                        ReliabilityScore = SelectedScore,
                        Deleted = false
                    };
                    _dALCritic.AddCritic(newCritic);
                    critic_window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanCriticExecute()
        {
            return (Critic.Name != "" && Critic.Surname != "" && SelectedScore != 0) ? true : false;
        }

    }
}
