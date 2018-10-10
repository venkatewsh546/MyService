using System;
using Android.App;
using Android.Content;
using Android.Telephony;
using static MyService.Utils;

namespace MyService
{
    [BroadcastReceiver]
    public class BcReceiver : BroadcastReceiver
    {       
        private PSListener pSListener;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action.Equals("android.intent.action.PHONE_STATE"))
                {
                    pSListener = new PSListener();
                    TelephonyManager tm = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);
                    tm.Listen(pSListener, PhoneStateListenerFlags.CallState);
                }
                else if (intent.Action== Intent.ActionBootCompleted)
                {
                    Intent backgroundService = new Intent(Application.Context, typeof(MyService));
                    Application.Context.StartForegroundService(backgroundService);
                }
                else if(intent.Action == Profiles.HOME)
                {
                    ProfileSelect(Profiles.HOME);
                }
                else if (intent.Action == Profiles.OFFICE)
                {
                    ProfileSelect(Profiles.OFFICE);
                }               
            }
            catch(Exception ex)
            {
                SendNotification("Error", ex.Message);
            }
        }
    }
}