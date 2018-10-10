using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using System;

namespace MyService
{
    public class PSListener : PhoneStateListener
    {
        public override void OnCallStateChanged(CallState state, string incomingNumber)
        {
            base.OnCallStateChanged(state, incomingNumber);            

            try
            {    
                if (state == CallState.Ringing)
                {
                    //if (MyService.wakeLock != null)
                    //{
                    //    MyService.powerManager = (PowerManager)Application.Context.GetSystemService(Context.PowerService);
                    //    MyService.wakeLock = MyService.powerManager.NewWakeLock(WakeLockFlags.ProximityScreenOff, "sleep");
                    //    MyService.wakeLock.Acquire();
                    //}

                    Intent intent = new Intent(Application.Context, typeof(EmptyActivity));
                    Application.Context.StartActivity(intent);
                }
                else if (state == CallState.Idle)
                {
                    //if (MyService.wakeLock != null)
                    //{
                    //    MyService.wakeLock.Release();
                    //}
                }
            }
            catch(Exception ex)
            {
                Utils.SendNotification("Error",ex.Message);
            }
           
        }
    }
}