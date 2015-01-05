## Compass Plugin for Xamarin.Forms

## What is this?
Provides and simple way to access the compass on Windows Phone, iOS and Android from you Xamarin.Forms projects

### Setup
* Available on NuGet: https://github.com/JarleySoft/Xamarin.Plugins
* Install into your PCL project and Client projects.

**Supports Xamarin.Forms**
* iOS
* Android
* Windows Phone 8

### API Usage

You will need to call CompassImplementation.Init() after the call to Forms.Init() for each platform.

**IsSupported**
```
/// <summary>
/// Determine if Compass is available.
/// </summary>
bool IsSupported();
```
IsSupported() always returns true on Android and iOS currently. Windows Phone returns the true availability of the compass.

**Start**
```
/// <summary>
/// Plugin will begin firing DirectionChanged events as they are available
/// </summary>
void Start();
```

**Stop**
```
/// <summary>
/// Plugin will stop firing events
/// </summary>
void Stop();
```

**DirectionChanged Event**
```
/// <summary>
/// Event fires when updates are available
/// </summary>
event EventHandler<CompassDataChangedEventArgs> DirectionChanged;
```

**CompassDataChangedEventArgs**
```
/// <summary>
/// EventArgs included when DirectionChanged Event fires
/// </summary>
double Heading { get; set; }
```

### Future Enhancements??

* Integrate Calibration Form for Windows
* Provide Access to raw data

#### Contributors
* [cbartonnh](https://github.com/JarleySoft)

Thanks!

#### License
Licensed under MIT see License file
