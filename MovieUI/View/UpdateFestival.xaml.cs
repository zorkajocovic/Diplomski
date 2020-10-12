using DAL;
using MahApps.Metro.Controls;
using MovieUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MovieUI.View
{
    /// <summary>
    /// Interaction logic for UpdateFestival.xaml
    /// </summary>
    public partial class UpdateFestival : MetroWindow
    {
        public UpdateFestival(Festival festival)
        {
            InitializeComponent();
            DataContext = new UpdateFestivalViewModel(this, festival);
        }
    }
}
