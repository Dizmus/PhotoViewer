using System;
using System.IO;
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
            PictureRepository.SQLitePlatform = new SQLitePlatformAndroid();
            PictureRepository.DbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "photoviewer.db3");

            TinyIoCContainer.Current.Register<MainController>();
            TinyIoCContainer.Current.Register<PictureRepository>();
            TinyIoCContainer.Current.Register<CommentRepository>();
        }
    }
}