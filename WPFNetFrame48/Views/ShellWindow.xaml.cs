using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.NavigationDrawer;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using WPFNetFrame48.Contracts.Views;
using WPFNetFrame48.ViewModels;

namespace WPFNetFrame48.Views
{
    public partial class ShellWindow : ChromelessWindow, IShellWindow
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString();
        public ShellViewModel _ShellViewModel;

        public ShellWindow(ShellViewModel viewModel)
        {
            try
            {
                // Test de resource voor InitializeComponent
                var goBack = Properties.Resources.ShellGoBackButton;
                System.Diagnostics.Debug.WriteLine($"Voor InitializeComponent - Resource waarde: {goBack}");

                InitializeComponent();

                // Test de resource na InitializeComponent
                goBack = Properties.Resources.ShellGoBackButton;
                System.Diagnostics.Debug.WriteLine($"Na InitializeComponent - Resource waarde: {goBack}");

                DataContext = viewModel;
                _ShellViewModel = viewModel;

                themeName = themeName == null ? "Windows11Light" : themeName;
                SfSkinManager.SetTheme(this, new Syncfusion.SfSkinManager.Theme(themeName));
                if (this is ShellWindow)
                {
                    if ((this as ShellWindow) != null && ((this as ShellWindow).Content is SfNavigationDrawer) && ((this as ShellWindow).Content as SfNavigationDrawer) != null && (((this as ShellWindow).Content as SfNavigationDrawer).ContentView) as Frame != null)
                    {
                        SfSkinManager.SetTheme((((this as ShellWindow).Content as SfNavigationDrawer).ContentView) as Frame, new Syncfusion.SfSkinManager.Theme(themeName));
                        SfSkinManager.SetTheme((this as ShellWindow).Content as SfNavigationDrawer, new Syncfusion.SfSkinManager.Theme(themeName));
                    }
                }
                if (themeName == "MaterialDark" || themeName == "Office2019HighContrast" || themeName == "MaterialDarkBlue" || themeName == "Office2019Black" || themeName == "Windows11Dark")
                {
                    _ShellViewModel.UpdateFillColor(new SolidColorBrush(Colors.White));
                }
                else
                {
                    _ShellViewModel.UpdateFillColor(new SolidColorBrush(Colors.Black));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
    public class MyObservableCollection : ObservableCollection<object> { }
}