using PhotoViewer.Layout.ViewModels;
using PhotoViewer.Layout.ViewModels.DomainModels;
using Xamarin.Forms;

namespace PhotoViewer.Layout {
    public class ViewImageView : ContentPage {
        public ViewImageView(ViewImageViewModel dataContext) {
            this.BindingContext = dataContext;
            Grid mainGrid = new Grid() {
                RowDefinitions = new RowDefinitionCollection() {
                    new RowDefinition() {Height = new GridLength(4, GridUnitType.Star)},
                    new RowDefinition() {Height = new GridLength(3, GridUnitType.Star)},
                    new RowDefinition() {Height = GridLength.Auto},
                    new RowDefinition() {Height = GridLength.Auto}
                }
            };

            Content = mainGrid;

            Image image = new Image() {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            image.SetBinding<ViewImageViewModel>(Image.SourceProperty, m => m.ImageSource);

            mainGrid.Children.Add(image, 0, 0);

            ListView commentsListView = PrepareCommentsListView();

            mainGrid.Children.Add(commentsListView, 0, 1);

            Label noComments = new Label() { Text = "Picture has no comments", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
            noComments.SetBinding<ViewImageViewModel>(IsVisibleProperty, m => m.NoPictureExists);
            mainGrid.Children.Add(noComments, 0, 1);

            Entry commentEntryBox = new Entry() { Keyboard = Keyboard.Chat };
            mainGrid.Children.Add(commentEntryBox, 0, 2);

            commentEntryBox.Placeholder = "Add New Comment";
            commentEntryBox.SetBinding<ViewImageViewModel>(Entry.TextProperty, m => m.NewCommentText);

            Button addNewCommentButton = new Button() { Text = "Add" };
            addNewCommentButton.SetBinding<ViewImageViewModel>(Button.CommandProperty, m => m.AddCommentCommand);

            mainGrid.Children.Add(addNewCommentButton, 0, 3);
        }

        private ListView PrepareCommentsListView() {
            ListView listView = new ListView() {
                ItemTemplate = new DataTemplate(() => {
                    StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };

                    Label commentDateLabel = new Label();
                    stackLayout.Children.Add(commentDateLabel);
                    commentDateLabel.SetBinding<CommentModel>(Label.TextProperty, m => m.CommentDate);


                    Label commentTextLabel = new Label();
                    stackLayout.Children.Add(commentTextLabel);
                    commentDateLabel.SetBinding<CommentModel>(Label.TextProperty, m => m.CommentText);
                    return new ViewCell {
                        View = stackLayout,
                    };
                }),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            listView.SetBinding<ViewImageViewModel>(ListView.ItemsSourceProperty, m => m.Comments);

            return listView;
        }
    }
}
