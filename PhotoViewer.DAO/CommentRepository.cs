using System;
using PhotoViewer.Domain;
using SQLite.Net.Interop;

namespace PhotoViewer.DAO {
    public class CommentRepository : Repository<Comment, int> {
        public CommentRepository(ISQLitePlatform sqLitePlatform, string dbPath) : base(sqLitePlatform, dbPath) {
        }
    }
}