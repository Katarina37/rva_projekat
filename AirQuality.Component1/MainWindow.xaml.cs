using AirQuality.Component1.ViewModels;
using AirQuality.Component1.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirQuality.Component1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new StationView();
        }

        private void BtnStations_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StationView();
        }

        private void BtnReadings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReadingView();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content is StationView stationView)
            {
                var vm = stationView.DataContext as StationViewModel;
                vm?.UndoCommand.Execute(null);
            }
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content is StationView stationView)
            {
                var vm = stationView.DataContext as StationViewModel;
                vm?.RedoCommand.Execute(null);
            }
        }
    }
}
