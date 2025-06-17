using System;
using System.Collections.ObjectModel;

namespace WCA.Models
{
    public class Areas_DataModel
    {
        public string Id { get; set; }
        public string Area_Name { get; set; }

        public Areas_DataModel()
        {
        }
        public Areas_DataModel(string id, string area_name)
        {
            Id = id;
            Area_Name = area_name;
        }
        public ObservableCollection<Areas_DataModel> AreaLists
        {
            get { return new ObservableCollection<Areas_DataModel>(); }
            set
            {
                AreaLists = value;
            }
        }
    }
}
