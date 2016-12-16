using System;
using System.Linq;

namespace Monitor.Watch
{
    public class DbFunction
    {
        public static string GetDbTable(Type type)
        {
            var attributes = type.GetCustomAttributes(false).Where(attr => attr.GetType().Name == "TableAttribute").SingleOrDefault() as dynamic;

            return attributes!=null ? attributes.Name : type.Name;

            //var attributes = (DbTableAttribute[])type.GetCustomAttributes(typeof(DbTableAttribute), false);

            //return attributes.Length > 0 ? attributes[0].Name : type.Name;
        }

    }
}
