using Xamarin.Forms;

namespace PhotoViewer.Layout.ViewModels.DomainModels {
    public class PictureModel {
        public int Id { get; set; }
        public ImageSource ImageSource { get; set; }

        public string CommentsCount { get; set; }

        public string ImageDate { get; set; }
    }
}