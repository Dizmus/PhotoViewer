using System;
using PhotoViewer.Domain;
using SQLite.Net.Interop;

namespace PhotoViewer.DAO {
    public class PictureRepository : Repository<Picture, int> {
        public PictureRepository(ISQLitePlatform sqLitePlatform, string dbPath) : base(sqLitePlatform, dbPath) {
        }
    }
}