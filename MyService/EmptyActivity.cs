using Android.App;
using Android.OS;

namespace MyService
{
    [Activity(Label = "EmptyActivity")]
    public class EmptyActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.callListener);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }
    }
}