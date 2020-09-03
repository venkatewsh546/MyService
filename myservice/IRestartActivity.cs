using Android.App;
using Android.Content;
using Java.Lang;
using System;

namespace myservice
{
    interface IRestartActivity
    {
        public void RestartActivity(Intent intent)
        {
            ((AlarmManager)Application.Context.GetSystemService(Context.AlarmService)).Set(
                     AlarmType.Rtc, DateTime.Now.Millisecond + 10000,
                PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.OneShot));
            Utils.SendNotification("Exception","Restarting");
            JavaSystem.Exit(2);
        }

        public void RestartActivityOnBoot()
        {
            Intent backgroundService = new Intent(Application.Context, typeof(SlaveService));
            Application.Context.StartForegroundService(backgroundService);
        }

    }
    class RestartService : IRestartActivity
    {
    }
}