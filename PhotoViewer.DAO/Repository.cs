using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoViewer.Domain;
using SQLite.Net;
using SQLite.Net.Interop;

namespace PhotoViewer.DAO {
    public class Repository<T, TId> where T : IEntity<TId>, new() {
        private static SQLiteConnection dataBase;
        private TableQuery<T> dataContext;

        public Repository() {
            if (dataBase == null) {
                dataBase = new SQLiteConnection(SQLitePlatform, DbPath);

                dataBase.CreateTable<Picture>();
                dataBase.CreateTable<Comment>();
            }

            dataContext = dataBase.Table<T>();
        }

        public static ISQLitePlatform SQLitePlatform { get; set; }

        public static string DbPath { get; set; }

        public IEnumerable<T> GetAll() {
            return dataContext;
        }

        public T GetById(TId id) {
            return dataContext.SingleOrDefault(o => o.Id.Equals(id));
        }

        public void Insert(T entity) {
            dataBase.Insert(entity);
        }

        public void Update(T entity) {
            dataBase.Update(entity);
        }

        public void Delete(T entity) {
            dataBase.Delete(entity);
        }

        public void SaveAll(IEnumerable<T> entities) {
            dataBase.Insert(entities);
        }

        public void Dispose() {
            dataContext = null;
        }
    }
}
