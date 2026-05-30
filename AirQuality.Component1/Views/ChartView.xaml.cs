using System;
using System.Windows.Controls;

namespace AirQuality.Component1.Views
{
    public partial class ChartView : UserControl
    {
        public ChartView()
        {
            InitializeComponent();
            Unloaded += ChartView_Unloaded;
        }

        private void ChartView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var disposable = DataContext as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
