using System.ComponentModel;
using System.Runtime.CompilerServices;
using PhotoViewer.Layout.Annotations;
using Xamarin.Forms;

namespace PhotoViewer.Layout.ViewModels.DomainModels {
    public class PictureModel : INotifyPropertyChanged {
        private string commentsCount;
        private ImageSource imageSource;
        private string imageDate;
        public int Id { get; set; }

        public ImageSource ImageSource {
            get { return imageSource; }
            set {
                if (Equals(value, imageSource)) return;
                imageSource = value;
                OnPropertyChanged();
            }
        }

        public string CommentsCount {
            get { return commentsCount; }
            set {
                if (value == commentsCount) return;
                commentsCount = value;
                OnPropertyChanged();
            }
        }

        public string ImageDate {
            get { return imageDate; }
            set {
                if (value == imageDate) return;
                imageDate = value;
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