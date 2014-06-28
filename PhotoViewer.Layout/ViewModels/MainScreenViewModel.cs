using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoViewer.Domain;
using PhotoViewer.Layout.Annotations;

namespace PhotoViewer.Layout.ViewModels {
    public class MainScreenViewModel : INotifyPropertyChanged {
        private ObservableCollection<PictureModel> pictures;
        private string mainText;

        public MainScreenViewModel() {
            PropertyChanged += MainScreenViewModel_PropertyChanged;
            pictures = new ObservableCollection<PictureModel>();
        }

        public ObservableCollection<PictureModel> Pictures {
            get { return pictures; }
            set {
                if (Equals(value, pictures)) return;
                pictures = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPictureCommand { get; set; }

        public bool AnyPictureExists {
            get {
                return Pictures != null && Pictures.Any();
            }
        }

        public bool NoPictureExists {
            get {
                return Pictures == null || !Pictures.Any();
            }
        }

        public void AddNewPicture(PictureModel picture) {
            Pictures.Add(picture);

            OnPropertyChanged("Pictures");
            OnPropertyChanged("AnyPictureExists");
            OnPropertyChanged("NoPictureExists");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void MainScreenViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "Pictures") {
               
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}