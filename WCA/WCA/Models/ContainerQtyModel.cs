using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Models
{
   public class ContainerQtyModel
    {
        public string WasteType { get; set; }
        public string SiteName { get; set; }
        public string ContainerType { get; set; }
        public string CustomerId { get; set; }
        public string CPID { get; set; }
        public int index { get; set; }
    //    public bool ExpandedWasteType { get; set; }
        public ContainerQtyModel(int index_get,string customerid,string wastetype,string sitename,string containertype,string cpid) {
            index = index_get;
            CustomerId = customerid;
            WasteType = wastetype;
            SiteName = sitename;
            ContainerType=containertype;
            CPID = cpid;
        //    ExpandedWasteType = expandedwastetype;
        }
    }
}
