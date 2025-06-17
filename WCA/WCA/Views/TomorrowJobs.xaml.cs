using System;
using System.Collections.Generic;
using WCA.ViewModels;
using Xamarin.Forms;

namespace WCA.Views
{
    public partial class TomorrowJobs : ContentPage
    {
        public TomorrowJobs()
        {
            InitializeComponent();
            this.BindingContext = new TomorrowJobsViewModel();

        }
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            this.BackgroundColor = Color.Yellow;
        }

        private TomorrowJobsViewModel ViewModel
        {
            get { return (TomorrowJobsViewModel)BindingContext; }
            set { BindingContext = value; }
        }


        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (ViewModel.Items.Count == 0)
                {
                    ViewModel.LoadJobsCommand.Execute(null);
                    Title = "Tomorrow's Jobs (" + ViewModel.Items.Count.ToString() + ")";
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }
    }
}
