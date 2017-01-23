using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using projectDB.Models;

namespace projectDB.Controllers
{
    public class CRUDController : Controller
    {
        private readonly IMongoDatabase _database;

        public CRUDController()
        {
            var client = new MongoClient(new MongoClientSettings { Server = new MongoServerAddress("localhost", 3979) });
            _database = client.GetDatabase("dvproject");
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Count(string key, string value)
        { 
            //value = "Cracov";
            key = "AREANAME";
            var collection = _database.GetCollection<Crime>("crimes");
            var filter = Builders<Crime>.Filter.Eq(key, value);
            var result = collection.Count(filter);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CRUDTest()
        {
            //return Json("chamara", JsonRequestBehavior.AllowGet);
            return View();
        }
        public JsonResult RunCRUDTest()
        {
            var collection = _database.GetCollection<Crime>("crimes");
            int[] operationsNumbers = new int[] { 10,100, 1000, 10000, 100000 };
            //orginal mongo test
            float[] mongoCreateResults = MongoCreateTest(collection, operationsNumbers);
            float[] mongoReadResults = MongoReadTest(collection, operationsNumbers);
            float[] mongoUpdateResults = MongoUpdateTest(collection, operationsNumbers);
            float[] mongoDeleteResults = MongoRemoweTest(collection, operationsNumbers);

            //float[] mongoCreateResults = new float[] { 2, 25, 398, 3012, 23341 };
            //float[] mongoReadResults = new float[] { 0, 0, 0, 0, 0 };
            //float[] mongoUpdateResults = new float[] { 686, 638, 630, 637, 646 };
            //float[] mongoDeleteResults = new float[] { 635, 623, 660, 740, 1982 };



            //MySql tests wyniki do podmiany
            float[] mySQLCreateResults = new float[] { 304.8f, 671.3f, 4401.84f, 42728.93f, 645479.74f };
            float[] mySQLReadResults = new float[] { 1, 1, 1, 1, 1 };
            float[] mySQLUpdateResults = new float[] { 315.73f, 689.93f, 4413.13f, 43212.01f, 654231.45f };
            float[] mySQLDeleteResults = new float[] { 311.28f, 1146.69f, 8200.47f, 80384.67f, 1192194.49f };

            return Json(new {
                                operationsNumbers = operationsNumbers,
                                mongoCreate = mongoCreateResults,
                                mongoRead = mongoReadResults,
                                mongoUpdate = mongoUpdateResults,
                                mongoDelete = mongoDeleteResults,

                                mySQLCreate = mySQLCreateResults, 
                                mySQLRead = mySQLReadResults,
                                mySQLmongoUpdate = mySQLUpdateResults,
                                mySQLDelete = mySQLDeleteResults}, JsonRequestBehavior.AllowGet); 
        }

        private float[] MongoCreateTest(IMongoCollection<Crime> collection, int[] insertNumbers)
        {
            System.Diagnostics.Debug.WriteLine("**********UpdateTest**********");
            float[] results = new float[insertNumbers.Length];
            for (int i = 0; i < insertNumbers.Length; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                mongoInsertOperation(collection, insertNumbers[i]);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                results[i] = elapsedMs;
                System.Diagnostics.Debug.WriteLine(insertNumbers[i] + "\t\t" + elapsedMs);
            }

            return results;
        }

        private float[] MongoReadTest(IMongoCollection<Crime> collection, int[] operationsNumbers)
        {
            System.Diagnostics.Debug.WriteLine("**********ReadTest**********");
            float[] results = new float[operationsNumbers.Length];
            for (int i = 0; i < operationsNumbers.Length; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                mongoReadOperation(collection, operationsNumbers[i]);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                results[i] = elapsedMs;
                System.Diagnostics.Debug.WriteLine(operationsNumbers[i] + "\t\t" + elapsedMs);
            }

            return results;
        }
        private float[] MongoUpdateTest(IMongoCollection<Crime> collection, int[] operationsNumbers) {
            System.Diagnostics.Debug.WriteLine("**********UpdateTest**********");
            float[] results = new float[operationsNumbers.Length];
            for (int i = 0; i < operationsNumbers.Length; i++)
            {
                mongoRemoveOperation(collection);
                mongoUpdateOperation(collection, operationsNumbers[i]);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                mongoRemoveOperation(collection);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                results[i] = elapsedMs;
                System.Diagnostics.Debug.WriteLine(operationsNumbers[i] + "\t\t" + elapsedMs);
            }

            return results;
        }
        private float[] MongoRemoweTest(IMongoCollection<Crime> collection, int[] operationsNumbers)
        {
            System.Diagnostics.Debug.WriteLine("**********RemoveTest**********");
            float[] results = new float[operationsNumbers.Length];
            for (int i = 0; i < operationsNumbers.Length; i++)
            {
                mongoRemoveOperation(collection);
                mongoInsertOperation(collection, operationsNumbers[i]);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                mongoRemoveOperation(collection);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                results[i] = elapsedMs;
                System.Diagnostics.Debug.WriteLine(operationsNumbers[i] + "\t\t" + elapsedMs);
            }

            return results;
        }



        private void mongoInsertOperation(IMongoCollection<Crime> collection, int insertNumber)
        {
            for (var i = 0; i < insertNumber; i++)
            {
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
        }
        private void mongoUpdateOperation(IMongoCollection<Crime> collection, int insertNumber)
        {
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Cracov");
            var update = Builders<Crime>.Update.Set("AREANAME", "Warsaw");
            collection.UpdateMany(filter, update);
        }
        private void mongoRemoveOperation(IMongoCollection<Crime> collection)
        {
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Cracov");
            collection.DeleteMany(filter);
            
        }
       
        private void mongoReadOperation(IMongoCollection<Crime> collection, int operationNumber) {
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Cracov");
            var results = collection.Find(filter).Limit(operationNumber);
        }
    }
}