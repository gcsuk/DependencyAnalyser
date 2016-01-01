using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DependencyAnalyser.Annotations;

namespace DependencyAnalyser.ViewModels
{
    public class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            Packages = new ObservableCollection<Models.Package>();
        }

        private string _targetDirectory = string.Empty;

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

        public ObservableCollection<Models.Package> Packages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
