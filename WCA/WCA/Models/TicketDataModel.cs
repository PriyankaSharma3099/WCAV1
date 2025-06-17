using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Models
{
    public class TicketDataModel
    {
        public string CPID { get; set; }
        public string TicketNo { get; set; }
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public int CancelreasonId { get; set; }
        public string Cancelreason { get; set; }
        public string CompleteAddress { get; set; }
        public string WasteTypeString { get; set; }
        public string TotalWeight { get; set; }
        public string CustomerOrderId { get; set; }
        public string Invocie_Name { get; set; }
        public bool ExtraWaste_General { get; set; }
        public bool ExtraWaste_DMR { get; set; }
        public bool ExtraWaste_Cardboard { get; set; }
        public bool ExtraWaste_Glass { get; set; }
        public bool ExtraWaste_Food { get; set; }
        public string WB1100_General { get; set; }
        public string WB1100_Food { get; set; }
        public string WB660_General { get; set; }
        public string WB660_Food { get; set; }
        public string WB240_General { get; set; }
        public string WB240_Food { get; set; }
        public string WB1100_DMR { get; set; }
        public string WB660_DMR { get; set; }
        public string WB240_DMR { get; set; }
        public string WB1100_Cardboard { get; set; }
        public string WB660_Cardboard { get; set; }
        public string WB240_Cardboard { get; set; }
        public string WB1100_Glass { get; set; }
        public string WB660_Glass { get; set; }
        public string WB240_Glass { get; set; }


        public string REL08_General { get; set; }
        public string REL12_General { get; set; }
        public string REL14_General { get; set; }
        public string REL16_General { get; set; }
        public string REL08_DMR { get; set; }
        public string REL12_DMR { get; set; }
        public string REL14_DMR { get; set; }
        public string REL16_DMR { get; set; }


        public string REL08_Cardboard { get; set; }
        public string REL12_Cardboard { get; set; }
        public string REL14_Cardboard { get; set; }
        public string REL16_Cardboard { get; set; }

        public string OPEN_25_WasteType { get; set; }
        public string COMP35_WasteType { get; set; }
        public string ENCL35_WasteType { get; set; }
        public string OPEN_40_WasteType { get; set; }
        public string FEL06_General { get; set; }
        public string FEL08_General { get; set; }
        public string FEL12_General { get; set; }
        public string FEL06_DMR { get; set; }
        public string FEL08_DMR { get; set; }
        public string FEL12_DMR { get; set; }
        public string FEL06_Cardboard { get; set; }
        public string FEL08_Cardboard { get; set; }
        public string FEL12_Cardboard { get; set; }
        public string FEL06_Glass { get; set; }
        public string FEL08_Glass { get; set; }
        public string FEL12_Glass { get; set; }


    }
}
