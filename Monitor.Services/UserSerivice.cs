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
        public bool Add(User model)
        {
            var db = new MongoDbContext();
            var user = db.Where<User>(x => x.UserName == model.UserName).FirstOrDefault();
            if (user == null)
            {
                db.Add(model);
                return true;
            }
            return false;
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

        public bool IsExitUser(string userName)
        {
            var db = new MongoDbContext();
            return db.Where<User>(x => x.UserName == userName).Any();
        }

        public User Get(string id)
        {
            var db = new MongoDbContext();
            var objectId = new ObjectId(id);
            return db.Where<User>(x => x.Id == objectId).FirstOrDefault();
        }

        public User Get(string username,string password)
        {
            var db = new MongoDbContext();
            return db.Where<User>(x => x.UserName == username && x.Password==password).FirstOrDefault();
        }

        public List<User> GetUserList(string userName,int pageIndex,int pageSize,out int total)
        {
            var db = new MongoDbContext();
            var query = db.Where<User>();
            if (!string.IsNullOrEmpty(userName))
                query = query.Where(x => x.UserName == userName);
            return query.OrderByDescending(x => x.Id).ToPageList(pageIndex,pageSize,out total);
        }
    }
}
