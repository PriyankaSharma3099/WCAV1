using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Models
{
   public class WasteTypePickerModel
    {
        public string WasteType { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string ContainerType { get; set; }
        public string SiteName { get; set; }
        public string CPID { get; set; }
        public int ID { get; set; }
        public WasteTypePickerModel(int id, string customerid, string wastetype,string type,string sitename,string containertype,string cpid)
        {
            ID = id;
            CustomerId = customerid;
            WasteType = wastetype;
            Type = type;
            SiteName=sitename;
            ContainerType = containertype;
            CPID = cpid;
        }

    }
}
