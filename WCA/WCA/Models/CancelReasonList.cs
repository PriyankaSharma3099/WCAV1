using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Models
{
    public class CancelReasonList // model for bind cancel reason list data 
    {
        public string WasteType { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public int ID { get; set; }
        public string SiteName { get; set; }
        public string CPID { get; set; }
        public CancelReasonList(int id, string customerid, string wastetype, string type,string sitename,string cpid)
        {
            ID = id;
            CustomerId = customerid;
            WasteType = wastetype;
            Type = type;
            SiteName = sitename;
            CPID = cpid;
        }
    }
}
