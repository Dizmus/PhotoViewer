using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoViewer.DAO;
using PhotoViewer.Domain;
using PhotoViewer.Layout.Annotations;
using PhotoViewer.Layout.ViewModels.DomainModels;
using Xamarin.Forms;

namespace PhotoViewer.Layout.ViewModels {
    public class MainScreenViewModel : INotifyPropertyChanged {
        private readonly PictureRepository pictureRepository;
        private readonly CommentRepository commentRepository;
        private ObservableCollection<PictureModel> pictures;
        private NavigationPage view;

        public MainScreenViewModel(PictureRepository pictureRepository, CommentRepository commentRepository) {
            this.pictureRepository = pictureRepository;
            this.commentRepository = commentRepository;
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

        public NavigationPage GetView() {
            NavigationPage navigationPage = view ?? (view = new NavigationPage(new MainView(this)) {
                Title = "Photo Viewer"
            });

            navigationPage.Pushed += navigationPage_Pushed;
            navigationPage.PoppedToRoot += navigationPage_PoppedToRoot;

            return navigationPage;
        }

        void navigationPage_PoppedToRoot(object sender, NavigationEventArgs e) {
            int p = 0;
        }

        void navigationPage_Pushed(object sender, NavigationEventArgs e) {
            int p = 0;
        }

        public void Prepare() {
            PictureRepository repository = pictureRepository;
            IList<Picture> enumerable = repository.GetAllWithChildren().ToList();

            IList<PictureModel> pictureModels = enumerable.Select(picture => new PictureModel() {
                CommentsCount = (picture.Comments != null ? picture.Comments.Count() : 0).ToString(CultureInfo.InvariantCulture),
                ImageSource = ImageSource.FromFile(picture.ThumbnailImagePath),
                ImageDate = picture.DateAdded.ToString(CultureInfo.InvariantCulture),
                Id = picture.Id
            }).ToList();

            Pictures = new ObservableCollection<PictureModel>(pictureModels);

            OnPropertyChanged("AnyPictureExists");
            OnPropertyChanged("NoPictureExists");
        }

        public void AddNewPicture(string originalPath, string thumbnailPath, DateTime dateAdded) {
            Picture picture = new Picture {
                OriginalImagePath = originalPath,
                ThumbnailImagePath = thumbnailPath,
                DateAdded = dateAdded
            };


            pictureRepository.Insert(picture);

            ImageSource imageSource = ImageSource.FromFile(thumbnailPath);
            PictureModel model = new PictureModel {
                ImageSource = imageSource,
                CommentsCount = "0",
                ImageDate = dateAdded.ToString(CultureInfo.InvariantCulture),
                Id = picture.Id
            };

            Pictures.Add(model);

            OnPropertyChanged("Pictures");
            OnPropertyChanged("AnyPictureExists");
            OnPropertyChanged("NoPictureExists");
        }

        public void ListViewItemTapped(PictureModel pictureModel) {
            ViewImageViewModel viewImageViewModel = new ViewImageViewModel(pictureRepository, commentRepository);
            viewImageViewModel.Prepare(pictureModel.Id);

            view.PushAsync(viewImageViewModel.GetView());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}