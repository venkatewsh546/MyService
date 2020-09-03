using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Widget;
using Java.Lang;
using System;
using System.Threading.Tasks;

namespace myservice
{
    [Activity(Label = "@string/app_name", Name = "com.android.vtsapps.myservice.MainActivity", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sample_main_layout);

            IRestartActivity restartService = new RestartService();

            var intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("crash", true);
            intent.AddFlags(ActivityFlags.ClearTop |
                            ActivityFlags.ClearTask |
                            ActivityFlags.NewTask);

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => { restartService.RestartActivity(intent); };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { restartService.RestartActivity(intent); };
            TaskScheduler.UnobservedTaskException += (sender, e) => { restartService.RestartActivity(intent); };

            Button office = FindViewById<Button>(id: Resource.Id.MainOffice);
            office.Click += delegate
            {
                Utils.ProfileSelect(ProfileName.OFFICE);
            };

            Button buttonHome = FindViewById<Button>(Resource.Id.MainHome);
            buttonHome.Click += delegate
            {
                Utils.ProfileSelect(ProfileName.HOME);
            };

            EditText whatsappno = FindViewById<EditText>(Resource.Id.whatsappno);

            Button openurl = FindViewById<Button>(Resource.Id.openurl);
            openurl.Click += delegate
            {
                if (!string.IsNullOrEmpty(whatsappno.Text))
                {
                    var uri = Android.Net.Uri.Parse("https://api.whatsapp.com/send?phone=" + whatsappno.Text.Replace("+", ""));
                    StartActivity(new Intent(Intent.ActionView, uri));
                }
            };

            RequestPermissions();

            CreateNotificationChannel();

            CreateService();

        }

        private void CreateNotificationChannel()
        {
            NotificationChannel channel
                = new NotificationChannel("546546", new Java.Lang.String("myserviceCnl"), NotificationImportance.Low)
                {
                    Description = "it will send notifications from my service"
                };
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void CreateService()
        {
            Intent backgroundService = new Intent(Application.Context, typeof(SlaveService));
            StartForegroundService(backgroundService);
        }

        private void RequestPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) != (int)Permission.Granted ||
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted )
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("requesting permissions");
                alert.SetMessage("requesting permissions");
                alert.SetPositiveButton("ok", (senderAlert, args) =>
                {
                    RequestPermissions(new string[]
                    {   Manifest.Permission.ReadPhoneState,
                        Manifest.Permission.ModifyPhoneState,
                        Manifest.Permission.WriteExternalStorage,
                         Manifest.Permission.ReadExternalStorage,
                         Manifest.Permission.ReceiveBootCompleted
                    }, 0);

                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Permission Not Granted", ToastLength.Short);
                    System.Environment.Exit(0);
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            int results = 1;

            switch (requestCode)
            {
                case 0:
                    {
                        if (grantResults.Length > 0)
                        {
                            foreach (Permission permission in grantResults)
                            {
                                if (permission.Equals(Permission.Granted))
                                {
                                    results = 0;
                                }
                                else
                                {
                                    results = 1;
                                }

                            }
                            Toast.MakeText(this, "Permission Granted", ToastLength.Short);
                        }

                        if (results == 1)
                        {
                            Toast.MakeText(this, "some of the Permission denied", ToastLength.Short);
                            System.Environment.Exit(0);
                        }
                        return;
                    }
            }
        }
    }
}

