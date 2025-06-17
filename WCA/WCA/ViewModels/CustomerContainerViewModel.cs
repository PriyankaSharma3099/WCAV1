using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WCA.Models;
using WCA.Views;
using Xamarin.Forms;

namespace WCA.ViewModels
{
    public class CustomerContainerViewModel : BaseViewModel
    {
        private CustomerContainer _customer_container;
        public CustomerContainer CustomerContainer
        {
            get => _customer_container;
        }

        public CustomerContainerViewModel(CustomerContainer customer_container)
        {
            this._customer_container = customer_container;
            this._customer_container.PropertyChanged += CustomerContainer_PropertyChanged;
        }

        private void CustomerContainer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerContainer.WeightVisibility))
            {
                OnPropertyChanged(nameof(WeightVisibility));
            }
            // Optional: If Weight_T could also be changed directly in the model
            // by some other process AND the ViewModel needs to be notified,
            // add a similar check for Weight_T.
            // if (e.PropertyName == nameof(CustomerContainer.Weight_T))
            // {
            //     OnPropertyChanged(nameof(Weight_T));
            // }
        }

        public string ContainerName { get { return _customer_container.ContainerName; } }
        public int Qty { get { return _customer_container.Qty; } }
        public int ExtraWasteId { get { return _customer_container.ExtraWasteId; } }
        public string ExtraWasteType { get { return _customer_container.ExtraWasteType; } }
        public string WasteType { get { return _customer_container.WasteType; } }
 /*       public int Weight { get { return _customer_container.Weight; }
            set
            {
                _customer_container.Weight = value;
                OnPropertyChanged("Email");
            }
        }*/
        public int RunNo { get { return _customer_container.RunNo; } }
        public string CustomerID { get { return _customer_container.CustomerID; } }

        public int Weight_T
        {
            get { return _customer_container.Weight_T; }
            set
            {
                if (_customer_container.Weight_T != value)
                {
                    _customer_container.Weight_T = value;
                    OnPropertyChanged(nameof(Weight_T)); // Ensure BaseViewModel or this class has OnPropertyChanged
                }
            }
        }

        public bool WeightVisibility
        {
            get { return _customer_container.WeightVisibility; }
            set
            {
                if (_customer_container.WeightVisibility != value)
                {
                    _customer_container.WeightVisibility = value;
                    OnPropertyChanged(nameof(WeightVisibility)); // Ensure BaseViewModel or this class has OnPropertyChanged
                }
            }
        }

        public bool ChargeAsContract
        {
            get { return _customer_container.ChargeAsContract; }
            set
            {
                if (CustomerContainer.ChargeAsContract == value) return;
                CustomerContainer.ChargeAsContract = value;
                Globals.ContainerSelectedValue = Qty.ToString();
                /*  CustomerContainer.ContainerListPicker.Clear();
                  Globals.ContainerSelectedValue = Qty.ToString();
                  ObservableCollection<int> tempContainer = new ObservableCollection<int>();
                  int Quantity_read = Qty;
                  bool Charge_As_Contract_read = ChargeAsContract;
                  int Starting_Index = 0;
                  if (value == false)
                  {
                      Starting_Index = 0;

                  }
                  else
                  {
                      Starting_Index = Quantity_read;
                  }
                  for (int index = Starting_Index; index <= 25; index++)
                  {
                      CustomerContainer.ContainerListPicker.Add(index);
                  }*/

            }
        }
        public bool Completed { get { return _customer_container.Completed; } }

        public bool IsVisibilityWasteType {
            get{
                return _customer_container.IsVisibilityWasteType;
            }
        }

        // Quantity picker list
        public ObservableCollection<ContainerQtyModel> ContainerListPicker
        {
            get { return _customer_container.ContainerListPicker; }
            set
            {
                ContainerListPicker = value;
            }
        }

        // extra waste picker list
        public ObservableCollection<WasteTypePickerModel> ExtraWasteTypeList
        {
            get { return _customer_container.ExtraWasteTypeList; }
            set
            {
                ExtraWasteTypeList = value;
            }
        }

    }
}
