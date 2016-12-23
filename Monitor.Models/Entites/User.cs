using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Monitor.Models.Entites
{
    [Table("User")]
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TrueName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }

    public enum EnmUserRole
    {
        管理员 = 1,
        开发人员 = 2
    }
}
