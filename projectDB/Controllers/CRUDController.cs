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

        public ActionResult CRUDTest()
        {
            //return Json("chamara", JsonRequestBehavior.AllowGet);
            return View();
        }
        public JsonResult RunCRUDTest()
        {
            var collection = _database.GetCollection<Crime>("crimes");
            int[] operationsNumbers = new int[] { 100, 1000, 10000 };
            //float[] mongoCreateResults = MongoCreateTest(collection, insertNumbers);
            float[] mongoCreateResults = new float[] {43, 434, 3754};
            
            
            //Tutaj powinienneś wsadzić swoje wyniki
            float [] mySQLCreateResults = new float[] {80, 800, 8000};
            return Json(new {
                                operationsNumbers = operationsNumbers,
                                mongoCreate = mongoCreateResults,
                                mySQLCreate = mySQLCreateResults 
                            }, JsonRequestBehavior.AllowGet); 
        }

        //Metoda testująca operację Crud dla 100, 1000, 10000, 10000 operacji
        public float[] MongoCrudTest()
        {

            var collection = _database.GetCollection<Crime>("crimes");
            //int[] insertNumbers = new int[] { 100, 1000, 10000, 100000 };
            int[] insertNumbers = new int[] { 100, 1000, 10000 };

            float[] createResults = MongoCreateTest(collection, insertNumbers);
            float[] removeResults = MongoRemoweTest(collection, insertNumbers);

            return removeResults;
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

        private float[] MongoRemoweTest(IMongoCollection<Crime> collection, int[] insertNumbers)
        {
            System.Diagnostics.Debug.WriteLine("**********RemoveTest**********");
            float[] results = new float[insertNumbers.Length];
            for (int i = 0; i < insertNumbers.Length; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                mongoRemoveOperation(collection, insertNumbers[i]);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                results[i] = elapsedMs;
                System.Diagnostics.Debug.WriteLine(insertNumbers[i] + "\t\t" + elapsedMs);
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

        private void mongoRemoveOperation(IMongoCollection<Crime> collection, int insertNumber)
        {
            var filter = Builders<Crime>.Filter.Eq("AREANAME", "Cracov");
            for (var i = 0; i < insertNumber; i++)
            {
                collection.DeleteOne(filter);
            }
        }
    }
}