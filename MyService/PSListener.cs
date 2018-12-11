using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using System;
using System.Threading;

namespace MyService
{
    public class PSListener : PhoneStateListener
    {
        public static PowerManager powerManager;
        public static PowerManager.WakeLock wakeLock;

        public override void OnCallStateChanged(CallState state, string incomingNumber)
        {
            base.OnCallStateChanged(state, incomingNumber);

            try
            {
                if (state == CallState.Ringing)
                {
                    powerManager = (PowerManager)Application.Context.GetSystemService(Context.PowerService);
                    wakeLock = powerManager.NewWakeLock(WakeLockFlags.ProximityScreenOff, "sleep");
                    wakeLock.Acquire(10000);
                }
                    
            }
            catch (Exception ex)
            {
                Utils.SendNotification("Error", ex.Message);
            }
           
        }
    }
}