using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DependencyAnalyser
{
    /// <summary>
    /// Interaction logic for ComponentManager.xaml
    /// </summary>
    public partial class ComponentManager
    {
        private readonly Services.ComponentsService _componentsService;
        private readonly ViewModels.ComponentManager _vm;

        public IEnumerable<Models.Component> Components => _vm.Components;

        public ComponentManager()
        {
            _componentsService = new Services.ComponentsService(Properties.Settings.Default.ApiUrl);

            _vm = new ViewModels.ComponentManager();

            InitializeComponent();
        }

        private void ComponentManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            _vm.Components = new ObservableCollection<Models.Component>(_componentsService.GetList());

            DataContext = _vm;
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            var newComponent = _componentsService.Add(new Models.Component {Name = _vm.NewComponentName});

            _vm.Components.Add(newComponent);
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.Components.ToList().ForEach(c =>
            {
                _componentsService.Update(c);
            });
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
            {
                _componentsService.Delete((Models.Component)components.SelectedItem);

                _vm.Components.Remove((Models.Component)components.SelectedItem);
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}