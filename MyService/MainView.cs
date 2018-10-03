using Android.App;
using Android.OS;
using Android.Widget;

namespace MyService
{
    [Activity(Label = "MainView")]
    public class MainView : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sample_main_layout);

            Button office = FindViewById<Button>(Resource.Id.MainOffice);
            office.Click += delegate
            {
                Utils.ProfileSelect(Profiles.OFFICE);
            };

            Button buttonHome = FindViewById<Button>(Resource.Id.MainHome);
            buttonHome.Click += delegate
            {
                Utils.ProfileSelect(Profiles.HOME);
            };
        }
    }
}