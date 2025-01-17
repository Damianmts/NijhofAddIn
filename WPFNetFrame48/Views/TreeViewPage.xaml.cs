using System.ComponentModel;
using System.Windows;
using System;
using System.Windows.Controls;
using Syncfusion.SfSkinManager;
using WPFNetFrame48.ViewModels;

namespace WPFNetFrame48.Views
{
    public partial class TreeViewPage
    {
        private readonly TreeViewViewModel _viewModel;

        private void SfTreeView_QueryNodeSize(object sender, Syncfusion.UI.Xaml.TreeView.QueryNodeSizeEventArgs e)
        {
            // Controleer of het argument geldig is
            if (e == null) return;

            // Dynamisch de hoogte berekenen op basis van inhoud
            double autoFitHeight = e.GetAutoFitNodeHeight();

            // Stel een minimumhoogte in
            double minHeight = 30; // Pas dit aan naar je gewenste minimumhoogte
            e.Height = Math.Max(autoFitHeight, minHeight);

            e.Handled = true; // Voorkom dat de standaardhoogte wordt gebruikt
        }

        public string ThemeName = App.Current.Properties["Theme"]?.ToString() != null
            ? App.Current.Properties["Theme"]?.ToString()
            : "Windows11Light";

        public TreeViewPage(TreeViewViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel; // Alleen in runtime de viewModel vervangen
            }

            SfSkinManager.SetTheme(this, new Theme(ThemeName));

            // Voeg de Loaded event handler toe
            this.Loaded += TreeViewPage_Loaded;
        }

        private async void TreeViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Start het initialisatieproces nadat de pagina is geladen
            await _viewModel.InitializeViewAsync();
        }
    }
}
