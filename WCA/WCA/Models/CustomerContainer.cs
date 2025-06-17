using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace WCA.Models
{
    public class CustomerContainer :INotifyPropertyChanged
    {
        public int Qty { get; set; }
        public bool IsVisibleShowChild { get; set; }
        public string ContainerName { get; set; }
        public string WasteType { get; set; }
        public string SiteName { get; set; }

        private int _weight_t;
        public int Weight_T
        {
            get { return _weight_t; }
            set
            {
                if (_weight_t != value)
                {
                    _weight_t = value;
                    OnPropertyChanged(nameof(Weight_T));
                }
            }
        }

        private bool _weightVisibility;
        public bool WeightVisibility
        {
            get { return _weightVisibility; }
            set
            {
                if (_weightVisibility != value)
                {
                    _weightVisibility = value;
                    OnPropertyChanged(nameof(WeightVisibility));
                }
            }
        }
        public string CPID { get; set; }
        public int RunNo { get; set; }
        public bool Completed { get; set; }
        public bool IsVisibilityWasteType { get; set; }

  //      public int CancelReasonID { get; set; }
        public string CustomerID { get; set; }
        public int ExtraWasteId { get; set; }
   //     public int CancelReasonId { get; set; }
        public string ExtraWasteType { get; set; }
   //     public string CancelReasonType { get; set; }
        public bool ChargeAsContract
        {
            get;
            set;
    }
        public ObservableCollection<ContainerQtyModel> ContainerListPicker
        {
            get;
            set;
        }
        public ObservableCollection<WasteTypePickerModel> ExtraWasteTypeList
        {
            get;
            set;
        }
     

        public CustomerContainer()
        {

        }
        //public CustomerContainer(int qty, string container_name, string waste_type, int run_no,bool charge_as_contract, int extra_waste_id,string extrawaste_type, ObservableCollection<ContainerQtyModel> container_list_picker, ObservableCollection<WasteTypePickerModel> extrawastetypelist,bool isvisibilitywastetype,string sitename,string cpid)
        //{
        //    Qty = qty;
        //    ContainerName = container_name;
        //    WasteType = waste_type;
        //    RunNo = run_no;
        //    ChargeAsContract = charge_as_contract;
        //    ContainerListPicker = container_list_picker;
        //    ExtraWasteTypeList = extrawastetypelist;
        //    ExtraWasteId = extra_waste_id;
        //    ExtraWasteType = extrawaste_type;
        //    IsVisibilityWasteType = isvisibilitywastetype;
        //    SiteName = sitename;
        //    CPID = cpid;
        //    WeightVisibility = false;
        //}
        public CustomerContainer(int qty, string container_name, string waste_type, int weight, int run_no, bool completed,bool charge_as_contract,string customer_id,int extra_waste_id, string extrawaste_type, ObservableCollection<ContainerQtyModel> container_list_picker, ObservableCollection<WasteTypePickerModel> extrawastetypelist,bool isvisibilitywastetype,string sitename,string cpid)
        {
            Qty = qty;
            ContainerName = container_name;
            WasteType = waste_type;
            Weight_T = weight;
            RunNo = run_no;
            Completed = completed;
           // CancelReasonID = cancel_reason_id;
            ChargeAsContract = charge_as_contract;
            ContainerListPicker = container_list_picker;
            ExtraWasteTypeList = extrawastetypelist;
            CustomerID = customer_id;
            ExtraWasteId = extra_waste_id;
            ExtraWasteType = extrawaste_type;
            IsVisibilityWasteType = isvisibilitywastetype;
            SiteName = sitename;
            CPID = cpid;
            this.WeightVisibility = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
