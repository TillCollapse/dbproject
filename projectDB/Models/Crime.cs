using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectDB.Models
{
    
    public class Crime
    {
        public Crime()
        {
            
        }
        [BsonId]
        public ObjectId Id;
        [BsonElement("DateRptd")]
        public string DateRptd;
        [BsonElement("DRNO")]
        public int DrNo;
        [BsonElement("DATEOCC")]
        public string DateOcc;
        [BsonElement("TIMEOCC")]
        public int TimeOcc;
        [BsonElement("AREA")]
        public int Area;
        [BsonElement("AREANAME")]
        public string AreaName;
        [BsonElement("RD")]
        public int Rd;
        [BsonElement("CrmCd")]
        public int CrmCd;
        [BsonElement("CrmCdDesc")]
        public string CrmCdDesc;
        [BsonElement("Status")]
        public string Status;
        [BsonElement("StatusDesc")]
        public string StatusDesc;
        [BsonElement("LOCATION")]
        public string Location;
        [BsonElement("CrossStreet")]
        public object CrossStreeet;
        [BsonElement("Location")]
        public string Location1;

    }
}
