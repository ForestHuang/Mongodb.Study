using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Mongodb.Common
{
    public class MongodbHelper<T> where T : class
    {
        private static string serverHost = string.Empty;
        private static string databaseName = string.Empty;
        private static string collectionName = string.Empty;

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
        private static IMongoDatabase GetMongodbDataBase()
        {
            //return new MongoClient(serverHost).GetDatabase(new MongoUrl(serverHost).DatabaseName);
            return new MongoClient(serverHost).GetDatabase(databaseName);
        }

        /// <summary>
        /// Insert （单条插入）
        /// </summary>
        /// <param name="entity">数据对象</param>
        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity", "待插入数据不能为空");
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                collection.InsertOne(entity);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Insert （批量插入）
        /// </summary>
        /// <param name="listEntity">数据集合</param>
        public void InsertBatch(List<T> listEntity)
        {
            try
            {
                if (listEntity == null && listEntity.Count <= 0)
                    throw new ArgumentNullException("listEntity", "待插入数据不能为空");
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                collection.InsertMany(listEntity);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Quary FindAll （查询全部数据）
        /// </summary>
        /// <param name="pageCount">总条数</param>
        /// <param name="isPaging">是否分页</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="func">条件表达式</param>
        /// <param name="orderby">排序表达式</param>
        /// <returns>泛型集合</returns>
        public List<T> FindAll(out int pageCount, bool isPaging = false, int pageIndex = 1, int pageSize = 50, Func<T, bool> func = null, Func<T, bool> orderby = null)
        {
            try
            {
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                var listResut = collection.Find(new BsonDocument()).ToList<T>();
                pageCount = listResut.Count;
                if (isPaging)
                    return listResut.Where(func).OrderBy(orderby).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList<T>();
                else
                    return listResut.Where(func).OrderBy(orderby).ToList<T>();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Quary Find （查询单条数据）
        /// </summary>
        /// <param name="func">条件表达式</param>
        /// <returns>T</returns>
        public T Find(Expression<Func<T, bool>> func)
        {
            try
            {
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                return collection.Find(func).FirstOrDefault();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Delete （根据条件批量删除数据）
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <returns>bool</returns>
        public bool Delete(Expression<Func<T, bool>> func)
        {
            try
            {
                bool result = false;
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                var delete = collection.DeleteMany(func);
                result = delete != null && delete.DeletedCount > 0;
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Update （根据条件替换整条数据）
        /// </summary>
        /// <param name="func">条件表达式</param>
        /// <param name="update">修改数据集合</param>
        /// <returns>bool</returns>
        public bool UpdateReplace(Expression<Func<T, bool>> func, T entity)
        {
            try
            {
                bool result = false;
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                var update = collection.ReplaceOne(func, entity, new UpdateOptions() { IsUpsert = true });
                result = update != null && update.ModifiedCount > 0;
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Update （根据条件修改数据）
        /// </summary>
        /// <param name="func">条件表达式</param>
        /// <param name="entity">修改对象</param>
        /// <returns>bool</returns>
        public bool Update(Expression<Func<T, bool>> func, T entity)
        {
            try
            {
                bool result = false;
                BsonDocument bsonDocument = BsonExtensionMethods.ToBsonDocument(entity);
                var collection = GetMongodbDataBase().GetCollection<T>(collectionName);
                var update = collection.UpdateOne(func, bsonDocument, new UpdateOptions() { IsUpsert = true });
                result = update != null && update.ModifiedCount > 0;
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}