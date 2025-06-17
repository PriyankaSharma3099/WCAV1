using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WCA.Models
{
    public class Vehicle_DataModel
    {

        public string Id { get; set; }
        public string Vehicle_Name { get; set; }
        public string Vehicle_Type { get; set; }

        public Vehicle_DataModel()
        {
        }
        public Vehicle_DataModel(string id, string vehicle_name,string vehicle_type)
        {
            Id = id;
            Vehicle_Name = vehicle_name;
            Vehicle_Type = vehicle_type;
        }
        public ObservableCollection<Vehicle_DataModel> VehicleLists
        {
            get { return new ObservableCollection<Vehicle_DataModel>(); }
            set
            {
                VehicleLists = value;
            }
        }

    }
}
