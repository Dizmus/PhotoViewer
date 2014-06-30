using System;
using System.Collections;
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
    public class ViewImageViewModel : INotifyPropertyChanged {
        private PictureRepository pictureRepository;
        private CommentRepository commentRepository;
        private Picture currentPicture;
        private Page view;
        private string newCommentText;

        public ViewImageViewModel(PictureRepository repository, CommentRepository commentRepository) {
            this.pictureRepository = repository;
            this.commentRepository = commentRepository;

            AddCommentCommand = new Command(onAddCommentCommand);
        }

        public ImageSource ImageSource { get; set; }

        public string NewCommentText {
            get { return newCommentText; }
            set {
                if (value == newCommentText) return;
                newCommentText = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommentCommand { get; set; }

        public ObservableCollection<CommentModel> Comments { get; set; }

        public bool NoPictureExists {
            get {
                return Comments == null || !Comments.Any();
            }
        }

        public Page GetView() {
            return view ?? (view = new NavigationPage(new ViewImageView(this)) {
                Title = "Photo Details"
            });
        }

        public void Prepare(int pictureId) {
            currentPicture = pictureRepository.GetById(pictureId);

            IList<CommentModel> commentModels = new List<CommentModel>();

            foreach (Comment comment in currentPicture.Comments) {
                commentModels.Add(new CommentModel() {
                    CommentDate = comment.PostDate.ToString(DateTimeFormatInfo.InvariantInfo),
                    CommentText = comment.Text
                });
            }

            Comments = new ObservableCollection<CommentModel>(commentModels);

            ImageSource = ImageSource.FromFile(currentPicture.OriginalImagePath);
        }

        private void onAddCommentCommand() {
            Comment comment = new Comment() {
                Text = NewCommentText,
                PostDate = DateTime.Now,
                Picture = currentPicture,
                PictureId = currentPicture.Id
            };

            currentPicture.Comments.Add(comment);

            commentRepository.Insert(comment);

            CommentModel commentModel = new CommentModel() {
                CommentDate = comment.PostDate.ToString(DateTimeFormatInfo.InvariantInfo),
                CommentText = comment.Text
            };
            Comments.Add(commentModel);

            OnPropertyChanged("Comments");
            OnPropertyChanged("NoPictureExists");
            NewCommentText = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}