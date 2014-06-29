using System;
using SQLite.Net.Attributes;

namespace PhotoViewer.Domain {
    [Table("Comments")]
    public class Comment : IEntity<int> {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime PostDate { get; set; }
    }
}