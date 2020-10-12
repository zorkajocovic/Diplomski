
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
   
    public class AddAwardViewModel : TemplateForProperties
    {
        private Award _award = new Award();
        private readonly DALAward _dALAward = new DALAward();
        private readonly DALFestival _dALFestival= new DALFestival();
        private AddAward award_window;
        private List<string> _allFestivals = new List<string>();
        private List<Festival> allFestivals = new List<Festival>();
        private int _selectedFestival;
        private string _festival;

        public AddAwardViewModel(AddAward window)
        {
            award_window = window;
            allFestivals = _dALFestival.AllFestivals();
        }

        public Award Award
        {
            get { return _award; }
            set
            {
                _award = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Award"));
            }
        }

        public string Festival
        {
            get { return _festival; }
            set
            {
                _festival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festival"));
            }
        }

        public List<string> Festivals
        {
            get { return AllFestivals(); }
            set
            {
                _allFestivals = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festivals"));
            }
        }

        public int SelectedFestival
        {
            get { return _selectedFestival; }
            set
            {
                _selectedFestival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFestival"));
            }
        }

        private List<string> AllFestivals()
        {
            _allFestivals = new List<string>();
            var allFestivals= _dALFestival.AllFestivals();
            foreach (var festival in allFestivals)
            {
                _allFestivals.Add(festival.Name);
            }
            return _allFestivals;
        }

        private ICommand addAward;
        public ICommand AddAward
        {
            get
            {
                if (addAward == null)
                {
                    addAward = new RelayCommand(param => AddAwardExecute(), param => CanSaveChangesExecute());
                }
                return addAward;
            }
        }

        private void AddAwardExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Award.Name))
                {
                    Award award = new Award()
                    {
                        Name = Award.Name,
                        Deleted = false
                    };
                    _dALAward.AddAward(award);

                    var lastedAddedAward = _dALAward.AllAwards().LastOrDefault();

                    AwardFestival awardFestival = new AwardFestival()
                    {
                        Award_Id = lastedAddedAward.Id,
                        Festival_Id = _dALFestival.AllFestivals().ElementAt(SelectedFestival).Id
                    };

                    _dALAward.AddAwardFestival(awardFestival);

                    award_window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveChangesExecute()
        {
            return (Award.Name != "") ? true : false;
        }

    }
}
