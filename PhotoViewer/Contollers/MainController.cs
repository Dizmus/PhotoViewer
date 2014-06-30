using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.Provider;
using Java.IO;
using Java.Net;
using PhotoViewer.DAO;
using PhotoViewer.Layout;
using PhotoViewer.Layout.ViewModels;
using PhotoViewer.Layout.ViewModels.DomainModels;
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

        public MainController(PictureRepository repository, CommentRepository commentRepository) {
            pictureRepository = repository;
            this.commentRepository = commentRepository;
            thumbnailsLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "thumbs");
            if (!Directory.Exists(thumbnailsLocation)) {
                Directory.CreateDirectory(thumbnailsLocation);
            }
        }

        public Page LoadMainPage() {
            mainScreenViewModel = new MainScreenViewModel(pictureRepository, commentRepository) { AddPictureCommand = new Command(OnAddNewPictureCommand) };

            return mainScreenViewModel.GetView();
        }

        public void AddNewPictureToGalery(string path) {
            string thumbnailPath = SaveThumbnailForImage(path);

            FileInfo info = new FileInfo(path);
            DateTime dateAdded = info.CreationTime;

            mainScreenViewModel.AddNewPicture(path, thumbnailPath, dateAdded);
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

            Bitmap loadImageFromUrl = Bitmap.CreateScaledBitmap(bm, 200, 200, true);
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