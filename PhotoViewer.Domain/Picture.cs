using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PhotoViewer.Domain {
    [Table("Pictures")]
    public class Picture : IEntity<int> {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToMany]
        public IEnumerable<Comment> Comments { get; set; }

        public string OriginalImagePath { get; set; }
       
        public string ThumbnailImagePath { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
