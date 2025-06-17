using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WCA.Models
{
    public class TradeWasteConfig_DataModel
    {
        public string TicketStart { get; set; }
        public string Date { get; set; }
        public string DriverID { get; set; }
        public string VehicleID { get; set; }
        public TradeWasteConfig_DataModel()
        {
        }
        public TradeWasteConfig_DataModel(string ticketStart, string date,string driverID, string vehicleID)
        {
            TicketStart = ticketStart;
            Date = date;
            DriverID = driverID;
            VehicleID = vehicleID;
        }
        public ObservableCollection<TradeWasteConfig_DataModel> ConfigLists
        {
            get { return new ObservableCollection<TradeWasteConfig_DataModel>(); }
            set
            {
                ConfigLists = value;
            }
        }
    }
}
