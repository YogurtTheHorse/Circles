using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Lines;

namespace Lines_Android {
    [Activity(Label = "Circles-Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            var g = new LinesGame(true);

            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

