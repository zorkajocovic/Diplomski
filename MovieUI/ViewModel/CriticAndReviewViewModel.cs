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
    class CriticAndReviewViewModel : TemplateForProperties
    {

        private Review _criticism = null;
        private Critic _critic = null;
        private List<Critic> _allCritics = new List<Critic>();
        private List<Review> _allCriticisms = new List<Review>();
        private DALCritic _dALCritic = new DALCritic();
        private DALReview _dALReview = new DALReview();

        public CriticAndReviewViewModel(CriticsAndReviews window)
        {
            _allCritics = _dALCritic.AllCritics();
            _allCriticisms = _dALReview.AllReviews();
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

        public Review Criticism
        {
            get { return _criticism; }
            set
            {
                _criticism = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Criticism"));
            }
        }

        public List<Critic> Critics
        {
            get { return _allCritics; }
            set
            {
                _allCritics = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Critics"));
            }
        }

        public List<Review> Criticisms
        {
            get { return _allCriticisms; }
            set
            {
                _allCriticisms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Criticisms"));
            }
        }

        private ICommand addCritic;
        public ICommand AddCritic
        {
            get
            {
                if (addCritic == null)
                {
                    addCritic = new RelayCommand(param => AddCriticExecute(), param => true);
                }
                return addCritic;
            }
        }

        private void AddCriticExecute()
        {
            AddCritic addCriticWindow = new AddCritic();
            addCriticWindow.ShowDialog();
            Critics = _dALCritic.AllCritics();        
        }

        private ICommand updateCritic;
        public ICommand UpdateCritic
        {
            get
            {
                if (updateCritic == null)
                {
                    updateCritic = new RelayCommand(param => UpdateCriticExecute(), param => CanUpdateOrDeleteCriticExecute());
                }
                return updateCritic;
            }
        }

        private void UpdateCriticExecute()
        {
            UpdateCritic update = new UpdateCritic(Critic);
            update.ShowDialog();                
        }

        private bool CanUpdateOrDeleteCriticExecute()
        {
            return Critic != null ? true : false;
        }

        private ICommand updateCriticism;
        public ICommand UpdateCriticism
        {
            get
            {
                if (updateCriticism == null)
                {
                    updateCriticism = new RelayCommand(param => UpdateCriticismExecute(), param => CanUpdateOrDeleteCriticismExecute());
                }
                return updateCriticism;
            }
        }

        private void UpdateCriticismExecute()
        {
            UpdateReview update = new UpdateReview(Criticism);
            update.ShowDialog();
            Criticisms = _dALReview.AllReviews();
        }

        private bool CanUpdateOrDeleteCriticismExecute()
        {
            return Criticism != null ? true : false;
        }

        private ICommand deleteCritic;
        public ICommand DeleteCritic
        {
            get
            {
                if (deleteCritic == null)
                {
                    deleteCritic = new RelayCommand(param => DeleteCriticExecute(), param => CanUpdateOrDeleteCriticExecute());
                }
                return deleteCritic;
            }
        }

        private void DeleteCriticExecute()
        {
            try
            {
                _dALCritic.DeleteCritic(Critic.Id);
                Critics = _dALCritic.AllCritics();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand deleteCriticism;
        public ICommand DeleteCriticism
        {
            get
            {
                if (deleteCriticism == null)
                {
                    deleteCriticism = new RelayCommand(param => DeleteCriticismExecute(), param => CanUpdateOrDeleteCriticismExecute());
                }
                return deleteCriticism;
            }
        }

        private void DeleteCriticismExecute()
        {
            try
            {
                _dALReview.DeleteReview(Criticism.Id);
                Criticisms = _dALReview.AllReviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
