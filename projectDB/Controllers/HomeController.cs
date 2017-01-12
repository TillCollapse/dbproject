using System;
using System.Messaging;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using projectDB.Models;
using IAsyncCursorSourceExtensions = MongoDB.Driver.IAsyncCursorSourceExtensions;
using IMongoCollectionExtensions = MongoDB.Driver.IMongoCollectionExtensions;

namespace projectDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoDatabase _database;

        public HomeController()
        {
            var client = new MongoClient(new MongoClientSettings { Server = new MongoServerAddress("localhost", 3979) });
            _database = client.GetDatabase("dvproject");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Statistics(string value)
        {
            return View();
        }

        public ActionResult StatisticsResults(string value)
        {
            var collection = _database.GetCollection<Crime>("crimes");
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Cracov");
            var getCrimes =
                IAsyncCursorSourceExtensions.ToList(IMongoCollectionExtensions.Find(collection, filter).Limit(1000));
            return View(getCrimes);
        }

        /*
         * CRUD operations
         */

        //create
        public void CreateCrime(int docNumber)
        {
            var collection = _database.GetCollection<Crime>("crimes");

            collection.InsertOne(new Crime()
            {
                Id = ObjectId.GenerateNewId(),
                DateRptd = "01/01/2016",
                DrNo = 1,
                DateOcc = "01/01/2016",
                TimeOcc = 1,
                Area = 1,
                AreaName = "Cracov",
                CrmCd = 1,
                Rd = 1,
                CrmCdDesc = "1",
                Status = "fail",
                StatusDesc = "fail",
                Location = "Cracov",
                CrossStreeet = "Blabla",
                Location1 = "Cracov1"
            });
        }

        //read
        public string ReadCrime()
        {
            var collection = _database.GetCollection<Crime>("crimes");
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Harbor");
            var getCrimes =
                IAsyncCursorSourceExtensions.ToList(IMongoCollectionExtensions.Find(collection, filter).Limit(10));

            return new JavaScriptSerializer().Serialize(getCrimes);
        }

        //update
        public void UpdateCrime(string filterKey, string filterValue, string updateKey, string updateValue)
        {
            var collection = _database.GetCollection<Crime>("crimes");
            var filter = Builders<Crime>.Filter.Eq(filterKey, filterValue);
            var update = Builders<Crime>.Update.Set(updateKey, updateValue);
            collection.UpdateOne(filter, update);
        }

        //delete - removes documents that match the specified condition
        public void DeleteCrime()
        {
            var collection = _database.GetCollection<Crime>("crimes");
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Cracov");
            collection.DeleteMany(filter);
        }

        
        
    }
}