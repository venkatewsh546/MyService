using Android.App;
using Android.OS;
using System;
using Android.Content;
using Android;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Views;

namespace MyService
{
    [Activity(Label = "MyService", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent backgroundService = new Intent(Application.Context, typeof(MyService));
            StartService(backgroundService);

            View layout = FindViewById(Resource.Id.sample_main_layout);


            string[] chkPermissions =
             {
                Manifest.Permission.WriteSms,
                Manifest.Permission.ReadSms,
                Manifest.Permission.Internet,
                Manifest.Permission.WakeLock,
                Manifest.Permission.SetAlarm,
                Manifest.Permission.UpdateDeviceStats,
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.BindDeviceAdmin,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.ReadPhoneState,
                Manifest.Permission.ModifyPhoneState,
                Manifest.Permission.SetAlarm,
             };


            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("requesting storage permissions");
                alert.SetMessage("requesting storage permissions");
                alert.SetPositiveButton("ok", (senderAlert, args) => {
                    RequestPermissions(new string[]
                    { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);
                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                    Toast.MakeText(this, "Storage Permission Not Granted", ToastLength.Short);
                    System.Environment.Exit(0);
                });
                Dialog dialog = alert.Create();
                dialog.Show();
                // Finish();

            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case 0:
                    {
                        if (grantResults.Length>0 && grantResults[0] == Permission.Granted)
                        {
                            Toast.MakeText(this, "Storage Permission Granted", ToastLength.Short);
                        }
                        else
                        {
                            Toast.MakeText(this, "Storage Permission Not Granted", ToastLength.Short);
                            System.Environment.Exit(0);
                        }
                    }
                    return;
            }

        }


    }
}

