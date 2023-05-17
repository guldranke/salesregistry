using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views;

/// <summary>
/// Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : UserControl {
    public Statistics() {
        InitializeComponent();
    }

    private void GetSalesClick(object sender, System.Windows.RoutedEventArgs e) {
        StatisticsViewModel context = (StatisticsViewModel)DataContext;

        context.GetGetSales();
    }
}
