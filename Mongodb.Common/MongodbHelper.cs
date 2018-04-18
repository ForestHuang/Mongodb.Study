using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;


namespace Mongodb.Common
{
    public class MongodbHelper<T> where T : class
    {
        private static string serverHost = string.Empty;
        private static string databaseName = string.Empty;
        private static string collectionName = string.Empty;

        private static readonly object lockMongodb = new object();

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="serverHost">链接地址（serverHost）</param>
        /// <param name="databaseName">数据库名（dataBase）</param>
        /// <param name="collectionName">表名（collections）</param>
        public MongodbHelper(string _serverHost, string _databaseName, string _collectionName)
        {
            serverHost = _serverHost;
            databaseName = _databaseName;
            collectionName = _collectionName;
        }

        /// <summary>
        /// create Mongodb
        /// </summary>
        /// <returns>MongoDatabase</returns>
        private static IMongoDatabase GetMongodbDataBase() { return new MongoClient(serverHost).GetDatabase(databaseName); }

        /// <summary>
        /// Insert 
        /// </summary>
        /// <param name="entity">数据库对象</param>
        public void Insert(T entity)
        {
            try
            {
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                collection.InsertOne(entity);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<T> FindAll()
        {
            try
            {
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                return collection.Find(new BsonDocument()).ToList<T>();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
