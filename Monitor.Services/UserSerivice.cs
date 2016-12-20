using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Monitor.Core;
using Monitor.Data;
using Monitor.Models.Entites;

namespace Monitor.Services
{
    public class UserSerivice
    {
        /// <summary>
        /// 添加或者更新用户
        /// </summary>
        /// <param name="model">用户对象</param>
        public void AddOrUpdate(User model)
        {
            var db = new MongoDbContext();
            var user = db.Where<User>(x => x.UserName == model.UserName).FirstOrDefault();
            if (user == null)
            {
                db.Add(model);
            }
            else
            {
                model.Id = user.Id;
                db.Update(model);
            }
        }

        public void Update(User model)
        {
            var db = new MongoDbContext();
            db.Update(model);
        }

        public void Delete(string id)
        {
            var db = new MongoDbContext();
            var objectId = new ObjectId(id);
            db.Delete<User>(x => x.Id == objectId);
        }

        public User Get(string id)
        {
            var db = new MongoDbContext();
            var objectId = new ObjectId(id);
            return db.Where<User>(x => x.Id == objectId).FirstOrDefault();
        }

        public List<User> GetUserList(int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();
            return db.Where<User>().OrderByDescending(x => x.Id).ToPageList(pageIndex,pageSize,out total);
        }
    }
}
