using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WCA.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
/*
    Keep the UI responsive with asynchronous operations. Mobile apps should keep the UI thread unblocked to improve the user's perception of performance. 
    Therefore, in the view model, use asynchronous methods for I/O operations and raise events to asynchronously notify views of property changes
 */
/*
    Keep view models and views independent. The binding of views to a property in a data source should be the view's principal dependency on its corresponding view model. 
    Specifically, don't reference view types, such as Button and ListView, from view models. By following the principles outlined here, view models can be tested in isolation, 
    therefore reducing the likelihood of software defects by limiting scope.

 */