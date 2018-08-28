using System;
using Android.App;
using Android.Graphics;
using Android.Media;

namespace MyService
{
    public static class Utils 
    {
        public static void SendNotification(String title,String msgtxt)
        {
            Notification.Builder mBuilder;
            NotificationManager mNotificationManager;
            mBuilder = new Notification.Builder(Application.Context);
            mBuilder.SetSmallIcon(Resource.Drawable.widgetView);
            mBuilder.SetContentTitle(title);
            mBuilder.SetContentText(msgtxt);
            mBuilder.SetAutoCancel(true);
            mBuilder.SetLargeIcon(BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.widgetView));
            mBuilder.SetStyle(new Notification.BigTextStyle().BigText(msgtxt));
           
            mNotificationManager = NotificationManager.FromContext(Application.Context);
            mNotificationManager.Notify(Convert.ToInt32(DateTime.Now.ToString("hhmmss")), mBuilder.Build());

        }     
    }
}