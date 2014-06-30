using System.Collections.Generic;
using System.Linq;
using PhotoViewer.Domain;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;

namespace PhotoViewer.DAO {
    public class Repository<T, TId> where T : IEntity<TId>, new() {
        private  SQLiteConnection dataBase;

        public Repository(ISQLitePlatform sqLitePlatform, string dbPath) {
            if (dataBase == null) {
                dataBase = new SQLiteConnection(sqLitePlatform, dbPath);

                dataBase.CreateTable<Picture>();
                dataBase.CreateTable<Comment>();
            }
        }

        public IEnumerable<T> GetAll() {
            return dataBase.Table<T>();
        }

        public IEnumerable<T> GetAllWithChildren() {
            return dataBase.Table<T>().ToList().Select(o => {
                dataBase.GetChildren(o);
                return o;
            });
        }

        public T GetById(TId id) {
            return dataBase.GetWithChildren<T>(id);
        }

        public void Insert(T entity) {
            dataBase.Insert(entity);
            dataBase.Commit();
        }

        public void Update(T entity) {
            dataBase.UpdateWithChildren(entity);
            dataBase.Commit();
        }

        public void Delete(T entity) {
            dataBase.Delete(entity);
            dataBase.Commit();
        }

        public void SaveAll(IEnumerable<T> entities) {
            dataBase.Insert(entities);
            dataBase.Commit();
        }
    }
}
