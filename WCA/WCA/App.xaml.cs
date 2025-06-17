using System;
using WCA.Services;
using WCA.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Threading.Tasks;


namespace WCA
{
    public partial class App : Application
    {
        #region Signature
        private readonly Func<Stream, string, Task<bool>> saveSignatureDelegate;
        public static Task<bool> SaveSignature(Stream bitmap, string filename)
        {
            return ((App)Application.Current).saveSignatureDelegate(bitmap, filename);
        }
        #endregion
        public App(string csv_areas,string csv_driver,string csv_vehicle,string csv_wasteconfig, Func<Stream, string, Task<bool>> saveSignature,string csv_cancel_reason,string storage_path, string csv_email,string CSV_vehicle_types)
        {
            InitializeComponent();
            //PdfSharp.Xamarin.Forms.Droid.Platform.Init();
            saveSignatureDelegate = saveSignature;
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
            //MainPage = new NavigationPage(new Views.LoginPage());
          //  Globals.CsvText = csv;
            Globals.CsvText_areas = csv_areas;
            Globals.CsvText_driver = csv_driver;
            Globals.CsvText_vehicle = csv_vehicle;
            Globals.csvText_wasteconfig = csv_wasteconfig;
          //  Globals.TradeWaste_FileName = Trade_Waste_FileName + ".txt";
          //  Globals.TradeWaste_FileName_withoutext = Trade_Waste_FileName;
            Globals.csvText_cancel_reason = csv_cancel_reason;
            Globals.csvText_email = csv_email;
            Globals.CsvText_VehicleTypes = CSV_vehicle_types;
            Globals.storagePath = storage_path;
        }
        protected override void OnStart()
        {
            //var currentActivity = DependencyService.Get<ICurrentActivity>().GetCurrentActivity() ;

        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }


    }
}
