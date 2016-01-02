using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DependencyAnalyser.Annotations;

namespace DependencyAnalyser.ViewModels
{
    public class MainWindow : INotifyPropertyChanged
    {
        private bool _isLoading = true;
        private string _targetDirectory = string.Empty;
        private Models.Component _selectedComponent;

        public MainWindow()
        {
            Packages = new ObservableCollection<Models.Package>();
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string TargetDirectory
        {
            get { return _targetDirectory; }
            set
            {
                _targetDirectory = value;
                OnPropertyChanged(nameof(TargetDirectory));
            }
        }

        public ObservableCollection<Models.Component> Components { get; set; }

        public Models.Component SelectedComponent
        {
            get { return _selectedComponent; }
            set
            {
                _selectedComponent = value;
                OnPropertyChanged(nameof(SelectedComponent));
            }
        }

        public ObservableCollection<Models.Package> Packages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
