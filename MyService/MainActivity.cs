using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Java.Util;

namespace MyService
{
    [Activity(Label = "MyService", MainLauncher = true)]
    public class MainActivity : Activity
    {       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sample_main_layout);

            Button office = FindViewById<Button>(Resource.Id.MainOffice);
            office.Click += delegate
            {
                Utils.ProfileSelect(ProfileName.OFFICE);
            };

            Button buttonHome = FindViewById<Button>(Resource.Id.MainHome);
            buttonHome.Click += delegate
            {
                Utils.ProfileSelect(ProfileName.HOME);
            };

            NotificationChannel channel 
                = new NotificationChannel("546546",new Java.Lang.String("MyServiceCnl"),NotificationImportance.Low)
            {
                Description = "it will send notifications from my service"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);


            //Task.Run(async () => { await CreateService();});
            Intent backgroundService = new Intent(Application.Context, typeof(MyService));
            StartForegroundService(backgroundService);

            ScheduleAlarm();
            RequestPermissions();
        }

        public void CreateService()
        {
            Intent backgroundService = new Intent(Application.Context, typeof(MyService));
            StartForegroundService(backgroundService);
        }

        private void RequestPermissions()
        {
            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("requesting permissions");
                alert.SetMessage("requesting permissions");
                alert.SetPositiveButton("ok", (senderAlert, args) =>
                {
                    RequestPermissions(new string[]
                    { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage}, 0);

                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Storage Permission Not Granted", ToastLength.Short);
                    System.Environment.Exit(0);
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }


            if (CheckSelfPermission(Manifest.Permission.ReadPhoneState) != (int)Permission.Granted)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("requesting permissions");
                alert.SetMessage("requesting permissions");
                alert.SetPositiveButton("ok", (senderAlert, args) =>
                {
                    RequestPermissions(new string[]
                    { Manifest.Permission.ReadPhoneState,Manifest.Permission.ModifyPhoneState}, 1);

                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "phone Permission Not Granted", ToastLength.Short);
                    System.Environment.Exit(0);
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }


            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("requesting permissions");
                alert.SetMessage("requesting permissions");
                alert.SetPositiveButton("ok", (senderAlert, args) =>
                {
                    RequestPermissions(new string[]
                    { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation}, 2);

                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Storage Permission Not Granted", ToastLength.Short);
                    System.Environment.Exit(0);
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        private void ScheduleAlarm()
        {
            Calendar amcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
            amcal.Set(CalendarField.HourOfDay, 9);
            amcal.Set(CalendarField.Minute, 10);
            amcal.Set(CalendarField.Second, 2);

            Intent officeprofile = new Intent(this, typeof(BcReceiver));
            officeprofile.SetAction(ProfileName.OFFICE);
            PendingIntent ampi = PendingIntent.GetBroadcast(this, 0, officeprofile, PendingIntentFlags.CancelCurrent);
            AlarmManager officealarmManager = (AlarmManager)GetSystemService(AlarmService);
            officealarmManager.SetRepeating(AlarmType.Rtc, amcal.TimeInMillis, AlarmManager.IntervalDay, ampi);

            Calendar pmcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
            pmcal.Set(CalendarField.HourOfDay, 17);
            pmcal.Set(CalendarField.Minute, 5);
            pmcal.Set(CalendarField.Second, 2);

            Intent homeprofile = new Intent(this, typeof(BcReceiver));
            homeprofile.SetAction(ProfileName.HOME);
            PendingIntent Ofpi = PendingIntent.GetBroadcast(this, 1, homeprofile, PendingIntentFlags.CancelCurrent);
            AlarmManager homealarmManager = (AlarmManager)GetSystemService(AlarmService);
            homealarmManager.SetRepeating(AlarmType.Rtc, pmcal.TimeInMillis, AlarmManager.IntervalDay, Ofpi);

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
                        return;
                    }
                case 1:
                    {
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                        {
                            Toast.MakeText(this, "phone Permission Granted", ToastLength.Short);
                        }
                        else
                        {
                            Toast.MakeText(this, "phone Permission Not Granted", ToastLength.Short);
                            System.Environment.Exit(0);
                        }
                        return;
                    }

                case 2:
                    {
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                        {
                            Toast.MakeText(this, "location Permission Granted", ToastLength.Short);
                        }
                        else
                        {
                            Toast.MakeText(this, "location Permission Not Granted", ToastLength.Short);
                            System.Environment.Exit(0);
                        }
                        return;
                    }

            }
        }
    }
}

