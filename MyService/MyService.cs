using System;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace MyService
{
    [Service]
    class MyService : Service, ISensorEventListener
    {
        private BroadcastReceiver bcReceiver;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 836598;

        SensorManager mSensorManager;
        Sensor mProximity;
        static public PowerManager powerManager;
        static public PowerManager.WakeLock wakeLock;

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

            PendingIntent mainPendingIntent = PendingIntent.GetActivity(this, 0, mainActivity, 0);


            var notification = new Notification.Builder(this, "546546")
                                    .SetContentTitle("MyService")
                                    .SetContentText("MyServiceNotify")
                                    .SetSmallIcon(Resource.Drawable.widgetView)
                                    .SetContentIntent(mainPendingIntent)
                                    //.SetOngoing(true)
                                    .SetAutoCancel(true)
                                    .Build();

            // Enlist this instance of the service as a foreground service
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (bcReceiver != null)
            {
                UnregisterReceiver(bcReceiver);
                Utils.SendNotification("Broadcat Unregister", "broadcast receiver UnRegistered");
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            
            AppDomain.CurrentDomain.UnhandledException += UnhandleException;

            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(Intent.ActionBootCompleted);
            //intentFilter.AddAction("android.net.conn.CONNECTIVITY_CHANGE");
            intentFilter.AddAction("android.intent.action.PHONE_STATE");           
            intentFilter.Priority = 100;     

            if (bcReceiver != null)
            {
                bcReceiver.Dispose();
            }

            bcReceiver = new BcReceiver();
            RegisterReceiver(bcReceiver, intentFilter);

            mSensorManager = (SensorManager)Application.ApplicationContext.GetSystemService(SensorService);
            mProximity = mSensorManager.GetDefaultSensor(SensorType.Proximity);
            powerManager = (PowerManager)Application.ApplicationContext.GetSystemService(PowerService);
            mSensorManager.RegisterListener(this, mProximity, SensorDelay.Normal);           

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            return;
        }

        public void OnSensorChanged(SensorEvent e)
        {
            return;

            /*
            Boolean iswiredheadset = false;

            try
            {
                audioManager  = (AudioManager)Application.Context.GetSystemService(AudioService);

                AudioDeviceInfo[] audioDeviceInfos = audioManager.GetDevices(GetDevicesTargets.Outputs);

               foreach(AudioDeviceInfo adi in audioDeviceInfos)
                {
                    if ( adi.Type == AudioDeviceType.WiredHeadset 
                        || adi.Type == AudioDeviceType.BluetoothA2dp 
                        || adi.Type == AudioDeviceType.WiredHeadphones 
                        || adi.Type == AudioDeviceType.UsbHeadset 
                        || adi.Type == AudioDeviceType.AuxLine 
                       )
                    {
                        iswiredheadset = true;
                    }
                }               

                if (!audioManager.IsMusicActive)
                {
                    if (iswiredheadset)
                    {
                        return;
                    }
                    if (Convert.ToInt32(Math.Ceiling(e.Values[0])) == 0)
                    {
                        if (timer == null)
                        {
                            timer = new Timer
                            {
                                Interval = 0.5 * 1000
                            };
                            timer.Elapsed += Runtask;
                            timer.Enabled = true;
                            timer.AutoReset = false;
                        }
                    }
                    else
                    {
                        if (timer != null)
                        {
                            timer.Stop();
                            timer.Dispose();
                            timer = null;

                        }

                        if (wakeLock != null)
                        {
                            wakeLock.Release();

                        }
                    }
                }
            }
            catch
            {
               
            }
            */
        }

        private void Runtask(object sender, ElapsedEventArgs e)
        {
           // wakeLock = powerManager.NewWakeLock(WakeLockFlags.ProximityScreenOff, "sleep");
           // wakeLock.Acquire();
        }


        public void UnhandleException(object sender, UnhandledExceptionEventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);
            PendingIntent pendingIntent = PendingIntent.GetActivity(new Application().ApplicationContext, 0, intent, PendingIntentFlags.OneShot);

            AlarmManager mgr = (AlarmManager)Application.ApplicationContext.GetSystemService(Context.AlarmService);
            mgr.Set(AlarmType.Rtc, DateTime.Now.Millisecond + 4000, pendingIntent);           
            System.Environment.Exit(2);
        }
    }
}