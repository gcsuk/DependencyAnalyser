using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DependencyAnalyser.Annotations;

namespace DependencyAnalyser.ViewModels
{
    public class ComponentManager : INotifyPropertyChanged
    {
        private string _newComponentName = string.Empty;
        private ObservableCollection<Models.Component> _components = new ObservableCollection<Models.Component>();

        public string NewComponentName
        {
            get { return _newComponentName; }
            set
            {
                _newComponentName = value;
                OnPropertyChanged(nameof(NewComponentName));
            }
        }

        public ObservableCollection<Models.Component> Components
        {
            get { return _components; }
            set
            {
                _components = value;
                OnPropertyChanged(nameof(Components));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
