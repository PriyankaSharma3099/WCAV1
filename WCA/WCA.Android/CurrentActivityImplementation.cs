using WCA;
using WCA.Droid;
using Xamarin.Forms;
using Android.App;

[assembly: Dependency(typeof(CurrentActivityImplementation))]
namespace WCA.Droid
{
    public class CurrentActivityImplementation : ICurrentActivity
    {
        public object GetCurrentActivity()
        {
            var context = MainActivity.Instance;
            return context;
        }
    }
}

