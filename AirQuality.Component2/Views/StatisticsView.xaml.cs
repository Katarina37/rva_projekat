using AirQuality.Component2.ViewModels;
using System.Windows.Controls;

namespace AirQuality.Component2.Views
{
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContext = new StatisticsViewModel();
        }
    }
}
