using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Plugin.Media;
using Java.Nio.FileNio;
using System.Linq;

namespace WCA.Droid
{
    [Activity(Label = "WCA", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
       // public string CSV_String { get; private set; }
        public string CSV_Areas { get; private set; }
        public string CSV_String_driver { get; private set; }
        public string CSV_String_vehicle { get; private set; }
        public string CSV_String_tradewasteconfig { get; private set; }
        public string CSV_String_cancelresson { get; private set; }
        public string CSV_String_email { get; private set; }
        public string CSV_String_vehicle_types { get; private set; }
        public static Activity CurrentActivity { get; private set; }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            PdfSharp.Xamarin.Forms.Droid.Platform.Init();
            await CrossMedia.Current.Initialize();
            Instance = this; // Store a reference to the MainActivity

           /*    Android.Support.V4.App.ActivityCompat.RequestPermissions(this,
                        new String[] { Android.Manifest.Permission.WriteExternalStorage,Android.Manifest.Permission.ReadExternalStorage },
                        1);

                Directory.CreateDirectory("/storage/external-1/test_dir1");*/

            // string storagePath = "";

            string storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
           
            var status_write = await Permissions.RequestAsync<Permissions.StorageWrite>();


            
           // String procMounts = File.ReadAllText(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath+"/TextFile1.txt");
            // for get SDCard Id from mounted storage devices file
          /*  String procMounts = System.IO.File.ReadAllText("/proc/mounts");

                    var candidateProcMountEntries = procMounts.Split('\n', '\r').ToList();
                    candidateProcMountEntries.RemoveAll(s => (s.IndexOf("/storage/", StringComparison.OrdinalIgnoreCase) < 0) || s.Contains("emulated") || s.Contains("tmpfs"));
                    if (candidateProcMountEntries != null && candidateProcMountEntries.Count > 0)
                    {
                        // sd card found
                        string sdCardString = candidateProcMountEntries[0].ToString();
                        String temp1 = "";
                        String temp2 = "";

                        for (int i = 1; i <= countChar(sdCardString, '/'); i++)
                        {
                            String temp = "/" + sdCardString.Split("/")[i] + "/";
                            if (temp.Equals("/storage/"))
                            {
                                temp1 = sdCardString.Split("/")[i];
                                temp2 = sdCardString.Split("/")[i + 1];
                            }
                        }

                        storagePath = secondString(temp2, ' ');
                    }*/
     

            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                var permission_request = await Permissions.RequestAsync<Permissions.StorageWrite>();

                 Permission_accept_continueAsync(savedInstanceState,storagePath);

            }

        }

        protected override void OnResume()
        {
            base.OnResume();
            CurrentActivity = this;
        }

        protected override void OnPause()
        {
            base.OnPause();
            CurrentActivity = null;
        }

        /*    public static int countChar(String str, char c)
            {
                int count = 0;

                for (int i = 0; i < str.Count(); i++)
                {
                    if (str.ElementAt(i) == c)
                        count++;
                }

                return count;
            }

            public static String secondString(String str, char c)
            {
                String temp = "/storage/";

                for (int i = 0; i < str.Count(); i++)
                {
                    if (str.ElementAt(i) != c)
                    {
                        temp += str.ElementAt(i) + "";
                    }
                    else
                    {
                        break;
                    }

                }
                return temp;
            }*/

        private async Task Permission_accept_continueAsync(Bundle savedInstanceState,string storage_path)
        {
           // var path_internal = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath; // internal storage path

            var status_write = await Permissions.RequestAsync<Permissions.StorageWrite>();

         //     string date_time = DateTime.Now.ToString("yyyyMMdd");  // for get current date from device
         ////   string date_time = "20220714"; // hardcoded date for testing purpose

         //   string TradeWasteFileName_temp = "TradeWaste_" + date_time ;

            if (status_write != PermissionStatus.Granted)
            {

                var status_write_local = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
            else
            {

                /*    String procMounts = "Mounted Devices file not found on device !";
                    var path_internal = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath; // internal storage path
                    if (!File.Exists("/proc/mounts"))
                    {
                        File.AppendAllText(path_internal + "/WCA_Logfile.txt", procMounts);  // for export log file in internal storage
                    }
                    else
                    {
                        procMounts = System.IO.File.ReadAllText("/proc/mounts");
                        File.AppendAllText(path_internal + "/WCA_Logfile.txt", procMounts);  // for export log file in internal storage

                    }
    */


                try {
                    if (Directory.Exists(storage_path))
                    {
                        var pathToWCADirectory = storage_path + "/WCA";
                        //  var pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/WCA";
                        Directory.CreateDirectory(pathToWCADirectory);
                        Directory.CreateDirectory(pathToWCADirectory + "/images");
                        Directory.CreateDirectory(pathToWCADirectory + "/Signatures");
                        Directory.CreateDirectory(pathToWCADirectory + "/Tickets");
                        //if (File.Exists(pathToWCADirectory + '/' + TradeWasteFileName_temp+".txt"))
                        //{
                        //    //     CSV_String = ReadFile(TradeWasteFileName_temp+".txt"); // for read file from asset folder
                        //    CSV_String = File.ReadAllText(pathToWCADirectory + '/' + TradeWasteFileName_temp + ".txt"); // for read file from Internal Directory 
                        //}
                        if(File.Exists(pathToWCADirectory + "/Areas.txt"))
                        {
                            CSV_Areas = File.ReadAllText(pathToWCADirectory+"/Areas.txt");
                        }
                        if (File.Exists(pathToWCADirectory + "/Drivers.txt"))
                        {
                            CSV_String_driver = File.ReadAllText(pathToWCADirectory + '/' + "Drivers.txt");
                        }
                        if (File.Exists(pathToWCADirectory + "/Vehicles.txt"))
                        {
                            CSV_String_vehicle = File.ReadAllText(pathToWCADirectory + '/' + "Vehicles.txt");
                        }
                        if (File.Exists(pathToWCADirectory + "/TradeWasteConfig.txt"))
                        {
                            CSV_String_tradewasteconfig = File.ReadAllText(pathToWCADirectory + '/' + "TradeWasteConfig.txt");
                        }
                        if (File.Exists(pathToWCADirectory + "/CancelReasons.txt"))
                        {
                            CSV_String_cancelresson = File.ReadAllText(pathToWCADirectory + '/' + "CancelReasons.txt");
                        }
                        if (File.Exists(pathToWCADirectory + "/Email.txt"))
                        {
                            CSV_String_email = File.ReadAllText(pathToWCADirectory + '/' + "Email.txt");
                        }
                        if (File.Exists(pathToWCADirectory + "/VehicleTypes.txt"))
                        {
                            CSV_String_vehicle_types = File.ReadAllText(pathToWCADirectory + '/' + "VehicleTypes.txt");
                        }
                        // check internet connection
                        /*   var current = Connectivity.NetworkAccess;

                           if (current == NetworkAccess.Internet)
                           {
                               // Connection to internet is available
                           }*/

                        // string TicketHtml = ReadFile("Tickethtml.txt");

                        Task.Run(async () => { await DeployTicketTemplateFromAssetsAsync("sign_img.png", storage_path); });
                        Task.Run(async () => { await DeployTicketTemplateFromAssetsAsync("top_bg.png", storage_path); });
                        Task.Run(async () => { await DeployTicketTemplateFromAssetsAsync("center_img.png", storage_path); });
                        Task.Run(async () => { await DeployTicketTemplateFromAssetsAsync("tick_icon.png", storage_path); });
                    }
                }
                catch (Exception ex) {


                }
                    // check internet connection
                    /*   var current = Connectivity.NetworkAccess;

                       if (current == NetworkAccess.Internet)
                       {
                           // Connection to internet is available
                       }*/

            }

            LoadApplication(new App(CSV_Areas, CSV_String_driver, CSV_String_vehicle, CSV_String_tradewasteconfig, OnSaveSignature, CSV_String_cancelresson,storage_path, CSV_String_email, CSV_String_vehicle_types));
           // Task.Run(async () => { await DeployTicketTemplateFromAssetsAsync(); });
        }
        //private Activity returnActivity() {
        //    return CurrentActivity;
        //}
        private async Task<bool> OnSaveSignature(Stream bitmap, string filename)
        {
           // var pathToNewFolder =path+ "/WCA/Signatures";
            // var pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/WCA/Signatures";
            //var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath; //Change to Cache
            // var path = FileSystem.CacheDirectory;
          //  var file = System.IO.Path.Combine(pathToNewFolder, filename);
            // Delete Existing Signature of same customer
            //if (File.Exists(filename))
            //{
            //    File.Delete(filename);
            //}
                using (var dest = File.OpenWrite(filename))
            {
                await bitmap.CopyToAsync(dest);
            }
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public string ReadFile(string file_name)
        {
            var stream = Assets.Open(file_name);
            StreamReader sr = new StreamReader(stream);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
        public async Task DeployTicketTemplateFromAssetsAsync(String fileName,string path)
        {

            // Android application default folder.
            //var appFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            //var appFolder = FileSystem.AppDataDirectory; // TODO: change path from Cache to AppDataDirectory
         //   var a = Assets.List("");
            var appFolder = path + "/WCA/images";
            var pdfFile = System.IO.Path.Combine(appFolder, fileName);

            // Check if the file already exists.
            if (!File.Exists(pdfFile))
            {
                using (FileStream writeStream = new FileStream(pdfFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // Assets is comming from the current context.
                    await Assets.Open(fileName).CopyToAsync(writeStream);
                }
            }
         /*   if (File.Exists(pdfFile))
            {

            }*/
        }
    }
}