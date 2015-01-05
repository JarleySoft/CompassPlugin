using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Compass.Forms.Plugin.Abstractions;
using System.Diagnostics;

namespace CompassTest
{
    public class App : Application
    {
        ICompass compass;

        public App()
        {
            Label label = new Label 
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            MainPage = new ContentPage
            {
                Content = label
            };

            compass = DependencyService.Get<ICompass>();

            compass.DirectionChanged += (sender, e) =>
            {
                Debug.WriteLine("*** Compass Heading = {0}", e.Heading);
                Device.BeginInvokeOnMainThread(() =>
                {
                    label.Text = String.Format("Heading = {0}", e.Heading.ToString());
                });
            };

            compass.Start();
        }

        ~App()
        {
            compass.Stop();
        }
    }
}
