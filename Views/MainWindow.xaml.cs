using System.Windows;
using System.Windows.Controls;
using SmartTaskManager.ViewModels;

namespace SmartTaskManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

        }

        // WPF ComboBox with static string items can't bind SelectedValue directly,
        // so we forward the selection to the ViewModel here.
        private void OnStatusFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainViewModel vm && sender is ComboBox cb
                && cb.SelectedItem is ComboBoxItem item)
            {
                vm.SelectedStatus = item.Content.ToString();
            }
        }

        private void OnPriorityFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainViewModel vm && sender is ComboBox cb
                && cb.SelectedItem is ComboBoxItem item)
            {
                vm.SelectedPriority = item.Content.ToString();
            }
        }
    }
}