using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            var components = _componentsService.GetList();

            if (components == null)
            {
                System.Windows.MessageBox.Show("An error occured retrieving the component list. Restart the app and try again.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _vm.Components = new ObservableCollection<Models.Component>(components);

                DataContext = _vm;
            }

            _vm.IsLoading = false;
        }

        private async void Load_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.IsLoading = true;

            _vm.Packages.Clear();

            try
            {
                var packages = await Task.Run(() => _analysisService.Analyse(_vm.TargetDirectory));

                packages.ToList().ForEach(p => _vm.Packages.Add(p));
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

            _vm.IsLoading = false;
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

        private async void Upload_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.IsLoading = true;

            _vm.SelectedComponent.Packages = _vm.Packages.ToList();

            var uploadTask = await _analysisService.Upload(_vm.SelectedComponent);

            if (uploadTask)
            {
                System.Windows.MessageBox.Show("Done!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Error. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _vm.IsLoading = false;
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
