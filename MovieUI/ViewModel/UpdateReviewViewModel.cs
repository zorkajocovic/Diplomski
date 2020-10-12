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
    public class UpdateReviewViewModel : TemplateForProperties
    {
        private readonly DALReview _dALReview = new DALReview();
        private readonly DALCritic _dALCritic = new DALCritic();
        private Review _currentCriticism;
        private UpdateReview _criticismWindow;
        private int selectedCritic;
        private string _critic;
        private List<string> _allCritics;
        private List<Critic> allCritics = new List<Critic>();
        private int selectedRate;

        public UpdateReviewViewModel(UpdateReview window, Review criticism)
        {
            _currentCriticism = criticism;
            _criticismWindow = window;
            allCritics = _dALCritic.AllCritics();
            _critic = _currentCriticism.Critic.Name + " " + _currentCriticism.Critic.Surname;
        }

        public Review Criticism
        {
            get { return _currentCriticism; }
            set
            {
                _currentCriticism = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Criticism"));
            }
        }

        public string Critic
        {
            get { return _critic; }
            set
            {
                _critic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Critic"));
            }
        }

        public int SelectedCritic
        {
            get { return selectedCritic; }
            set
            {
                selectedCritic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCritic"));
            }
        }

        public List<string> Critics
        {
            get { return AllCritics(); }
            set
            {
                _allCritics = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Critics"));
            }

        }

        private List<string> AllCritics()
        {
            _allCritics = new List<string>();
            var allCritics = _dALCritic.AllCritics();
            foreach (var critic in allCritics)
            {
                _allCritics.Add(critic.Name + " " + critic.Surname);
            }

            return _allCritics;
        }

        public void SendRate(object rate)
        {
            selectedRate = Convert.ToInt16(rate);
        }

        private ICommand updateCritic;
        public ICommand UpdateCriticism
        {
            get
            {
                if (updateCritic == null)
                {
                    updateCritic = new RelayCommand(param => UpdateCriticismExecute(), param => CanUpdateCriticimsExecute());
                }
                return updateCritic;
            }
        }

        private void UpdateCriticismExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Criticism.Text))
                {
                    _currentCriticism.Critic_Id = allCritics.ElementAt(SelectedCritic).Id;
                    _currentCriticism.Rate = selectedRate;
                    _dALReview.UpdateReview(_currentCriticism);
                    _criticismWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateCriticimsExecute()
        {
            return Criticism.Text != "" && Critic != null? true : false;
        }

    }
}
