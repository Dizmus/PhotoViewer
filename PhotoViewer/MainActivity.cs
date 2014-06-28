using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PhotoViewer.Layout;
using PhotoViewer.Layout.ViewModels;
using Xamarin.Forms.Platform.Android;

namespace PhotoViewer {
    [Activity(Label = "PhotoViewer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AndroidActivity {
        int count = 1;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            MainScreenViewModel mainScreenViewModel = new MainScreenViewModel();
            mainScreenViewModel.MainText = "Hello there";

            SetPage(App.GetMainPage(mainScreenViewModel));
        }
    }
}

