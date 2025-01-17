using System.Windows.Controls;

using WPFNetFrame48.ViewModels;

namespace WPFNetFrame48.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
