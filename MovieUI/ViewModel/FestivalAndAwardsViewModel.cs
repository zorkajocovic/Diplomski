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
    public class FestivalAndAwardsViewModel : TemplateForProperties
    {
        private Festival _festival = null;
        private Award _award = null;
        private List<Festival> _allFestivals = new List<Festival>();
        private List<Award> _allAwards = new List<Award>();
        private List<AwardFestival> _allAwardFestival = new List<AwardFestival>();
        private List<string> _festivals = new List<string>();
        private List<string> _awards = new List<string>();
        private readonly DALAward _dALAward = new DALAward();
        private readonly DALFestival _dALFestival = new DALFestival();
        private string festivalForAward;
        private string awardForFestival;
        private int selectedFestival;
        private int selectedAward;

        public FestivalAndAwardsViewModel(FestivalAndAwards window)
        {
            _allFestivals = _dALFestival.AllFestivals();
            _allAwards = _dALAward.AllAwards();
            _allAwardFestival = _dALAward.AllAwardFestival();
        }

        public Festival Festival
        {
            get { return _festival; }
            set
            {
                _festival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festival"));
            }
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

        public int SelectedFestival
        {
            get { return selectedFestival; }
            set
            {
                selectedFestival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFestival"));
            }
        }

        public int SelectedAward
        {
            get { return selectedAward; }
            set
            {
                selectedAward = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAward"));
            }
        }

        public string FestivalForAward
        {
            get { return festivalForAward; }
            set
            {
                festivalForAward = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FestivalForAward"));
            }
        }

        public string AwardForFestival
        {
            get { return awardForFestival; }
            set
            {
                awardForFestival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AwardForFestival"));
            }
        }

        public List<Festival> Festivals
        {
            get { return _allFestivals; }
            set
            {
                _allFestivals = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Festivals"));
            }
        }

        public List<AwardFestival> AwardFestival
        {
            get { return _allAwardFestival; }
            set
            {
                _allAwardFestival = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AwardFestival"));
            }
        }

        public List<Award> Awards
        {
            get { return _allAwards; }
            set
            {
                _allAwards = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Awards"));
            }
        }

        public List<string> AllFestivals
        {
            get { return GetAllFestivals(); }
            set
            {
                _festivals = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllFestivals"));
            }
        }

        public List<string> AllAwards
        {
            get { return GetAllAwards(); }
            set
            {
                _awards = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllAwards"));
            }
        }

        private List<string> GetAllAwards()
        {
            _awards = new List<string>();
            var allAwards = _allAwards;
            foreach (var award in allAwards)
            {
                _awards.Add(award.Name);
            }

            return _awards;
        }

        private List<string> GetAllFestivals()
        {
            _festivals = new List<string>();
            var allFestivals = Festivals;
            foreach (var festival in allFestivals)
            {
                _festivals.Add(festival.Name);
            }

            return _festivals;
        }

        private ICommand addFestivals;
        public ICommand AddFestival
        {
            get
            {
                if (addFestivals == null)
                {
                    addFestivals = new RelayCommand(param => AddFestivalsExecute(), param => true);
                }
                return addFestivals;
            }
        }

        private void AddFestivalsExecute()
        {
            AddFestival addFestivalWindow = new AddFestival();
            addFestivalWindow.ShowDialog();
            Festivals = _dALFestival.AllFestivals();
        }

        private ICommand addAward;
        public ICommand AddAward
        {
            get
            {
                if (addAward == null)
                {
                    addAward = new RelayCommand(param => AddAwardExecute(), param => true);
                }
                return addAward;
            }
        }

        private void AddAwardExecute()
        {
            AddAward addAwardWindow = new AddAward();
            addAwardWindow.ShowDialog();
            Awards = _dALAward.AllAwards();
        }

        private ICommand updateFestival;
        public ICommand UpdateFestival
        {
            get
            {
                if (updateFestival == null)
                {
                    updateFestival = new RelayCommand(param => UpdateFestivalExecute(), param => CanUpdateOrDeleteFestivalExecute());
                }
                return updateFestival;
            }
        }

        private void UpdateFestivalExecute()
        {
            UpdateFestival update = new UpdateFestival(Festival);
            update.ShowDialog();
            Festivals = _dALFestival.AllFestivals();
        }

        private bool CanUpdateOrDeleteFestivalExecute()
        {
            return Festival != null ? true : false;
        }

        private ICommand updateAward;
        public ICommand UpdateAward
        {
            get
            {
                if (updateAward == null)
                {
                    updateAward = new RelayCommand(param => UpdateAwardExecute(), param => CanUpdateOrDeleteAwardExecute());
                }
                return updateAward;
            }
        }

        private void UpdateAwardExecute()
        {
            UpdateAward update = new UpdateAward(Award);
            update.ShowDialog();
            Awards = _dALAward.AllAwards();
        }

        private bool CanUpdateOrDeleteAwardExecute()
        {
            return Award != null ? true : false;
        }

        private ICommand deleteFestival;
        public ICommand DeleteFestival
        {
            get
            {
                if (deleteFestival == null)
                {
                    deleteFestival = new RelayCommand(param => DeleteFestivalExecute(), param => CanUpdateOrDeleteFestivalExecute());
                }
                return deleteFestival;
            }
        }

        private void DeleteFestivalExecute()
        {
            try
            {
                _dALFestival.DeleteFestival(Festival.Id);
                Festivals = _dALFestival.AllFestivals();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand deleteAward;
        public ICommand DeleteAward
        {
            get
            {
                if (deleteAward == null)
                {
                    deleteAward = new RelayCommand(param => DeleteAwardExecute(), param => CanUpdateOrDeleteAwardExecute());
                }
                return deleteAward;
            }
        }

        private void DeleteAwardExecute()
        {
            try
            {
                _dALAward.DeleteAward(Award.Id);
                Awards = _dALAward.AllAwards();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand addAwardFestival;
        public ICommand AddAwardFestival
        {
            get
            {
                if (addAwardFestival == null)
                {
                    addAwardFestival = new RelayCommand(param => AddAwardFestivalExecute(), param => CanSaveChangesExecute());
                }
                return addAwardFestival;
            }
        }

        private void AddAwardFestivalExecute()
        {
            try
            {
                AwardFestival awardFestival = new AwardFestival()
                {
                    Award_Id = _allAwards.ElementAt(SelectedAward).Id,
                    Festival_Id = _allFestivals.ElementAt(SelectedFestival).Id
                };

                if (!_allAwardFestival.Exists(a => a.Award_Id == awardFestival.Award_Id 
                                            && a.Festival_Id == awardFestival.Festival_Id)){
                    _dALAward.AddAwardFestival(awardFestival);
                    AwardFestival = _dALAward.AllAwardFestival();
                }
                else
                {
                    MessageBox.Show("Izabrana nagrada se vec dodjeljuje na festivalu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveChangesExecute()
        {
            return FestivalForAward != null && AwardForFestival != null ? true : false;
        }
    }
}
