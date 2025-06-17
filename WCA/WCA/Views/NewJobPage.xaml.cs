using System;
using System.Collections.Generic;
using System.ComponentModel;
using WCA.Models;
using WCA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WCA.Views
{
    public partial class NewJobPage : ContentPage
    {
        public Job Job { get; set; }

        public NewJobPage()
        {
            InitializeComponent();
            BindingContext = new NewJobsViewModel();
        }
    }
}