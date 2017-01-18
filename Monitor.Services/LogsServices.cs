using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Data;
using Monitor.Models.Entites;

namespace Monitor.Services
{
    public class LogsServices
    {
        public void Add(Logs log)
        {
            var db = new MongoDbContext();
            db.Add<Logs>(log);
        }
    }
}
