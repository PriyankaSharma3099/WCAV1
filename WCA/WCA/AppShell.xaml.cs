using System;
using System.Collections.Generic;
using WCA.ViewModels;
using WCA.Views;
using Xamarin.Forms;

namespace WCA
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(JobDetailPage), typeof(JobDetailPage));
            Routing.RegisterRoute(nameof(NewJobPage), typeof(NewJobPage));
            //Routing.RegisterRoute(nameof(Jobs), typeof(Jobs));
        }

    }
}
