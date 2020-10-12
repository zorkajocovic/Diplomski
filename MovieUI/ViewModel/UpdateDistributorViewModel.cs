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
    class UpdateDistributorViewModel : TemplateForProperties
    {
        private DALDistributor _dALHelper = new DALDistributor();
        private Distributor _distributor;
        UpdateDistributor zanrWindow;

        public UpdateDistributorViewModel(UpdateDistributor zanrWin, Distributor distributor)
        {
            zanrWindow = zanrWin;
            _distributor = distributor;
        }

        public Distributor Distributor
        {
            get { return _distributor; }
            set
            {
                _distributor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distributor"));
            }
        }
        
        private ICommand updateDistributor;
        public ICommand UpdateDistributor
        {
            get
            {
                if (updateDistributor == null)
                {
                    updateDistributor = new RelayCommand(param => UpdateDistributorExecute(), param => CanUpdateDistributorExecute());
                }
                return updateDistributor;
            }
        }

        private void UpdateDistributorExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Distributor.Name))
                {
                    _dALHelper.UpdateDistributor(_distributor);
                    zanrWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanUpdateDistributorExecute()
        {
            return Distributor.Name != "" ? true : false;
        }

    }
}
