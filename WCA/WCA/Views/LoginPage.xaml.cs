using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private void Pin1_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPin2.Focus();
        }

        private void Pin2_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPin3.Focus();
        }

        private void Pin3_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPin4.Focus();
        }

        private void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != "")
            {
                //txtPin1.Focus();
                //txtPin1.IsEnabled = false;
            }
        }

        private void txtPin4_Completed(object sender, EventArgs e)
        {
            if (txtPin1.Text != "3" && txtPin2.Text != "0" && txtPin3.Text != "0" && txtPin4.Text != "1")
            {
                lblIncorrect.Text = "Incorrect PIN";
                lblEnter.Text = "";
            }
            else if (!Directory.Exists(Globals.storagePath+"/WCA"))
            {
                lblIncorrect.Text = "Storage Permission Denied";
                ((Entry)sender).Text = "0";
            }
            else if (!Directory.Exists(Globals.storagePath)) {
                lblIncorrect.Text = "Storage Path Not Found";
                ((Entry)sender).Text = "0";
            }                                
            else if (Globals.CsvText_areas == null || Globals.CsvText_driver == null || Globals.CsvText_vehicle == null )
            {
                lblIncorrect.Text = "Required Data Files Not Found";
                ((Entry)sender).Text = "0";
            }
            else if (Globals.csvText_email == null)
            {
                lblIncorrect.Text = "Email.txt File Not Found";
                ((Entry)sender).Text = "0";
            }
            else {
                //  var pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/WCA";
                var pathToNewFolder = Globals.storagePath + "/WCA";
                if (Directory.Exists(pathToNewFolder))
                {
                    if (File.Exists(pathToNewFolder + "/Vehicles.txt"))
                    {
                        Globals.CsvText_vehicle = File.ReadAllText(pathToNewFolder + '/' + "Vehicles.txt");
                    }
                    if (File.Exists(pathToNewFolder + "/Areas.txt"))
                    {
                        Globals.CsvText_areas = File.ReadAllText(pathToNewFolder + '/' + "Areas.txt");
                    }
                }
            }
            txtPin4.Unfocus();
        }
    }
}