## Compass Plugin
Provides and simple way to access the compass on iOS, Android, and Windows. Returns 0-360 degrees

### Setup
* Available on NuGet: https://www.nuget.org/packages/Plugin.Compass
* Install into your PCL project and Client projects.

**Platform Support**

|Platform|Version|
| ------------------- | :-----------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.Android|API 14+|
|Windows 10 UWP|10+|

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
* [Carl Barton](https://github.com/JarleySoft)
* [James Montemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under MIT see License file
