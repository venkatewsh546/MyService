using System;
using Android.App;
using Android.Content;
using Android.Telephony;
using static myservice.Utils;

namespace myservice
{
    [BroadcastReceiver(Label = "BcReceiver", Name = "com.android.vtsapps.myservice.BcReceiver", Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new string[] { "android.intent.action.PHONE_STATE"})]

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
                else if(intent.Action == ProfileName.HOME)
                {
                    ProfileSelect(ProfileName.HOME);
                }
                else if (intent.Action == ProfileName.OFFICE)
                {
                    ProfileSelect(ProfileName.OFFICE);
                }
                else 
                {
                    Intent backgroundService = new Intent(Application.Context, typeof(SlaveService));
                    Application.Context.StartForegroundService(backgroundService);
                }
            }
            catch(Exception ex)
            {
                SendNotification("Error", ex.Message);
            }
        }
    }
}