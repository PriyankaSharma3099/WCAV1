using System;
using System.Collections.Generic;
using System.Text;
using WCA.Views;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace WCA.ViewModels
{
    public class LoginViewModel : BaseViewModel 
    {
        public Command LoginCommand { get; }
        //public Command CompletedCommand { get; }
        //public Command ClearCommand { get; }
        //public event PropertyChangedEventHandler PropertyChanged;
        string _pin1;
        string _pin2;
        string _pin3;
        string _pin4;
        public string Pin1
        {
            get => _pin1;
            set => SetProperty(ref _pin1, value);
        }
        public string Pin2
        {
            get => _pin2;
            set => SetProperty(ref _pin2, value);
        }
        public string Pin3
        {
            get => _pin3;
            set => SetProperty(ref _pin3, value);
        }
        public string Pin4
        {
            get => _pin4;
            set
            {
                _pin4 = value;
                //CheckPIN(); 
            }
        }
        //private async void CheckPIN()
        //{
        //    // await App.Current.MainPage.DisplayAlert("Alert", "Test - " + Pin1.ToString(), "OK");
        //    // Check all numbers here and redirect to page
        //    if (Pin1 == "3" && Pin2 == "0" && Pin3 == "0" && Pin4 == "1")
        //    {

        //        //Shell.Current.GoToAsync($"//{nameof(Jobs)}");
        //        //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        //        await App.Current.MainPage.Navigation.PushAsync(new Jobs());
        //        //await Shell.Current.GoToAsync("//Views/Jobs");
        //    }
        //    else
        //    {
        //        // Set focus to Pin 1
        //        //var entry = CurrenPage.FindByName<Entry>("txtPin1");
        //        //entry.Focus();

        //    }
        //}
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            //CompletedCommand = new Command(CheckPIN);
            //ClearCommand = new Command(OnClearClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        //private void OnClearClicked(object obj)
        //{
        //    Pin1 = string.Empty;
        //    Pin2 = string.Empty;
        //    Pin3 = string.Empty;
        //    Pin4 = string.Empty;
        //}

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
