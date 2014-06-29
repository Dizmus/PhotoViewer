using System;
using Xamarin.Forms;

namespace PhotoViewer.Layout.ViewModels {
    public class PictureModel {
        public ImageSource ImageSource { get; set; }

        public string CommentsCount { get; set; }

        public string ImageDate { get; set; }
    }
}