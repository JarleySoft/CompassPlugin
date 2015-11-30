## Compass Plugin

## What is this?
Provides and simple way to access the compass on iOS, Android, and Windows. Returns 0-360 degrees

### Setup
* Available on NuGet: https://www.nuget.org/packages/Plugin.Compass
* Install into your PCL project and Client projects.

**Supports**
* iOS
* iOS Unified
* Android
* Windows Phone 8
* Windows Phone 8.1 RT
* Windows Store 8.1 RT
* Universal Windows Apps

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
