using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Java.Util;
using System;
using System.Threading.Tasks;

namespace myservice
{
    [Service(Label = "SlaveService")]
    class SlaveService : Service, IRestartActivity
    {
        private BroadcastReceiver bcReceiver;
        private BcReceiverOnBoot bcReceiverOnBoot;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 836598;
        public IRestartActivity restartService;
        Intent mainActivity;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);

            restartService = new RestartService();

            mainActivity = new Intent(this, typeof(MainActivity));
            mainActivity.SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);
            PendingIntent mainPendingIntent = PendingIntent.GetActivity(Application.Context, 0, mainActivity,0);

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => { restartService.RestartActivity(mainActivity);};
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { restartService.RestartActivity(mainActivity);};
            TaskScheduler.UnobservedTaskException += (sender, e) => { restartService.RestartActivity(mainActivity);};


            Intent homeIntent = new Intent(Application.Context, typeof(BcReceiver));
            homeIntent.SetAction(ProfileName.HOME);
            homeIntent.PutExtra("profile", ProfileName.HOME);
            PendingIntent homePI = PendingIntent.GetBroadcast(Application.Context, 0, homeIntent, PendingIntentFlags.UpdateCurrent);                        
            Notification.Action homeAction = new Notification.Action.Builder(Resource.Drawable.home,"HomeProfile", homePI).Build();
         

            Intent officeIntent = new Intent(Application.Context, typeof(BcReceiver));
            officeIntent.SetAction(ProfileName.OFFICE);
            officeIntent.PutExtra("profile", ProfileName.OFFICE);
            PendingIntent officePI = PendingIntent.GetBroadcast(Application.Context, 0, officeIntent, PendingIntentFlags.UpdateCurrent);
            Notification.Action officeAction = new Notification.Action.Builder(Resource.Drawable.office,"officeProfile",officePI).Build();

            var notification = new Notification.Builder(this, "546546")
                                    .SetContentTitle("myservice")
                                    .SetContentText("myserviceNotify")
                                    .SetSmallIcon(Resource.Drawable.widgetView)
                                    .SetContentIntent(mainPendingIntent)
                                    .AddAction(homeAction)
                                    .AddAction(officeAction)
                                    .SetOngoing(true)
                                    .Build();

            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

            return StartCommandResult.Sticky;           

        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Intent broadcastIntent = new Intent(Application.Context, typeof(SlaveService));
            broadcastIntent.SetAction("android.intent.action.REBOOT");
            SendBroadcast(broadcastIntent);
        }

        public override void OnCreate()
        {
            base.OnCreate();  

            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(Intent.ActionBootCompleted);
            intentFilter.AddAction(Intent.ActionLockedBootCompleted);
            intentFilter.AddAction("android.intent.action.PHONE_STATE");
            intentFilter.AddAction("android.intent.action.BOOT_COMPLETED");
            intentFilter.AddAction("android.intent.action.REBOOT");
            intentFilter.AddAction("android.intent.action.QUICKBOOT_POWERON");
            intentFilter.Priority = 100;     

            if (bcReceiver != null)
            {
                bcReceiver.Dispose();
            }

            bcReceiver = new BcReceiver();
            RegisterReceiver(bcReceiver, intentFilter);

            if(bcReceiverOnBoot != null)
            {
                bcReceiverOnBoot.Dispose();
            }

            bcReceiverOnBoot = new BcReceiverOnBoot();
            RegisterReceiver(bcReceiverOnBoot, intentFilter);

            ScheduleAlarm();

        }

        private void ScheduleAlarm()
        {
            Calendar amcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
            amcal.Set(CalendarField.HourOfDay, 9);
            amcal.Set(CalendarField.Minute, 0);
            amcal.Set(CalendarField.Second, 0);

            Intent officeprofile = new Intent(Application.Context, typeof(BcReceiver));
            officeprofile.SetAction(ProfileName.OFFICE);
            PendingIntent ampi = PendingIntent.GetBroadcast(Application.Context, 0, officeprofile, PendingIntentFlags.UpdateCurrent);
            AlarmManager officealarmManager = (AlarmManager)GetSystemService(AlarmService);
            officealarmManager.SetRepeating(AlarmType.RtcWakeup, amcal.TimeInMillis, AlarmManager.IntervalHalfDay, ampi);

            Calendar pmcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
            pmcal.Set(CalendarField.HourOfDay, 5);
            pmcal.Set(CalendarField.Minute, 0);
            pmcal.Set(CalendarField.Second, 0);

            Intent homeprofile = new Intent(Application.Context, typeof(BcReceiver));
            homeprofile.SetAction(ProfileName.HOME);
            PendingIntent Ofpi = PendingIntent.GetBroadcast(Application.Context, 1, homeprofile, PendingIntentFlags.UpdateCurrent);
            AlarmManager homealarmManager = (AlarmManager)GetSystemService(AlarmService);
            homealarmManager.SetRepeating(AlarmType.RtcWakeup, pmcal.TimeInMillis, AlarmManager.IntervalHalfDay, Ofpi);

        }
    }
}