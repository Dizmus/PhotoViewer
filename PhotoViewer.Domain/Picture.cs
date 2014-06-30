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
        private List<Comment> comments;

        public Picture() {
            comments = new List<Comment>();
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToMany]
        public List<Comment> Comments {
            get { return comments; }
            set { comments = value; }
        }

        public string OriginalImagePath { get; set; }
       
        public string ThumbnailImagePath { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
