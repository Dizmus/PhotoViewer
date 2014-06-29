using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using PhotoViewer.Domain;
using TinyIoC;

namespace PhotoViewer.Services.Android {
    public class ImagingService : IImagingService {
        public static readonly int PickImageId = 1000;
        
        public Picture RequestNewImage() {
            Activity activity = TinyIoCContainer.Current.Resolve<Activity>();

            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            activity.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PickImageId);
            return null;

        }
    }
}
