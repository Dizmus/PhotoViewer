﻿using System;
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

        public MainController() {
            thumbnailsLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "thumbs");
            if (!Directory.Exists(thumbnailsLocation)) {
                Directory.CreateDirectory(thumbnailsLocation);
            }
        }

        public Page LoadMainPage() {
            mainScreenViewModel = new MainScreenViewModel { AddPictureCommand = new Command(OnAddNewPictureCommand) };

            PictureRepository repository = GetPictureRepository();
            IList<Picture> enumerable = repository.GetAll().ToList();

            IList<PictureModel> pictureModels = enumerable.Select(picture => new PictureModel() {
                CommentsCount = (picture.Comments != null ? picture.Comments.Count() : 0).ToString(CultureInfo.InvariantCulture),
                ImageSource = ImageSource.FromFile(picture.ThumbnailImagePath),
                ImageDate = picture.DateAdded.ToString(CultureInfo.InvariantCulture),
                Id =  picture.Id
            }).ToList();

           
            mainScreenViewModel.Pictures = new ObservableCollection<PictureModel>(pictureModels);

            return mainScreenViewModel.GetView();
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

            FileInfo info = new FileInfo(path);
            DateTime dateAdded = info.CreationTime;
            
            Picture picture = new Picture();
            picture.OriginalImagePath = path;
            picture.ThumbnailImagePath = thumbnailPath;
            picture.DateAdded = dateAdded;
            repository.Insert(picture);

            ImageSource imageSource = ImageSource.FromFile(thumbnailPath);
            PictureModel model = new PictureModel {
                ImageSource = imageSource,
                CommentsCount = "0",
                ImageDate = dateAdded.ToString(CultureInfo.InvariantCulture),
                Id =  picture.Id
            };
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