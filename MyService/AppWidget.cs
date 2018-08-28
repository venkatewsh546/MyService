using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Media;
using Android.Widget;


namespace MyService
{
    [BroadcastReceiver(Label = "AppWidgetbcr")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget_provider")]
    public class AppWidget : AppWidgetProvider
    {
        static RemoteViews remoteViews;
        static AudioManager audio;

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            context = Application.Context;

            base.OnUpdate(context, appWidgetManager, appWidgetIds);            

            ComponentName componentName = new ComponentName(Application.Context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
            
            remoteViews = new RemoteViews(context.PackageName, Resource.Layout.Main);
            remoteViews.SetOnClickPendingIntent(Resource.Id.OfficeButton, PendingIntent(context,"Office"));
            remoteViews.SetOnClickPendingIntent(Resource.Id.HomeButton, PendingIntent(context, "Home"));

            appWidgetManager.UpdateAppWidget(componentName, remoteViews);
        }

        public PendingIntent PendingIntent(Context context,string action)
        {
            Intent intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(action);
            return Android.App.PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if(intent.Action== "Office")
            {
                audio = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                audio.SetStreamVolume(Stream.Ring, 1, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Alarm, 0, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Notification, 2, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.System, 2, VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Music, 0, VolumeNotificationFlags.ShowUi);
            }
            else if(intent.Action == "Home")
            {
                audio = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                audio.RingerMode = RingerMode.Normal;
                audio.SetStreamVolume(Stream.Ring, audio.GetStreamMaxVolume(Stream.Ring), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Alarm, audio.GetStreamMaxVolume(Stream.Alarm), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Notification, audio.GetStreamMaxVolume(Stream.Notification), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.System, audio.GetStreamMaxVolume(Stream.Notification), VolumeNotificationFlags.ShowUi);
                audio.SetStreamVolume(Stream.Music, audio.GetStreamMaxVolume(Stream.Music), VolumeNotificationFlags.ShowUi);
              
            }
        }

    }    
}