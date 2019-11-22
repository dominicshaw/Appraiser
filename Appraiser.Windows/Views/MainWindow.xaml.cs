using System;
using System.Windows;
using Appraiser.Windows.ViewModels;
using Serilog;

namespace Appraiser.Windows.Views
{
    public partial class MainWindow
    {
        private readonly ILogger _log;
        private readonly MainViewModel _model;

        public MainWindow(ILogger log, MainViewModel model)
        {
            InitializeComponent();

            _log = log;
            _model = model;

            DataContext = _model;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _model.Load();
            }
            catch (Exception ex)
            {
                _log.Error("{exception}", ex);
            }
        }
    }
}
