using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examda.AspNet.Data.Mongo.Driver.NET10;
using Examda.AspNet.Data.Mongo.Driver.NET20;

namespace Monitor.Data
{
    public  class MongoDbContext : DbContext
    {
        public MongoDbContext(string connectionStringName)
            : base(connectionStringName)
        {

        }
        public MongoDbContext(EnmMongoDb db = EnmMongoDb.MonitorDb)
            : base(db.ToString())
        {

        }
    }
}
