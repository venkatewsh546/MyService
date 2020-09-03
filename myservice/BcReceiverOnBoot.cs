using System;
using Android.App;
using Android.Content;
using Android.Telephony;
using static myservice.Utils;

namespace myservice
{
    [BroadcastReceiver(Label = "BcReceiverOnBoot", Name = "com.android.vtsapps.myservice.BcReceiverOnBoot", 
        Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new string[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, 
        "android.intent.action.QUICKBOOT_POWERON", "android.intent.action.BOOT_COMPLETED",
        "android.intent.action.REBOOT", "android.intent.action.QUICKBOOT_POWERON"})]

    public class BcReceiverOnBoot : BroadcastReceiver
    {

        private IRestartActivity restartActivity;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                restartActivity = new RestartService();
                restartActivity.RestartActivityOnBoot();

               // Intent backgroundService = new Intent(Application.Context, typeof(SlaveService));
               //Application.Context.StartForegroundService(backgroundService);

            }
            catch(Exception ex)
            {
                SendNotification("Error in BcReceiverOnBoot", ex.Message);
            }
        }
    }
}