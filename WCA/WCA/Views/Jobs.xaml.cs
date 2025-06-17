    using System;
using System.Collections.Generic;
using WCA.ViewModels;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace WCA.Views
{
    public partial class Jobs : ContentPage
    {
        Picker ExtraWastePicker;
        private JobsGroupViewModel ViewModel
        {
            get { return (JobsGroupViewModel)BindingContext; }
            set { BindingContext = value; }
        }

        private bool CheckForTomorrowJobClick = true;

        ToolbarItem roundCompleteToolbarItem;

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (ViewModel.Items.Count == 0)
                {
                    ViewModel.LoadJobsCommand.Execute(null);
                    Title = "Jobs (" + ViewModel.Items.Count.ToString() + ")"; 
                }

                int count = ViewModel.Items.Count();
                roundCompleteToolbarItem = new ToolbarItem
                {
                    Text = "Round Complete",
                    Order = ToolbarItemOrder.Primary,
                    Priority = 0,
                    Command = new Command(() => Round_Complete()) 
                };

                if (count == 0)
                {
                    if (!ToolbarItems.Contains(roundCompleteToolbarItem))
                        ToolbarItems.Add(roundCompleteToolbarItem);
                }


            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
        }
        
        public Jobs(JobsGroupViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            this.BackgroundColor = Color.Yellow;
        }
        private async void onAboutClicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage()); // pass ViewModel object to next page

        }

        private void containerQty_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Page currentPage = Navigation.NavigationStack.LastOrDefault();
            //Picker extrawastepicker = currentPage.FindByName<Picker>("containerExtrawaste");
            //extrawastepicker.IsVisible = true;
            this.ViewModel.PickerValueUpdated(sender);
        }      
        private void pickerWasteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ViewModel.WasteTypePickerValueUpdated(sender);
        } 
        private void pickerCancelJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ViewModel.CancelReasonPickerValueUpdated(sender);
        }
        private async void Round_Complete()
        {
            bool imageFrom = await DisplayAlert("Round Complete", "Do you want to Complete Today's Round ?", "Yes", "No");
            if (imageFrom)
            {
                if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName))
                {
                    string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    // File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName);
                    if (File.Exists(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName_withoutext + "_processed.txt"))
                    {
                         File.Delete(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName_withoutext + "_processed.txt");
                    }
                    File.AppendAllLines(Globals.storagePath + "/WCA/" + Globals.TradeWaste_FileName_withoutext + "_processed.txt", lines);
                    await Application.Current.MainPage.DisplayAlert("Round Complete", "Round Complete File Generated Successfully !", "OK");

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");

                }

            }
         
        }

        async void ButtonTomorrowJobs_Clicked(System.Object sender, System.EventArgs e)
        {
            if (CheckForTomorrowJobClick)
            {
                CheckForTomorrowJobClick = false;
                if (Globals.CsvText_TomorrowJobs == null)
                {

                    string date_time = DateTime.Now.AddDays(1).ToString("yyyyMMdd");  // for get current date from device
                                                                                      //   string date_time = "20220714"; // hardcoded date for testing purpose

                    string TradeWasteFileName_temp = Globals.AreaSelectedValue + "_" + date_time;
                    if (File.Exists(Globals.storagePath + "/WCA/" + TradeWasteFileName_temp + ".txt"))
                    {
                        Globals.CsvText_TomorrowJobs = File.ReadAllText(Globals.storagePath + "/WCA/" + TradeWasteFileName_temp + ".txt"); // for read file from Internal Directory 

                        await Application.Current.MainPage.Navigation.PushAsync(new TomorrowJobs()); // pass ViewModel object to next page
                        CheckForTomorrowJobClick = true;

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Tomorrow's Jobs(" + TradeWasteFileName_temp + ".txt" + ") File Not Found !", "OK");
                        CheckForTomorrowJobClick = true;

                    }
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new TomorrowJobs()); // pass ViewModel object to next page
                    CheckForTomorrowJobClick = true;
                }
            }
        }
    }

   
}