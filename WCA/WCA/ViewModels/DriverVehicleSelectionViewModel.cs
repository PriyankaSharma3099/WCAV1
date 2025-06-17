using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net.Mail;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using WCA.Models;
using WCA.Views;
using Xamarin.Forms;
using static Microsoft.Graph.GeneratedErrorConstants;
using Command = Xamarin.Forms.Command;
using File = System.IO.File;
using Application = Xamarin.Forms.Application;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;
using System.Runtime.InteropServices;
using WCA;
using Org.BouncyCastle.Cms;
using Newtonsoft.Json;
using System.Net.Http;

namespace WCA.ViewModels
{
    public class DriverVehicleSelectionViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public DriverVehicleSelectionViewModel(bool expanded = false)
        {
            isVisible = false; //hide vehicle registration row bydefault
            isVisible_area = false; //hide Area add row bydefault
            Globals.DriverSelectedIndex = 0;
            Globals.VehicleSelectedIndex = 0;
            Globals.AreaSelectedIndex = 0;
            Globals.Vehicle_TypesSelectedIndex = 0;
            Globals.Vehicle_Type = 0;
            if (Globals.CsvText_driver != null)
            {
                driver_list_dataBind();
            }
            if (Globals.CsvText_vehicle != null)
            {
                vehicle_list_dataBind();
            }
            if (Globals.CsvText_areas != null)
            {
                areas_list_dataBind();
            }
            if (Globals.CsvText_VehicleTypes != null)
            {
                vehicleTypes_list_dataBind();
            }

            Next_button_click_Command = new Command(Next_button_click); // next button click
            Save_button_click_Command = new Command(Save_button_click); // vehicle name save button
            Area_Save_button_click_Command = new Command(Area_Save_button_click ); // vehicle name save button
            OnClick_Add_Vehicle = new Command(Click_Add_Vehicle); // add vehicle label click
            OnClick_Add_Area = new Command(Click_Add_Area); // add area label click


        }
        public Command Next_button_click_Command { get; }
        public Command Save_button_click_Command { get; } // vehicle save button
        public Command Area_Save_button_click_Command { get; } // Area save button 
        public Command OnClick_Add_Vehicle { get; }
        public Command OnClick_Add_Area { get; }


        string _vehicle_name_text_entry;
        public string vehicle_name_text_entry
        {
            get => _vehicle_name_text_entry;
            set => SetProperty(ref _vehicle_name_text_entry, value);
        }

        string _area_name_text_entry;
        public string area_name_text_entry
        {
            get => _area_name_text_entry;
            set => SetProperty(ref _area_name_text_entry, value);
        }

        private ObservableCollection<Driver_DataModel> _driverLists = new ObservableCollection<Driver_DataModel>();
        public ObservableCollection<Driver_DataModel> DriverLists
        {
            get { return _driverLists; }
            set
            {
                _driverLists = value;
            }
        }

        private ObservableCollection<Areas_DataModel> _areasLists = new ObservableCollection<Areas_DataModel>();
        private ObservableCollection<Vehicle_DataModel> _vehicleLists = new ObservableCollection<Vehicle_DataModel>();
        public ObservableCollection<Vehicle_DataModel> VehicleLists
        {
            get { return _vehicleLists; }
            set
            {
                _vehicleLists = value;
            }
        }
        private ObservableCollection<VehicleTypes_DataModel> _vehicletypesLists = new ObservableCollection<VehicleTypes_DataModel>();
        public ObservableCollection<VehicleTypes_DataModel> VehicleTypesLists
        {
            get { return _vehicletypesLists; }
            set
            {
                _vehicletypesLists = value;
            }
        }
        public ObservableCollection<Areas_DataModel> AreasLists
        {
            get { return _areasLists; }
            set
            {
                _areasLists = value;
            }
        }

        private ObservableCollection<TradeWasteConfig_DataModel> _wasteconfigLists = new ObservableCollection<TradeWasteConfig_DataModel>();
        public ObservableCollection<TradeWasteConfig_DataModel> WasteconfigLists
        {
            get { return _wasteconfigLists; }
            set
            {
                _wasteconfigLists = value;
            }
        }

        private bool isvisible;

        public bool isVisible
        {
            get
            {
                return isvisible;
            }

            set
            {
                if (isvisible != value)
                {
                    isvisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isvisible_area;

        public bool isVisible_area
        {
            get
            {
                return isvisible_area;
            }

            set
            {
                if (isvisible_area != value)
                {
                    isvisible_area = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Click_Add_Vehicle()
        {
            isVisible = !isVisible;
        }
        public void Click_Add_Area() {
            isVisible_area = !isVisible_area;
            //SendEmail();
        }


        public async void Next_button_click()
        {
            //Console.WriteLine("selected vehicle type at Driver Vehicle Selection screen =>" + Globals.Vehicle_TypesSelectedIndex);

            if (Globals.DriverSelectedIndex != 0 && Globals.VehicleSelectedIndex != 0 && Globals.AreaSelectedIndex !=0)
            {
                if (Globals.csvText_wasteconfig != null)
                {
                    wasteconfig_list_dataBind(); // update driver and vehiche id to wasteconfig file
                }
                
                string date_time = DateTime.Now.ToString("yyyyMMdd");  // for get current date from device
           //   string date_time = "20220714"; // hardcoded date for testing purpose

                string TradeWasteFileName_temp = Globals.AreaSelectedValue+"_" + date_time;
                
                if (File.Exists(Globals.storagePath + "/WCA/" + TradeWasteFileName_temp + ".txt"))
                {
                    //     CSV_String = ReadFile(TradeWasteFileName_temp+".txt"); // for read file from asset folder

                        Globals.CsvText = File.ReadAllText(Globals.storagePath + "/WCA/" + TradeWasteFileName_temp + ".txt"); // for read file from Internal Directory 
                        Globals.TradeWaste_FileName = TradeWasteFileName_temp + ".txt";
                        Globals.TradeWaste_FileName_withoutext = TradeWasteFileName_temp;
                        await App.Current.MainPage.Navigation.PushAsync(new Jobs(new JobsGroupViewModel())); // navigate to Jobs screen
              
            }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", TradeWasteFileName_temp + ".txt file not found !", "OK");
                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Please Select Driver,Vehicle and Area Name !", "OK");
            }
        }

        // add vehicle in vehicles.txt file
        public void Save_button_click(Object obj)
        {
            String temp_vehicle_no = vehicle_name_text_entry;
            if (vehicle_name_text_entry != null && vehicle_name_text_entry.Trim().Length > 0 && vehicle_name_text_entry.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
            {
                if (Globals.Vehicle_TypesSelectedIndex != 0)
                {
                    File.AppendAllText(Globals.storagePath + "/WCA/Vehicles.txt", "\r\n-1\t" + temp_vehicle_no + "\t" + Globals.Vehicle_TypesSelectedIndex.ToString()); // need to add vehicle type instead of "1"
                    VehicleLists.Add(new Vehicle_DataModel("-1", temp_vehicle_no, Globals.Vehicle_TypesSelectedIndex.ToString())); // need to add vehicle type instead of "1"
                    vehicle_name_text_entry = null;
                    isVisible = false;
                }
                else {
                    Application.Current.MainPage.DisplayAlert("Alert", "Please Select Vehicle Type", "OK");
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please Enter Valid Vehicle Name", "OK");

            }

        }

        // add Area in Areas.txt file
        public void Area_Save_button_click(Object obj)
        {
            if (area_name_text_entry != null && area_name_text_entry.Trim().Length > 0 && area_name_text_entry.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
            {
                String temp_area_no = area_name_text_entry.Trim();
                File.AppendAllText(Globals.storagePath + "/WCA/Areas.txt", "\r\n-1\t" + temp_area_no);
                AreasLists.Add(new Areas_DataModel("-1", temp_area_no));
                area_name_text_entry = null;
                isVisible_area = false;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please Enter Valid Area Name", "OK");

            }

        }

        // Bind data to driver list
        private void driver_list_dataBind()
        {
            int cols = 1;          // Columns in CSV
            string[] lines = Globals.CsvText_driver.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
            int lineCount = lines.Length;
            string[,] data = new string[lineCount, cols];

            for (int i = 0; i < lineCount; i++)
            {
                var values = lines[i].Split('|');
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = values[j];
                }
            }
            // Iterate through CSV lines
            for (int i = 1; i < lineCount; i++)
            {
                if (i < lineCount)
                {
                    //Append current record to temp array
                    for (int j = 0; j < cols; j++)
                    {
                        DriverLists.Add(new Driver_DataModel(data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0], data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1]));
                    }
                }
                Console.WriteLine(i);
            }
        }
        // Bind data to vehicleTypes list
        private void vehicleTypes_list_dataBind()
        {
            int cols = 1;          // Columns in CSV
            string[] lines = Globals.CsvText_VehicleTypes.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
            int lineCount = lines.Length;
            string[,] data = new string[lineCount, cols];

            for (int i = 0; i < lineCount; i++)
            {
                var values = lines[i].Split('|');
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = values[j];
                }
            }
            // Iterate through CSV lines
            for (int i = 1; i < lineCount; i++)
            {
                if (i < lineCount)
                {
                    //Append current record to temp array
                    for (int j = 0; j < cols; j++)
                    {
                        VehicleTypesLists.Add(new VehicleTypes_DataModel(data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0], data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1]));
                    }
                }
                Console.WriteLine(i);
            }
        }

        // Bind data to vehicle list
        private void vehicle_list_dataBind()
        {
            int cols = 1;          // Columns in CSV
            string[] lines = Globals.CsvText_vehicle.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
            int lineCount = lines.Length;
            string[,] data = new string[lineCount, cols];

            for (int i = 0; i < lineCount; i++)
            {
                var values = lines[i].Split('|');
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = values[j];
                }
            }
            // Iterate through CSV lines
            for (int i = 1; i < lineCount; i++)
            {
                if (i < lineCount)
                {
                    //Append current record to temp array
                    for (int j = 0; j < cols; j++)
                    {
                        VehicleLists.Add(new Vehicle_DataModel(data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0], data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1], data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[2]));
                    }
                }
            }
        }

        private void areas_list_dataBind()
        {
            int cols = 1;          // Columns in CSV
            string[] lines = Globals.CsvText_areas.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries); ;
            int lineCount = lines.Length;
            string[,] data = new string[lineCount, cols];

            for (int i = 0; i < lineCount; i++)
            {
                var values = lines[i].Split('|');
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = values[j];
                }
            }
            // Iterate through CSV lines
            for (int i = 1; i < lineCount; i++)
            {
                if (i < lineCount)
                {
                    //Append current record to temp array
                    for (int j = 0; j < cols; j++)
                    {
                        AreasLists.Add(new Areas_DataModel(data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[0], data[i, j].Split(new[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries)[1]));
                    }
                }
            }
        }

        // update driver id and vehicle id in wasteconfig file
        private void wasteconfig_list_dataBind()
        {
            if (File.Exists(Globals.storagePath + "/WCA/TradeWasteConfig.txt"))
            {
                List<TradeWasteConfig_DataModel> list = new List<TradeWasteConfig_DataModel>();
                string[] lines = File.ReadAllLines(Globals.storagePath + "/WCA/TradeWasteConfig.txt");
                for (int i = 0; i < lines.Count() / 4; i++)
                {
                    if (lines[i * 4] != string.Empty || lines[i * 4 + 1] != string.Empty || lines[i * 4 + 2] != string.Empty || lines[i * 4 + 3] != string.Empty)
                    {
                        list.Add(new TradeWasteConfig_DataModel { TicketStart = lines[i * 4], Date = lines[i * 4 + 1], DriverID = lines[i * 4 + 2], VehicleID = lines[i * 4 + 3] });
                    }
                }
                TradeWasteConfig_DataModel p = list.Where(i => i.DriverID != null && i.VehicleID != null).FirstOrDefault();
                if (p != null)
                {
                    p.DriverID = p.DriverID.Substring(0, 9) + Globals.DriverSelectedIndex;
                    p.VehicleID = p.VehicleID.Substring(0, 10) + Globals.VehicleSelectedIndex;
                    File.Delete(Globals.storagePath + "/WCA/TradeWasteConfig.txt");
                    foreach (var item in list)
                    {
                        File.AppendAllLines(Globals.storagePath + "/WCA/TradeWasteConfig.txt", new string[] { item.TicketStart, item.Date, item.DriverID, item.VehicleID });
                    }
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "File Not Found for Update", "OK");
            }
        }


        //public async Task SendEmail()
        //{
        //    string ClientId = "59d0bbbd-1ee5-4c80-9d98-8c7d403d2399"; 
        //    string TenantId = "36f4c3be-b3b9-4602-abe3-5301c9544cbf"; 
        //string Scopes = "https://graph.microsoft.com/.default";
        //    string FromEmailAddress = "tickets@ismwaste.co.uk"; //"ISMtickets@frog.co.uk"
        //string toEmail = "dps.microlentsystems@gmail.com";
        //string subject = "Test Mail form WCA with Frog IT Support";
        //string body = "Test Body";
        //string[] GraphScopes = { "https://graph.microsoft.com/.default" };

        //    try
        //    {
        //        var app = PublicClientApplicationBuilder
        //   .Create(ClientId)
        //   .WithAuthority($"https://login.microsoftonline.com/{TenantId}")
        //   .WithRedirectUri("msauth://com.adhsoftware.wca/aGx1L%2FqmY3MxRzXqZq1at3qI6bY%3D")
        //   .WithParentActivityOrWindow(DependencyService.Get<ICurrentActivity>().GetCurrentActivity)
        //   .Build();

        //    var accounts = await app.GetAccountsAsync();
        //    IAccount account = accounts.FirstOrDefault();

        //    AuthenticationResult result;
        //    if (account != null)
        //    {
        //        result = await app.AcquireTokenSilent(new[] { Scopes }, account).ExecuteAsync();
        //    }
        //    else
        //    {
        //        result = await app.AcquireTokenInteractive(new[] { Scopes }).WithParentActivityOrWindow(DependencyService.Get<ICurrentActivity>().GetCurrentActivity()) // GetCurrentActivity
        //     .ExecuteAsync();
        //    }

        //    var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
        //    {
        //        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        //        return Task.CompletedTask;
        //    }));

        //    var email = new Message
        //    {
        //        Subject = subject,
        //        Body = new ItemBody
        //        {
        //            ContentType = BodyType.Text,
        //            Content = body
        //        },
        //        ToRecipients = new List<Recipient>()
        //        {
        //            new Recipient
        //            {
        //                EmailAddress = new EmailAddress
        //                {
        //                    Address = toEmail
        //                }
        //            }
        //        },
        //        From = new Recipient
        //        {
        //            EmailAddress = new EmailAddress
        //            {
        //                Address = FromEmailAddress
        //            }
        //        }
        //    };
        //    await graphClient.Users[FromEmailAddress].SendMail(email, true).Request().PostAsync();
            
        //    }
        //    catch (Exception ex)
        //    {
        //        Application.Current.MainPage.DisplayAlert("Alert", ex.ToString(), "OK");

        //    }
        //}

        }
}