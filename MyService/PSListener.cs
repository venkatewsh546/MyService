using Android.OS;
using Android.Telephony;
using Android.App;
using Android.Content;
using System;

namespace MyService
{
    public class PSListener : PhoneStateListener
    {
        PowerManager powerManager;
        int attemp = 0;
        int attempidle = 0;

        public PSListener()
        {
        }

        public override void OnCallStateChanged(CallState state, string incomingNumber)
        {
            base.OnCallStateChanged(state, incomingNumber);

            try
            {
                if (state == CallState.Ringing)
                {
                    powerManager = (PowerManager)Application.Context.GetSystemService(Context.PowerService);
                    MyService.wakeLock = powerManager.NewWakeLock(WakeLockFlags.ProximityScreenOff, "sleep");
                    MyService.wakeLock.Acquire();
                }
                else if (state == CallState.Offhook)
                {
                    if (attemp == 0)
                    {
                        if (MyService.wakeLock != null)
                        {
                            MyService.wakeLock.Release();
                        }
                        attemp = attemp + 1;
                    }
                }
                else if (state == CallState.Idle)
                {
                    if (attempidle == 0)
                    {
                        if (MyService.wakeLock != null)
                        {
                            MyService.wakeLock.Release();
                        }
                        attempidle = attempidle + 1;
                    }
                }
            }
            catch(Exception ex)
            {
                Utils.SendNotification("Error",ex.Message);
            }
           
        }
    }
}