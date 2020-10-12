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
    public class AddDistributorViewModel : TemplateForProperties
    {
        private Distributor _distributor = new Distributor();
        private DALDistributor _dALHelper = new DALDistributor();
        private AddDistributor actor_window;

        public AddDistributorViewModel(AddDistributor window)
        {
            actor_window = window;
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
        
        private ICommand addDistributor;
        public ICommand AddDistributor
        {
            get
            {
                if (addDistributor == null)
                {
                    addDistributor = new RelayCommand(param => AddDistributerExecute(), param => CanSaveChangesExecute());
                }
                return addDistributor;
            }
        }

        private void AddDistributerExecute()
        {
            try
            {
                if (!string.IsNullOrEmpty(Distributor.Name))
                {
                    Distributor newDistributor = new Distributor
                    {
                        Name = Distributor.Name,
                        Deleted = false
                    };
                    _dALHelper.AddDistributor(newDistributor);
                    actor_window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveChangesExecute()
        {
            return Distributor.Name != "" ? true : false;
        }


    }
}
