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
        static bool office =true;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action !=null )
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
                }
                else
                {
                    int time = Convert.ToInt32(DateTime.Now.ToString("HH"));

                    if(DayOfWeek.Saturday == DateTime.Now.DayOfWeek || DayOfWeek.Sunday == DateTime.Now.DayOfWeek)
                    {
                        office = false;
                    }

                    if (time == 9 && office)
                    {
                        ProfileSelect("Office");
                    }
                    else if (time == 17)
                    {
                        ProfileSelect("Home");
                    }
                }
            }
            catch(Exception ex)
            {
                SendNotification("Error", ex.Message);
            }
        }
    }
}