using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WCA.Models;
using WCA.ViewModels;
using Xamarin.Forms;

namespace WCA
{
    public class PickerToCommandBehavior : Behavior<Picker>
    {
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
       
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }
        public ICommand Command { private set; get; }

        protected override void OnAttachedTo(Picker picker)
        {
            picker.SelectedIndexChanged += Picker_Changed;
            base.OnAttachedTo(picker);
        }

        private void Picker_Changed(object sender, EventArgs e)
        {
            try
            {
                var pickerVal = ((Picker)sender).Items[((Picker)sender).SelectedIndex];
                if (((Picker)sender).Title == "Select Driver")
                {
                    Globals.DriverSelectedIndex = Convert.ToInt32(((Driver_DataModel)((Picker)sender).SelectedItem).Id);
                    Globals.DriverSelectedValue = pickerVal;
                }
                else if (((Picker)sender).Title == "Select Vehicle")
                {
                    Globals.VehicleSelectedIndex = Convert.ToInt32(((Vehicle_DataModel)((Picker)sender).SelectedItem).Id);
                    Globals.VehicleSelectedValue = pickerVal;
                    Globals.Vehicle_Type = Convert.ToInt32(((Vehicle_DataModel)((Picker)sender).SelectedItem).Vehicle_Type);
                }
                else if (((Picker)sender).Title == "Select Vehicle Type")
                {
                    Globals.Vehicle_TypesSelectedIndex = Convert.ToInt32(((VehicleTypes_DataModel)((Picker)sender).SelectedItem).Id);
                    Globals.Vehicle_TypesSelectedValue = pickerVal;
                }
                else if (((Picker)sender).Title == "Select Area Type")
                {
                    Globals.AreaSelectedIndex = Convert.ToInt32(((Areas_DataModel)((Picker)sender).SelectedItem).Id);
                    Globals.AreaSelectedValue = pickerVal;
                }
                else
                {
                    
                    // update Qty in tradewaste file
                   // wastetype_qty_update(sender);

                }
                }
            catch (Exception ex)
            {
              //  Globals.ContainerSelectedValue = null;
            }
        }
  
        protected override void OnDetachingFrom(Picker picker)
        {
            picker.SelectedIndexChanged += Picker_Changed;
            base.OnDetachingFrom(picker);
        }
    }
}

