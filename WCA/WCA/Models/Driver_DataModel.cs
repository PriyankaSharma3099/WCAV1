using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WCA.Models
{
    public class Driver_DataModel
    {
        public string Id { get; set; }
        public string Driver_Name { get; set; }

        public Driver_DataModel() { 
        }
        public Driver_DataModel(string id, string driver_name) 
        {
            Id = id;
            Driver_Name = driver_name;
        }
        public ObservableCollection<Driver_DataModel> DriverLists
        {
            get {return new ObservableCollection<Driver_DataModel>(); }
            set
            {
                DriverLists = value;
            }
        }
    }
}
