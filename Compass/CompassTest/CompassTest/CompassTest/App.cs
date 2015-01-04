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

            MainPage = new ContentPage
            {
                Content = new Label
                {
                    Text = "Hello, Forms !",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                },
            };

            compass = DependencyService.Get<ICompass>();

            compass.DirectionChanged += (sender, e) =>
            {
                Debug.WriteLine("*** Compass Heading = {0}", e.Heading);
            };

            compass.Start();
        }

        ~App()
        {
            compass.Stop();
        }
    }
}
