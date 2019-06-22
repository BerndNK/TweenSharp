namespace TweenSharp.Animation
{
    public class TimelineOptions
    {
        public double Duration { get; set; }
        public bool Yoyo { get; set; }
        public Timeline.TimelineEventHandler OnCompleteHandler { get; set; }
        public Timeline.TimelineEventHandler OnBeginHandler { get; set; }
        public Timeline.TimelineEventHandler OnUpdateHandler { get; set; }
        public Timeline.TimelineEventHandler OnRepeatHandler { get; set; }
        public object[] OnUpdateParams { get; set; }
        public object[] OnCompletedParams { get; set; }
        public object[] OnRepeatParams { get; set; }
        public object[] OnBeginParams { get; set; }
        public Easing.EasingFunction EasingFunction { get; set; }
        public int Repeat { get; set; }
        public double RepeatDelay { get; set; }
        public double YoyoDelay { get; set; }
        public double SpeedMultiplier { get; set; } = 1;
        public bool AlwaysOnCurrentValue { get; set; }
    }
}