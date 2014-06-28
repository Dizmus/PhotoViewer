using System;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PhotoViewer.Contollers;
using PhotoViewer.Layout;
using PhotoViewer.Layout.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Uri = Android.Net.Uri;

namespace PhotoViewer {
    [Activity(Label = "PhotoViewer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AndroidActivity {
        private MainController mainController;
        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            mainController = TinyIoCContainer.Current.Resolve<MainController>();
            mainController.AddNewPictureEvent +=OnAddNewPictureEvent;

            TinyIoCContainer.Current.Register<Activity>(this);

            Forms.Init(this, bundle);

            Page mainPage = mainController.LoadMainPage();
            SetPage(mainPage);
        }

        private void OnAddNewPictureEvent() {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null)) {
                Uri uri = data.Data;
                string path = GetPathToImage(uri);
                mainController.AddNewPictureToGalery(path);
            }
        }

        private string GetPathToImage(Uri uri) {
            string path = null;
            // The projection contains the columns we want to return in our query.
            string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
            using (ICursor cursor = ManagedQuery(uri, projection, null, null, null)) {
                if (cursor != null) {
                    int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }
            return path;
        }
    }
}

