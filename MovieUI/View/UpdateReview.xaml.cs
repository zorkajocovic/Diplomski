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
    /// Interaction logic for UpdateCriticism.xaml
    /// </summary>
    public partial class UpdateReview : MetroWindow
    {
        public UpdateReview(Review criticism)
        {
            InitializeComponent();
            DataContext = new UpdateReviewViewModel(this, criticism);
        }

        private void radio1_Checked(object sender, RoutedEventArgs e)
        {
            var dataContext = (UpdateReviewViewModel)DataContext;
            dataContext.SendRate(((RadioButton)sender).Content);
        }

        private void radio2_Checked(object sender, RoutedEventArgs e)
        {
            var dataContext = (UpdateReviewViewModel)DataContext;
            dataContext.SendRate(((RadioButton)sender).Content);
        }

        private void radio3_Checked(object sender, RoutedEventArgs e)
        {
            var dataContext = (UpdateReviewViewModel)DataContext;
            dataContext.SendRate(((RadioButton)sender).Content);
        }

        private void radio4_Checked(object sender, RoutedEventArgs e)
        {
            var dataContext = (UpdateReviewViewModel)DataContext;
            dataContext.SendRate(((RadioButton)sender).Content);
        }

        private void radio5_Checked(object sender, RoutedEventArgs e)
        {
            var dataContext = (UpdateReviewViewModel)DataContext;
            dataContext.SendRate(((RadioButton)sender).Content);
        }
    }
}
