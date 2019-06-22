# TweenSharp ![alt text](https://github.com/BerndNK/TweenSharp/raw/master/Icon/IconInversed50px.png "TweenSharpLogo")

Provides an easy to use fluent API to quickly generate tweening functions.

# Usage
```C#
// Create a TweenHandler
var handler = new TweenHandler();

// Create a tween
var point = new Point(0, 0); // Point is a class!
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
handler.Update(100); // point.X == 10 (Tween has ended)
```
Ideally you create own handler instance and update it everytime your scene renders.

### Fine tuning
You can use easing functions, delays and concatination of multiple properties.
```C#
point.Tween(x => x.X).And(x => x.Y).To(20).In(1.5).Ease(Easing.BackEaseIn).Delay(0.5);
````
See the Easing.cs class for all available easing methods.

### Events
```C#
point.Tween(x => x.X).To(20).In(0.5).Delay(1.0)
.OnBegin(OnBeginHandler) // gets called when the tween begins. So after 1.0 seconds delay
.OnUpdate(OnUpdateHandler) // gets called whenever the value gets updates. (After the value has been set)
.OnComplete(OnCompleteHandler) // gets called when the tween is completed
.OnCompleteParams(5); // you may also specify method parameters the tween shall call your method with
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

