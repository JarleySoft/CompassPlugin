## Compass Plugin
Provides and simple way to access the compass on iOS, Android, and Windows. Returns 0-360 degrees

### Setup
* Available on NuGet: https://www.nuget.org/packages/Plugin.Compass
* Install into your PCL project and Client projects.

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No||

### API Usage

**IsSupported**
```csharp
/// <summary>
/// Determine if Compass is available.
/// </summary>
bool IsSupported { get; }
```


**Start**
```csharp
/// <summary>
/// Plugin will begin firing CompassChanged events as they are available
/// </summary>
void Start();
```

**Stop**
```csharp
/// <summary>
/// Plugin will stop firing events
/// </summary>
void Stop();
```

**DirectionChanged Event**
```csharp
/// <summary>
/// Event fires when updates are available
/// </summary>
event EventHandler<CompassChangedEventArgs> CompassChanged;
```

**CompassChangedEventArgs**
```csharp
/// <summary>
/// EventArgs included when CompassChanged Event fires (Returns 0-360 degrees)
/// </summary>
double Heading { get; }
```

### Usage

```csharp
 
CrossCompass.Current.CompassChanged += (s, e) =>
{
    Debug.WriteLine("*** Compass Heading = {0}", e.Heading);
    
    label.Text = $"Heading = {e.Heading}";
   
};

CrossCompass.Current.Start();
```

#### Contributors
* [cbartonnh](https://github.com/JarleySoft)
* [James Montemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under MIT see License file
