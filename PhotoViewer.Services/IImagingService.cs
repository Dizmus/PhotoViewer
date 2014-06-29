using PhotoViewer.Domain;

namespace PhotoViewer.Services {
    public interface IImagingService {
        Picture RequestNewImage();
    }
}