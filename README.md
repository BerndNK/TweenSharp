# TweenSharp ![alt text](https://github.com/BerndNK/TweenSharp/raw/master/Icon/IconInversed50px.png "TweenSharpLogo")
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/BerndNK/TweenSharp/blob/master/LICENSEE)
[![NuGet](https://img.shields.io/nuget/v/TweenSharp.svg)](https://www.nuget.org/packages/TweenSharp/)


Provides an easy to use fluent API to quickly generate tweening functions.
```
Install-Package TweenSharp
```

# Usage
```C#
// Create a TweenHandler
var handler = new TweenHandler();

// Create a tween
var point = new Point(0, 0); // Point is a class (structs will not work)
var tween = point.Tween(x => x.X) // select which property to tween
                .To(10) // target value
                .In(1.0); // tween time in seconds

// Feed it to the handler
handler.Add(tween);

// Tell the handler that time has passed
handler.Update(100); // progress 100 milliseconds. point.X == 1
handler.Update(100); // point.X == 2
handler.Update(200); // point.X == 4
handler.Update(200); // point.X == 6
handler.Update(200); // point.X == 8
handler.Update(200); // point.X == 10
handler.Update(100); // point.X == 10 (tween has ended)
```
Ideally you create one handler instance and update it everytime your scene renders. For example for WPF the CompositionTarget.Rendering event.

### Fine tuning
You can use easing functions, delays and concatination of multiple properties.
```C#
point.Tween(x => x.X)
.And(x => x.Y) // tween multiple properties at once
.To(20).In(1.5)
.Ease(Easing.BackEaseIn) // specify custom easing functions
.Delay(0.5); // specify a delay before the tweening starts
````
See the Easing.cs class for all available easing methods.

### Sequence
Use .ToSequence() to nest multiple tweens into one
```C#
var tweens = new List<Timeline>(); // timeline is the base class for tweens
tweens.Add(point.Tween(x => x.X).To(20).In(1));
tweens.Add(point.Tween(x => x.Y).To(10).In(1));
handler.Add(tweens.ToSequence()); // a sequence is a single instance which contains multiple tweens
````

### AlwaysOnCurrentValue
React to interrupting value changes.
AlwaysOnCurrentValue tells the tween to always use the current value instead of initializing it once the tweens starts.
```C#
// with AlwaysOnCurrentValue = false (default)
handler.Add(point.Tween(x => x.X).To(10).In(0.5));
handler.Update(100); // point.X == 2, initialized start value with the current value of point.X (0)
handler.Update(100); // point.X == 4
handler.Update(100); // point.X == 6
point.X = 0;
// the current value is calculated with the progress (80%) of the start value (0) and target value (10):
handler.Update(100); // point.X == 8 // note that updating point.X did not have an effect on the tween
handler.Update(100); // point.X == 10

// with AlwaysOnCurrentValue = true
handler.Add(point.Tween(x => x.X).To(10).In(0.5).AlwaysOnCurrentValue(false));
handler.Update(100); // point.X == 2
handler.Update(100); // point.X == 4
handler.Update(100); // point.X == 6
point.X = 0;
// the current value is calculated with the progress (80%) of the CURRENT value (0) and target value (10):
handler.Update(100); // point.X == 5
handler.Update(100); // point.X == 10
````

### Events
```C#
point.Tween(x => x.X).To(20).In(0.5).Delay(1.0)
.OnBegin(OnBeginHandler) // gets called when the tween begins. So after 1.0 seconds delay
.OnUpdate(OnUpdateHandler) // gets called whenever the value gets updates. (After the value has been set)
.OnComplete(OnCompleteHandler) // gets called when the tween is completed
.OnCompleteParams(5); // you may also specify method parameters the tween shall call your method with


// Example
handler.Add(point.Tween(x => x.X).To(10).In(1.0).Delay(0.4));
handler.Update(200); // delay running
handler.Update(200); // delay running
handler.Update(200); // OnBegin called point.X == 2 OnUpdate called
handler.Update(200); // point.X == 4 OnUpdate called
handler.Update(200); // point.X == 6 OnUpdate called
handler.Update(200); // point.X == 8 OnUpdate called
handler.Update(200); // point.X == 10 OnUpdate called, OnComplete called
handler.Update(100); // point.X == 10 (tween has ended)
````
### Repeat
You can specifiy repeat actions.
```C#
point.Tween(x => x.X).To(20).In(0.5).Repeat(1); // executes the point.X -> 20 tween twice
point.Tween(x => x.X).To(20).In(0.5).Repeat(1).Yoyo(true); // tweens point.X -> 20 then point.X -> 0
point.Tween(x => x.X).To(20).In(0.5).Repeat(2).Yoyo(true); // tweens point.X -> 20 -> 0 -> 20

// tweens point.X -> 20 after 0.5 seconds, then point.X -> 0 after 1 second
point.Tween(x => x.X).To(20).In(0.5).Repeat(2).Delay(0.5).RepeatDelay(1.0); 

// there is also a repeat event
point.Tween(x => x.X).To(20).In(0.5).Delay(1.0).Repeat(1)
.OnRepeat(OnRepeatHandler) // gets called after 1.5 seconds (delay + duration)
.OnComplete(OnCompleteHandler); // gets called after 2 seconds (delay + duration + repeat duration)
````

### Time
Through time modifiying you can achieve slow- or fast-motion effects.
```C#
handler.TimeModifier = 0.5; // runs tweens at half speed
handler.TimeModifier = 2; // runs tweens at double speed

// You can even tween this property
handler.Add(handler.Tween(x => x.TimeModifier).To(0.5).In(0.5).Yoyo(true).Repeat(1));
````

### Custom tween operations
You can specify custom functions for tweening properties
```C#
[StructLayout(LayoutKind.Explicit)]
public struct ARGB
{
    [FieldOffset(0)]
    public UInt32 AsUint;
    [FieldOffset(0)]
    public byte a;
    [FieldOffset(1)]
    public byte r;
    [FieldOffset(2)]
    public byte g;
    [FieldOffset(3)]
    public byte b;
}

// tweens each color channel separately
public static UInt32 ColorProgressFunction(UInt32 startValue, UInt32 endValue, double position)
{
    var firstColorArgb = new ARGB { AsUint = startValue };
    var secondColorArgb = new ARGB { AsUint = endValue };

    var argb = new ARGB
    {
        a = (byte)(firstColorArgb.a + (secondColorArgb.a - firstColorArgb.a) * position),
        r = (byte)(firstColorArgb.r + (secondColorArgb.r - firstColorArgb.r) * position),
        g = (byte)(firstColorArgb.g + (secondColorArgb.g - firstColorArgb.g) * position),
        b = (byte)(firstColorArgb.b + (secondColorArgb.b - firstColorArgb.b) * position)
    };

    return argb.AsUint;
}

var someObjectWithColor = new SomeObjectWithColor { Color = 0xff0000ff } // Color is UInt32

// tween blue to red in 1 second
someObjectWithColor(x => x.Color, ColorProgressFunction).To(0xffff0000).In(1.0);
````

