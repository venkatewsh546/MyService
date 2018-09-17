using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyService
{
    [Activity(Label = "CallListener")]
    public class CallListener : Activity
    {
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            View layout = FindViewById(Resource.Id.sample_main_layout);

        }
    }
}