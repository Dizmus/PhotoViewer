using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoViewer.Layout.ViewModels;
using Xamarin.Forms;

namespace PhotoViewer.Layout {
    public class MainScreen : ContentPage {
        public MainScreen() {
            Grid contentGrid = new Grid() {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = new RowDefinitionCollection() {
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition() {Height = GridLength.Auto}
                }
            };

            StackLayout stackLayoutNoImageExists = new StackLayout() {
                Orientation = StackOrientation.Vertical,
            };

            stackLayoutNoImageExists.SetBinding<MainScreenViewModel>(IsVisibleProperty, m => m.NoPictureExists);

            Label noPicturesLabel = new Label() {
                Text = "You have no pictures, add pictures now!",
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };
            stackLayoutNoImageExists.Children.Add(noPicturesLabel);

            contentGrid.Children.Add(stackLayoutNoImageExists, 0, 0);

            StackLayout stackLayoutImages = new StackLayout() {
                Orientation = StackOrientation.Vertical
            };

            stackLayoutImages.SetBinding<MainScreenViewModel>(IsVisibleProperty, m => m.AnyPictureExists);
            contentGrid.Children.Add(stackLayoutImages, 0, 0);

            stackLayoutImages.Children.Add(new
                Label {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "Your Pictures"
                });

            ListView listView = new ListView() {
                RowHeight = 70,
                ItemTemplate = new DataTemplate(GetListViewItemTemplate),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            listView.SetBinding<MainScreenViewModel>(ListView.ItemsSourceProperty, m => m.Pictures);
            listView.SetBinding<MainScreenViewModel>(ListView.SelectedItemProperty, m => m.SelectedPicture);

            stackLayoutImages.Children.Add(listView);

            Button addImageButton = new Button() { Text = "Add pictures" };

            addImageButton.SetBinding<MainScreenViewModel>(Button.CommandProperty, m => m.AddPictureCommand);

            contentGrid.Children.Add(addImageButton, 0, 1);

            this.Content = contentGrid;
        }

        private object GetListViewItemTemplate() {
            Grid grid = new Grid() {
                Padding = new Thickness(2, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = new RowDefinitionCollection() {
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions = new ColumnDefinitionCollection() {
                    new ColumnDefinition() {Width = GridLength.Auto},
                    new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            Image image = new Image() {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            image.SetBinding<PictureModel>(Image.SourceProperty, p => p.ImageSource);
            grid.Children.Add(image, 0, 1, 0, 2);

            Label imageDateLabel = new Label() {
                YAlign = TextAlignment.Center
            };

            imageDateLabel.SetBinding<PictureModel>(Label.TextProperty, m => m.ImageDate);
            grid.Children.Add(imageDateLabel, 1, 0);

            StackLayout commentsStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center };

            Label commentsCountLabel = new Label();
            commentsCountLabel.SetBinding<PictureModel>(Label.TextProperty, p => p.CommentsCount);

            commentsStackLayout.Children.Add(new Label() { Text = "Comments: " });
            commentsStackLayout.Children.Add(commentsCountLabel);

            grid.Children.Add(commentsStackLayout, 1, 1);

            return new ViewCell {
                View = grid,
            };
        }
    }
}
