using System.Web.Mvc;
using System.Web.Script.Serialization;
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
            var client = new MongoClient(new MongoClientSettings {Server = new MongoServerAddress("localhost", 3979)});
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

        /*
         * CRUD operations
         */

        //create
        public void CreateCrime()
        {
            var collection = _database.GetCollection<Crime>("crimes");
            var crime = new Crime();
            collection.InsertOne(crime);
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
        public void DeleteCrime(string id)
        {
            var collection = _database.GetCollection<Crime>("crimes");
            var filter = Builders<Crime>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }
    }
}