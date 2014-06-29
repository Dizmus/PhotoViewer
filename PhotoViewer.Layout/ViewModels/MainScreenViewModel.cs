using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoViewer.Domain;
using PhotoViewer.Layout.Annotations;
using PhotoViewer.Layout.ViewModels.DomainModels;
using Xamarin.Forms;

namespace PhotoViewer.Layout.ViewModels {
    public class MainScreenViewModel : INotifyPropertyChanged {
        private ObservableCollection<PictureModel> pictures;
        private string mainText;
        private PictureModel selectedPicture;

        private NavigationPage view;

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

        public PictureModel SelectedPicture {
            get { return selectedPicture; }
            set {
                if (Equals(value, selectedPicture)) return;
                selectedPicture = value;
                OnPropertyChanged();
            }
        }

        public NavigationPage GetView() {
            return view ?? (view = new NavigationPage(new MainView() { BindingContext = this }) {
                Title = "Photo Viewer"
            });
        }

        public void AddNewPicture(PictureModel picture) {
            Pictures.Add(picture);

            OnPropertyChanged("Pictures");
            OnPropertyChanged("AnyPictureExists");
            OnPropertyChanged("NoPictureExists");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void MainScreenViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedPicture") {
                if (SelectedPicture != null) {
                    ViewImageViewModel viewImageViewModel = new ViewImageViewModel();
                    viewImageViewModel.LoadComments(SelectedPicture.Id);

                    view.Navigation.PushAsync(viewImageViewModel.GetView());
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}