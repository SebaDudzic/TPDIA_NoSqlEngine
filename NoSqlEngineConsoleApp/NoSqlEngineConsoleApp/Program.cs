using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlEngineConsoleApp;
using MongoDB.Bson;
using MongoDB.Driver;
using NoSqlEngineConsoleApp;


namespace TestMongo
{
    class Program
    {

        static void Main(string[] args)
        {

            DbEngine dbEngine = new DbEngine();
            var sender = new DataSender(dbEngine);
            dbEngine.RunAllTests().Wait();
          
            Console.ReadLine();
        }     
    }
}
