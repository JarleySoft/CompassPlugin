namespace CompassTest.Tizen
{
    class Program : Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            
            LoadApplication(new App());
            MainWindow.AvailableRotations = ElmSharp.DisplayRotation.Degree_0;
        }

        static void Main(string[] args)
        {
            var app = new Program();
            global::Xamarin.Forms.Platform.Tizen.Forms.Init(app);
            app.Run(args);
        }
    }
}
