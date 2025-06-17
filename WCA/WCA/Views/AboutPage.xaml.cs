using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WCA.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
    }
}
/*
    Avoid enabling and disabling UI elements in the code-behind. Ensure that view models are responsible for defining logical state changes 
    that affect some aspects of the view's display, such as whether a command is available, or an indication that an operation is pending. 
    Therefore, enable and disable UI elements by binding to view model properties, rather than enabling and disabling them in code-behind.
 */