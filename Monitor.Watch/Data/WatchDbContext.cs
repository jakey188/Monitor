using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;

namespace Monitor.Watch.Data
{
    public class WatchDbContext
    {
        /// <summary>
        /// 初始化连接字符为MongoUrl
        /// </summary>
        public WatchDbContext()
            : this("MonitorDb")
        {

        }

        /// <summary>
        /// 初始化连接字符
        /// </summary>
        /// <param name="connectionStringName">connectionStringName</param>
        public WatchDbContext(string connectionStringName= "MonitorDb")
        {
            if (string.IsNullOrWhiteSpace(connectionStringName)) throw new Exception("connectionStringName is not entity");

            ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string ConnectionString { get; set; }


        /// <summary>
        /// 数据库连接
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                var url = new MongoUrlBuilder(ConnectionString);
                var client = new MongoClient(ConnectionString);
                return client.GetDatabase(url.DatabaseName);
            }
        }

        /// <summary>
        /// 获取数据库文档
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <returns></returns>
        public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class
        {
            return Database.GetCollection<TEntity>(DbFunction.GetDbTable(typeof(TEntity)));
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="TEntity">类型参数</typeparam>
        /// <param name="entity">要插入的对象</param>
        public virtual TEntity Add<TEntity>(TEntity entity) where TEntity : class
        {
            this.GetCollection<TEntity>().InsertOne(entity);

            return entity;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">要添加的集合TEntity</param>
        public virtual void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            this.GetCollection<TEntity>().InsertMany(entities);
        }
    }

}
