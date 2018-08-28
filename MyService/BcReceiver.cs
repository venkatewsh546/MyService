using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Net.Wifi;
using static MyService.Utils;

namespace MyService
{
    [BroadcastReceiver]
    public class BcReceiver : BroadcastReceiver
    {
        WifiManager wifiManager;
        static AudioManager audio;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action.Equals("android.net.conn.CONNECTIVITY_CHANGE"))
                {
                    int time = Convert.ToInt32(DateTime.Now.Hour.ToString());

                    wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
                    if (wifiManager.IsWifiEnabled 
                        && wifiManager.ConnectionInfo.SSID.ToLower().Contains("hpe") 
                        && (time == 9 || time == 10))
                    {
                        audio = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                        audio.SetStreamVolume(Stream.Ring, 1, VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.Alarm, 0, VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.Notification, 2, VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.System, 2, VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.Music, 0, VolumeNotificationFlags.ShowUi);

                        if (audio != null)
                        {
                            audio.Dispose();
                        }
                    }
                    else if (time == 17)
                    {
                        audio = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                        audio.RingerMode = RingerMode.Normal;
                        audio.SetStreamVolume(Stream.Ring, audio.GetStreamMaxVolume(Stream.Ring), VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.Alarm, audio.GetStreamMaxVolume(Stream.Alarm), VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.Notification, audio.GetStreamMaxVolume(Stream.Notification), VolumeNotificationFlags.ShowUi);
                        audio.SetStreamVolume(Stream.System, audio.GetStreamMaxVolume(Stream.Notification), VolumeNotificationFlags.ShowUi);

                        if (audio != null)
                        {
                            audio.Dispose();
                        }
                    }
                }
                else
                {
                    SendNotification("info", DateTime.Now.ToString());
                }
            }
            catch
            {

            }
        }
    }
}