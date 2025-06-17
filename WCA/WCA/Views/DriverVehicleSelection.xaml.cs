using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverVehicleSelection : ContentPage
    {
        private DriverVehicleSelectionViewModel ViewModel
        {
            get { return (DriverVehicleSelectionViewModel)BindingContext; }
            set { BindingContext = value; }
        }
        public DriverVehicleSelection(DriverVehicleSelectionViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
        }
    }
}