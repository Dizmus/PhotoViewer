using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Android.App;
using Android.Runtime;
using PhotoViewer.Contollers;
using PhotoViewer.DAO;
using SQLite.Net.Platform.XamarinAndroid;
using TinyIoC;

namespace PhotoViewer {
    [Application(Debuggable = true, ManageSpaceActivity = typeof(MainActivity))]
    public class App : Application {
        protected App(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) {
        }

        public App() {
        }

        public override void OnCreate() {
            base.OnCreate();

            SetupIoc();
        }

        private static void SetupIoc() {
            SQLitePlatformAndroid sqLitePlatformAndroid = new SQLitePlatformAndroid();
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "photoviewer.db3");

            PictureRepository pictureRepository = new PictureRepository(sqLitePlatformAndroid, path);
            CommentRepository commentRepository = new CommentRepository(sqLitePlatformAndroid, path);

            TinyIoCContainer.Current.Register(pictureRepository);
            TinyIoCContainer.Current.Register(commentRepository);
            TinyIoCContainer.Current.Register<MainController>();
          }
    }
}