using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.Res;
using System.IO;
using System.Threading.Tasks;

namespace WCA.Droid
{
    public class ReadAsset : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //InputStream input = Assets.Open("TicketTemplate.pdf");
            //AssetManager assets = Android.App.Application.Context.Assets;
            var asset = Application.Context.Assets.Open("TicketTemplate.pdf");

            //CopyDatabaseAsync().ContinueWith(t =>
            //{
            //    if (t.Status != TaskStatus.RanToCompletion)
            //        return;

            //    //your code here
            //});
        }
        

        //public static async Task CopyDatabaseAsync(Activity activity)
        //{
        //    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "YOUR_DATABASENAME");

        //    if (!File.Exists(dbPath))
        //    {
        //        try
        //        {
        //            using (var dbAssetStream = activity.Assets.Open("YOUR_DATABASENAME"))
        //            using (var dbFileStream = new FileStream(dbPath, FileMode.OpenOrCreate))
        //            {
        //                var buffer = new byte[1024];

        //                int b = buffer.Length;
        //                int length;

        //                while ((length = await dbAssetStream.ReadAsync(buffer, 0, b)) > 0)
        //                {
        //                    await dbFileStream.WriteAsync(buffer, 0, length);
        //                }

        //                dbFileStream.Flush();
        //                dbFileStream.Close();
        //                dbAssetStream.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //Handle exceptions
        //        }
        //    }
        //}

    }
}