using Xamarin.Forms;
using System.Diagnostics;
using Plugin.Compass.Abstractions;
using Plugin.Compass;

namespace CompassTest
{
    public class App : Application
    {
        ICompass compass;

        public App()
        {
            compass = CrossCompass.Current;
            var label = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            var buttonStart = new Button
            {
                Text = "Start",
                IsEnabled = true
            };

            var buttonStop = new Button
            {
                Text = "Stop",
                IsEnabled = false
            };

            buttonStart.Clicked += (sender, args) =>
            {

                compass.CompassChanged += (s, e) =>
                {
                    Debug.WriteLine("*** Compass Heading = {0}", e.Heading);

                    label.Text = $"Heading = {e.Heading}";

                };

                compass.Start();

                buttonStart.IsEnabled = false;
                buttonStop.IsEnabled = true;

            };

            buttonStop.Clicked += (sender, args) =>
            {
                if (compass != null)
                {
                    compass.Stop();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        buttonStart.IsEnabled = true;
                        buttonStop.IsEnabled = false;
                    });
                }

            };

            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    Children = { label, buttonStart, buttonStop }
                }
            };
        }
    }
}
