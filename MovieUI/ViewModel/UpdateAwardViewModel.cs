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
    public class UpdateAwardViewModel : TemplateForProperties
    {
        UpdateAward window;
        private readonly DALAward _dALHelper = new DALAward();
        private Award _currentAward;
       
        public UpdateAwardViewModel(UpdateAward window, Award award)
        {
            this.window = window;
            _currentAward = award;
        }

        public Award Award
        {
            get { return _currentAward; }
            set
            {
                _currentAward = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Award"));
            }
        }
        
        private ICommand updateAward;
        public ICommand UpdateAward
        {
            get
            {
                if (updateAward == null)
                {
                    updateAward = new RelayCommand(param => UpdateAwardExecute(), param => CanUpdateAwardExecute());
                }
                return updateAward;
            }
        }

        private void UpdateAwardExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Award.Name))
                {
                    _dALHelper.UpdateAward(_currentAward);
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateAwardExecute()
        {
            return (Award.Name != "") ? true : false;
        }
    }
}
