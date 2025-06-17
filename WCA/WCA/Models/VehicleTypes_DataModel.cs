using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WCA.Models
{
    public class VehicleTypes_DataModel
    {
        public string Id { get; set; }
        public string Vehicle_Name { get; set; }
        public VehicleTypes_DataModel(string id, string vehicle_name)
        {
            Id = id;
            Vehicle_Name = vehicle_name;
        }
        public ObservableCollection<VehicleTypes_DataModel> VehicleTypesLists
        {
            get { return new ObservableCollection<VehicleTypes_DataModel>(); }
            set
            {
                VehicleTypesLists = value;
            }
        }
    }
}

