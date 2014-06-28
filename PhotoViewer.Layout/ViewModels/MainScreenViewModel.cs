using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PhotoViewer.Domain;
using PhotoViewer.Layout.Annotations;

namespace PhotoViewer.Layout.ViewModels {
    public class MainScreenViewModel : INotifyPropertyChanged {
        private ObservableCollection<Picture> pictures;
        private string mainText;

        public ObservableCollection<Picture> Pictures {
            get { return pictures; }
            set {
                if (Equals(value, pictures)) return;
                pictures = value;
                OnPropertyChanged();
            }
        }

        public string MainText {
            get { return mainText; }
            set {
                if (value == mainText) return;
                mainText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}