using System;
using System.IO;
using System.Text;
using Android.Graphics;
using Android.Provider;
using Java.IO;
using Java.Net;
using PhotoViewer.DAO;
using PhotoViewer.Layout;
using PhotoViewer.Layout.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using File = Java.IO.File;
using IOException = System.IO.IOException;
using Path = System.IO.Path;
using Picture = PhotoViewer.Domain.Picture;

namespace PhotoViewer.Contollers {
    public class MainController {
        private const int THUMBNALE_NAME_LENGTH = 8;
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private PictureRepository pictureRepository;
        private CommentRepository commentRepository;

        public delegate void CommandEvent();

        public CommandEvent AddNewPictureEvent;
        private MainScreenViewModel mainScreenViewModel;
        private string thumbnailsLocation;

        public MainController() {
            thumbnailsLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "thumbs");
            if (!Directory.Exists(thumbnailsLocation)) {
                Directory.CreateDirectory(thumbnailsLocation);
            }
        }

        public Page LoadMainPage() {
            mainScreenViewModel = new MainScreenViewModel { AddPictureCommand = new Command(OnAddNewPictureCommand) };

            ContentPage mainScreen = new MainScreen();
            mainScreen.BindingContext = mainScreenViewModel;
            return mainScreen;
        }

        private PictureRepository GetPictureRepository() {
            if (pictureRepository == null) {
                pictureRepository = TinyIoCContainer.Current.Resolve<PictureRepository>();
            }

            return pictureRepository;
        }

        public void AddNewPictureToGalery(string path) {
            PictureRepository repository = GetPictureRepository();

            string thumbnailPath = SaveThumbnailForImage(path);

            Picture picture = new Picture();
            picture.OriginalImagePath = path;
            picture.ThumbnailImagePath = thumbnailPath;
            repository.Insert(picture);

            ImageSource imageSource = ImageSource.FromFile(thumbnailPath);
            PictureModel model = new PictureModel { ImageSource = imageSource };
            mainScreenViewModel.AddNewPicture(model);
        }

        private CommentRepository GetCommentRepository() {
            if (commentRepository == null) {
                commentRepository = TinyIoCContainer.Current.Resolve<CommentRepository>();
            }

            return commentRepository;
        }

        private void OnAddNewPictureCommand() {
            if (AddNewPictureEvent != null) {
                AddNewPictureEvent();
            }
        }

        private string SaveThumbnailForImage(string path) {
            string resultFilePath = string.Empty;
            Bitmap loadImageFromUrl = LoadImageFromUrl(path);
            MemoryStream bytes = new MemoryStream();
            loadImageFromUrl.Compress(Bitmap.CompressFormat.Jpeg, 40, bytes);

            string randomFileName = GetRandomFileName(THUMBNALE_NAME_LENGTH) + ".jpg";
            resultFilePath = Path.Combine(thumbnailsLocation, randomFileName);
            File f = new File(resultFilePath);
            f.CreateNewFile();
            //write the bytes in file
            FileOutputStream fo = new FileOutputStream(f);
            fo.Write(bytes.ToArray());

            // remember close de FileOutput
            fo.Close();

            return resultFilePath;
        }

        public static Bitmap LoadImageFromUrl(String url) {

            Bitmap bm;
            try {

                bm = BitmapFactory.DecodeFile(url);

            } catch (IOException e) {
                return null;
            }

            Bitmap loadImageFromUrl = Bitmap.CreateScaledBitmap(bm, 100, 100, true);
            bm.Dispose();
            return loadImageFromUrl;
        }

        private string GetRandomFileName(int size) {

            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}