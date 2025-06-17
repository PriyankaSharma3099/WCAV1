using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WCA.Models;
using Xamarin.Forms;

namespace WCA.ViewModels
{
    public class NewJobsViewModel : BaseViewModel
    {
        private string text;
        private string description;

        public NewJobsViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Job newJob = new Job()
            {
                Id = Guid.NewGuid().ToString(),
                Text = Text,
                Description = Description
            };

            await DataStore.AddJobAsync(newJob);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
