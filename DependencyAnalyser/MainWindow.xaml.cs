using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace DependencyAnalyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ViewModels.MainWindow _vm;
        private readonly Services.AnalysisService _analysisService;
        private readonly Services.ComponentsService _componentsService;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new ViewModels.MainWindow();
            _analysisService = new Services.AnalysisService();
            _componentsService = new Services.ComponentsService();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _vm.Components = new ObservableCollection<Models.Component>(_componentsService.GetList());

            DataContext = _vm;
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.Packages.Clear();

            try
            {
                _analysisService.Analyse(_vm.TargetDirectory).ToList().ForEach(p => _vm.Packages.Add(p));
            }
            catch (ArgumentException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Invalid Target Directory", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "File Not Found", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "File Not Found", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _vm.TargetDirectory = dialog.SelectedPath;
            }
        }

        private void Upload_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ManageComponents_OnClick(object sender, RoutedEventArgs e)
        {
            var componentsWindow = new ComponentManager();

            componentsWindow.ShowDialog();

            _vm.Components.Clear();

            foreach (var component in componentsWindow.Components)
            {
                _vm.Components.Add(component);
            }
        }
    }
}
