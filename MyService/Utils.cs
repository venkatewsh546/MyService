using System;
using Android.App;
using Android.Content;
using Android.Media;

namespace MyService
{
    public static class Utils 
    {
        static AudioManager audio;
        static Notification.Builder mBuilder;
        static NotificationManager mNotificationManager;

        public static void SendNotification(String title,String msgtxt)
        {

            mBuilder = new Notification.Builder(Application.Context, "546546");
            mBuilder.SetSmallIcon(Resource.Drawable.notification_bg_normal);
            mBuilder.SetContentTitle(title);
            mBuilder.SetContentText(DateTime.Now.ToShortDateString()+ " "+msgtxt);
            mBuilder.SetAutoCancel(true);

           // mBuilder.SetLargeIcon(BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.widgetView));
            mBuilder.SetStyle(new Notification.BigTextStyle().BigText(msgtxt));

            mNotificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            mNotificationManager.Notify(Convert.ToInt32(DateTime.Now.ToString("hhmmss")), mBuilder.Build());

        }     

        public static void ProfileSelect(String profile)
        {
            if (profile.ToLower() == "Office".ToLower())
            {
                audio = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                audio.SetStreamVolume(Stream.Ring, 1, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Alarm, 0, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Notification, 2, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.System, 2, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Music, 0, VolumeNotificationFlags.ShowUi);
                SendNotification("Office Profile", "selected");
            }
            else if (profile.ToLower() == "Home".ToLower())
            {
                audio = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                audio.RingerMode = RingerMode.Normal;
                audio.SetStreamVolume(Stream.Ring, audio.GetStreamMaxVolume(Stream.Ring), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Alarm, audio.GetStreamMaxVolume(Stream.Alarm), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Notification, audio.GetStreamMaxVolume(Stream.Notification), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.System, audio.GetStreamMaxVolume(Stream.Notification), VolumeNotificationFlags.ShowUi);
                SendNotification("home Profile", "selected");

            }
        }
    }
}