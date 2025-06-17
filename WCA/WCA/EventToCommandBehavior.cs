using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using WCA.Views;
using WCA.ViewModels;
using System.Windows.Input;

namespace WCA
{
    public class EventToCommandBehavior : Behavior<Entry>
    {
        //public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null); 
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        //public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
        //public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }
        public ICommand Command { private set; get; }

        //public object CommandParameter { ... }
        //public IValueConverter Converter { ...  }

        protected override void OnAttachedTo(Entry entry)
        {
            //entry.TextChanged += OnEntryTextChanged;
            entry.Completed += Entry_Completed;
            base.OnAttachedTo(entry);
        }
            
        private void Entry_Completed(object sender, EventArgs e)
        {
            //CheckPIN();
            if (((Entry)sender).Text == "1")
            {

                App.Current.MainPage.Navigation.PushAsync(new DriverVehicleSelection(new DriverVehicleSelectionViewModel()));
                //App.Current.MainPage.Navigation.PushAsync(new Jobs(new JobsGroupViewModel()));
            }  
            else
            {
                // Set label text
                
            }
            // Clear all 4 textboxes
            //Application.Current.Properties["Pin1"] = "";
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            //entry.TextChanged -= OnEntryTextChanged;
            entry.Completed += Entry_Completed;
            base.OnDetachingFrom(entry);
        }

        //void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        //{
        //    double result;
        //    bool isValid = double.TryParse(args.NewTextValue, out result);
        //    ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        //}
    }
}
