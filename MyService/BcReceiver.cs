using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Net.Wifi;
using Android.Telephony;
using static MyService.Utils;

namespace MyService
{
    [BroadcastReceiver]
    public class BcReceiver : BroadcastReceiver
    {
        WifiManager wifiManager;
        static AudioManager audio;
        private PSListener pSListener;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action.Equals("android.net.conn.CONNECTIVITY_CHANGE"))
                {
                    int time = Convert.ToInt32(DateTime.Now.ToString("HH"));

                    if (time == 9)
                    {
                        wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));

                        if (!wifiManager.IsWifiEnabled)
                        {                            
                            wifiManager.SetWifiEnabled(true);
                        }

                        IList<ScanResult> scanwifinetworks = wifiManager.ScanResults;
                        var match = scanwifinetworks.Where(r => r.Ssid.ToLower().Contains("hpe")).FirstOrDefault();

                        if (match != null)
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
                }
                else if (time >= 1700 && time <= 1730)
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
                else if (intent.Action.Equals("android.intent.action.PHONE_STATE"))
                {
                    pSListener = new PSListener();
                    TelephonyManager tm = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);
                    tm.Listen(pSListener, PhoneStateListenerFlags.CallState);
                }              
            }
            catch(Exception ex)
            {
                SendNotification("Error", ex.Message);
            }
        }
    }
}