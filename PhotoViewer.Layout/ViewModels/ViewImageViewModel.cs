using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoViewer.DAO;
using PhotoViewer.Domain;
using PhotoViewer.Layout.Annotations;
using PhotoViewer.Layout.ViewModels.DomainModels;
using TinyIoC;
using Xamarin.Forms;

namespace PhotoViewer.Layout.ViewModels {
    public class ViewImageViewModel : INotifyPropertyChanged {
        private PictureRepository pictureRepository;
        private CommentRepository commentRepository;
        private Picture currentPicture;
        private Page view;

        public ViewImageViewModel() {
            pictureRepository = TinyIoCContainer.Current.Resolve<PictureRepository>();
            commentRepository = TinyIoCContainer.Current.Resolve<CommentRepository>();

            AddCommentCommand = new Command(onAddCommentCommand);
        }

        public ImageSource ImageSource { get; set; }

        public string NewCommentText { get; set; }

        public ICommand AddCommentCommand { get; set; }

        public ObservableCollection<CommentModel> Comments { get; set; }

        public Page GetView() {
            return view ?? (view = new NavigationPage(new ViewImageView() { BindingContext = this }) {
                Title = "Photo Details"
            });
        }

        public void LoadComments(int pictureId) {
            currentPicture = pictureRepository.GetById(pictureId);

            IList<CommentModel> commentModels = new List<CommentModel>();

            foreach (Comment comment in currentPicture.Comments) {
                commentModels.Add(new CommentModel() {
                    CommentDate = comment.PostDate.ToString(DateTimeFormatInfo.InvariantInfo),
                    CommentText = comment.Text
                });
            }

            Comments = new ObservableCollection<CommentModel>(commentModels);
        }

        private void onAddCommentCommand() {
            Comment comment = new Comment() {
                Text = NewCommentText,
                PostDate = DateTime.Now
            };

            currentPicture.Comments.Add(comment);
            pictureRepository.Update(currentPicture);
            Comments.Add(new CommentModel() {
                CommentDate = comment.PostDate.ToString(DateTimeFormatInfo.InvariantInfo),
                CommentText = comment.Text
            });

            OnPropertyChanged("Comments");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}