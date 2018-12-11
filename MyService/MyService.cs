using Android.App;
using Android.Content;
using Android.OS;

namespace MyService
{
    [Service]
    class MyService : Service
    {
        private BroadcastReceiver bcReceiver;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 836598;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //flags = StartCommandFlags.Retry;
            base.OnStartCommand(intent, flags, startId);
  
            Intent mainActivity = new Intent(this, typeof(MainActivity));
            mainActivity.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            PendingIntent mainPendingIntent = PendingIntent.GetActivity(this, 0, mainActivity,0);
            

            Intent homeIntent = new Intent(Application.Context, typeof(BcReceiver));
            homeIntent.SetAction(ProfileName.HOME);
            homeIntent.PutExtra("profile", ProfileName.HOME);
            PendingIntent homePI = PendingIntent.GetBroadcast(this, 0, homeIntent, PendingIntentFlags.UpdateCurrent);                        
            Notification.Action homeAction = new Notification.Action.Builder(Resource.Drawable.home,"HomeProfile", homePI).Build();
         

            Intent officeIntent = new Intent(this, typeof(BcReceiver));
            officeIntent.SetAction(ProfileName.OFFICE);
            officeIntent.PutExtra("profile", ProfileName.OFFICE);
            PendingIntent officePI = PendingIntent.GetBroadcast(this, 0, officeIntent, PendingIntentFlags.UpdateCurrent);
            Notification.Action officeAction = new Notification.Action.Builder(Resource.Drawable.office,"officeProfile",officePI).Build();

            var notification = new Notification.Builder(this, "546546")
                                    .SetContentTitle("MyService")
                                    .SetContentText("MyServiceNotify")
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

            Intent broadcastIntent = new Intent(this, typeof(MyService));
            SendBroadcast(broadcastIntent);

        }

        public override void OnCreate()
        {
            base.OnCreate();  

            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(Intent.ActionBootCompleted);
            intentFilter.AddAction("android.intent.action.PHONE_STATE");
            intentFilter.Priority = 100;     

            if (bcReceiver != null)
            {
                bcReceiver.Dispose();
            }

            bcReceiver = new BcReceiver();
            RegisterReceiver(bcReceiver, intentFilter);

        }   
    }
}