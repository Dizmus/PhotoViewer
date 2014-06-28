using SQLite.Net.Attributes;

namespace PhotoViewer.Domain {
    [Table("Comments")]
    public class Comment {
        public string Text { get; set; }
    }
}