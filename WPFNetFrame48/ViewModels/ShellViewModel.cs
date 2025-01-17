using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using Syncfusion.UI.Xaml.NavigationDrawer;
using Syncfusion.Windows.Shared;

using WPFNetFrame48.Contracts.Services;
using WPFNetFrame48.Helpers;
using WPFNetFrame48.Properties;

namespace WPFNetFrame48.ViewModels
{
    public class ShellViewModel : Observable
    {
        private ICommand _optionsMenuItemInvokedCommand;
        private readonly INavigationService _navigationService;
        private object _selectedMenuItem;
        private RelayCommand _goBackCommand;
        private ICommand _menuItemInvokedCommand;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;
        private bool _isInitialNavigation = true;

        public object SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                var newValue = value as NavigationPaneItem;
                if (newValue == null && value is FrameworkElement fe)
                {
                    newValue = fe.DataContext as NavigationPaneItem;
                }

                if (!ReferenceEquals(newValue, _selectedMenuItem))
                {
                    Set(ref _selectedMenuItem, newValue);
                    if (!_isInitialNavigation && newValue?.TargetType != null)
                    {
                        NavigateTo(newValue.TargetType);
                    }
                }
            }
        }

        public void UpdateFillColor(SolidColorBrush FillColor)
        {
            foreach (var item in MenuItems)
            {
                (item as NavigationPaneItem).Path.Fill = FillColor;
            }
            SettingsIconColor = FillColor;
        }

        private SolidColorBrush settingsIconColor;

        public SolidColorBrush SettingsIconColor  // Let op: naam aangepast van Setttings naar Settings
        {
            get { return settingsIconColor; }
            set
            {
                settingsIconColor = value;
                OnPropertyChanged(nameof(SettingsIconColor));
            }
        }

        public ObservableCollection<NavigationPaneItem> MenuItems { get; set; } = new ObservableCollection<NavigationPaneItem>()
        {
            new NavigationPaneItem() {
                Label = Resources.ShellMainPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M28.414 4H7V44H39V14.586ZM29 7.414 35.586 14H29ZM9 42V6H27V16H37V42Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(MainViewModel)
            },
            new NavigationPaneItem() {
                Label = Resources.ShellTreeViewPage,
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M42.5 6C43.8807 6 45 4.88071 45 3.5V2.5C45 1.11929 43.8807 0 42.5 0H13.5C12.1193 0 11 1.11929 11 2.5V3.5C11 4.88071 12.1193 6 13.5 6H42.5ZM43 3.5C43 3.77614 42.7761 4 42.5 4H13.5C13.2238 4 13 3.77614 13 3.5V2.5C13 2.22386 13.2238 2 13.5 2H42.5C42.7761 2 43 2.22386 43 2.5V3.5ZM42.5 17C43.8807 17 45 15.8807 45 14.5V13.5C45 12.1193 43.8807 11 42.5 11H18.5C17.1193 11 16 12.1193 16 13.5V14.5C16 15.8807 17.1193 17 18.5 17H42.5ZM43 14.5C43 14.7761 42.7761 15 42.5 15H18.5C18.2238 15 18 14.7761 18 14.5V13.5C18 13.2239 18.2238 13 18.5 13H42.5C42.7761 13 43 13.2239 43 13.5V14.5ZM45 27.5C45 28.8807 43.8807 30 42.5 30H18.5C17.1193 30 16 28.8807 16 27.5V26.5C16 25.1193 17.1193 24 18.5 24H42.5C43.8807 24 45 25.1193 45 26.5V27.5ZM42.5 28C42.7761 28 43 27.7761 43 27.5V26.5C43 26.2239 42.7761 26 42.5 26H18.5C18.2238 26 18 26.2239 18 26.5V27.5C18 27.7761 18.2238 28 18.5 28H42.5ZM42.5 42C43.8807 42 45 40.8807 45 39.5V38.5C45 37.1193 43.8807 36 42.5 36H13.5C12.1193 36 11 37.1193 11 38.5V39.5C11 40.8807 12.1193 42 13.5 42H42.5ZM43 39.5C43 39.7761 42.7761 40 42.5 40H13.5C13.2238 40 13 39.7761 13 39.5V38.5C13 38.2239 13.2238 38 13.5 38H42.5C42.7761 38 43 38.2239 43 38.5V39.5ZM4.83204 5.25192C4.43621 5.84566 3.56376 5.84566 3.16794 5.25192L1.03645 2.0547C0.593416 1.39015 1.06981 0.5 1.8685 0.5L6.13147 0.5C6.93016 0.5 7.40656 1.39015 6.96352 2.0547L4.83204 5.25192ZM11.7519 15.3321C12.3456 14.9362 12.3456 14.0638 11.7519 13.6679L8.55469 11.5365C7.89013 11.0934 6.99999 11.5698 6.99999 12.3685L6.99998 16.6315C6.99998 17.4302 7.89013 17.9066 8.55468 17.4635L11.7519 15.3321ZM11.7519 26.6679C12.3456 27.0638 12.3456 27.9362 11.7519 28.3321L8.55468 30.4635C7.89013 30.9066 6.99998 30.4302 6.99998 29.6315L6.99999 25.3685C6.99999 24.5698 7.89013 24.0934 8.55469 24.5365L11.7519 26.6679ZM3.16794 41.2519C3.56376 41.8457 4.43621 41.8457 4.83204 41.2519L6.96352 38.0547C7.40656 37.3901 6.93016 36.5 6.13147 36.5H1.8685C1.06981 36.5 0.593416 37.3901 1.03645 38.0547L3.16794 41.2519Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(TreeViewViewModel)
            },
        };

        public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SettingsIconColor = new SolidColorBrush(Colors.Black);
        }

        private void OnLoaded()
        {
            _navigationService.Navigated += OnNavigated;
        }

        private void OnUnloaded()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack()
            => _navigationService.CanGoBack;

        private void OnGoBack()
            => _navigationService.GoBack();

        private void NavigateTo(Type targetViewModel)
        {
            if (targetViewModel != null)
            {
                _navigationService.NavigateTo(targetViewModel.FullName);
            }
        }

        private void OnNavigated(object sender, string viewModelName)
        {
            var item = MenuItems
                        .OfType<NavigationPaneItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetType?.FullName);

            if (item != null && !ReferenceEquals(item, _selectedMenuItem))
            {
                SelectedMenuItem = item;
            }

            _isInitialNavigation = false;
            GoBackCommand.OnCanExecuteChanged();
        }

        private void OnOptionsMenuItemInvoked()
            => NavigateTo(typeof(SettingsViewModel));
    }

    public class NavigationPaneItem
    {
        public string Label { get; set; }
        public Path Path { get; set; }
        public Type TargetType { get; set; }
    }
}