using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PhotoViewer.Domain;
using PhotoViewer.Layout;
using PhotoViewer.Layout.ViewModels;
using Xamarin.Forms;

namespace PhotoViewer {
    public class App {
        public static Page GetMainPage(MainScreenViewModel viewModel) {
            ContentPage mainScreen = new MainScreen();
            mainScreen.BindingContext = viewModel;
            return mainScreen;
        }
    }
}