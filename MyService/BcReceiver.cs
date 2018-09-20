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
                            ProfileSelect("Office");
                            SendNotification("Office Profile", "office profile selected");                      
                    }
                }
                else if (time == 17)
                {
                        ProfileSelect("Home");
                        SendNotification("home Profile", "home profile selected");
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