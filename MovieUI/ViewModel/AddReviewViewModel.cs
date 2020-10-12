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
    public class AddReviewViewModel : TemplateForProperties
    {
        private Review _criticism = new Review();
        private DALReview _dALReview = new DALReview();
        private DALCritic _dALCritic = new DALCritic();
        private AddReview _window;
        private int selectedCritic = 0;
        private string _critic;
        private List<string> _allCritics;
        private List<Critic> allCritics = new List<Critic>();
        private int _currentMovieId;
        private int selectedRate;

        public AddReviewViewModel(AddReview window, int currentMovieId)
        {
            _window = window;
            allCritics = _dALCritic.AllCritics();
            _currentMovieId = currentMovieId;
        }

        public Review Criticism
        {
            get { return _criticism; }
            set
            {
                _criticism = value;
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

        private ICommand addReview;
        public ICommand AddReview
        {
            get
            {
                if (addReview == null)
                {
                    addReview = new RelayCommand(param => AddReviewExecute(), param => CanSaveChangesExecute());
                }
                return addReview;
            }
        }

        private void AddReviewExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Criticism.Text))
                {
                    Review newCriticism = new Review
                    {
                        Text = Criticism.Text,
                        Critic_Id = allCritics.ElementAt(selectedCritic).Id,
                        Movie_Id = _currentMovieId,
                        Rate = selectedRate,
                        Deleted = false
                    };

                    _dALReview.AddReview(newCriticism);
                    _window.Close();
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveChangesExecute()
        {
            return Criticism.Text != "" && Critic != null && selectedRate != 0 ? true : false;
        }

        private ICommand criticAndReviews;
        public ICommand CriticAndReviews
        {
            get
            {
                if (criticAndReviews == null)
                {
                    criticAndReviews = new RelayCommand(param => CriticAndReviewsExecute(), param => true);
                }
                return criticAndReviews;
            }
        }

        private void CriticAndReviewsExecute()
        {
            try
            {
                CriticsAndReviews addCriticismWindow = new CriticsAndReviews();
                addCriticismWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

      
    }
}
