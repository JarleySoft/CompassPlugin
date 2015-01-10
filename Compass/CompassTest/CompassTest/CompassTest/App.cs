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
                if (compass == null)
                {
                    compass = DependencyService.Get<ICompass>();

                    compass.DirectionChanged += (s, e) =>
                    {
                        Debug.WriteLine("*** Compass Heading = {0}", e.Heading);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label.Text = String.Format("Heeading = {0}", e.Heading.ToString());
                        });
                    };
                }

                compass.Start();

                Device.BeginInvokeOnMainThread(() =>
                {
                    buttonStart.IsEnabled = false;
                    buttonStop.IsEnabled = true;
                });

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
/*
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
 */
        }
    }
}
