using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PhotoViewer.Domain {
    [Table("Pictures")]
    public class Picture {
        [OneToMany]
        public IEnumerable<Comment> Comments { get; set; }
    }
}
