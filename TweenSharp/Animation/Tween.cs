using System;

namespace TweenSharp.Animation
{
    public static class TweenProgress
    {
        public delegate TValue ProgressFunction<TValue>(TValue startValue, TValue endValue, double progress);
    }

    public class Tween<TTarget, TValue> : Timeline<Tween<TTarget, TValue>>, ITracksTarget
    {
        public TweenProgress.ProgressFunction<TValue> ProgressFunction { get; set; }
        public Action<TValue> ValueSetter { get; set; }
        public Func<TValue> ValueGetter { get; set; }
        private TValue _startValue;
        private bool _startValueIsSet;

        private bool _isDone;

        protected override bool GetIsDone() => _isDone;

        public TTarget Target { get; set; }
        public TValue TargetValue { get; set; }

        public TValue StartValue {
            get => _startValue;
            set {
                _startValue = value;
                _startValueIsSet = true;
            }
        }

        public TweenDirection TweenDirection { get; set; }

        public Tween()
        {

        }

        public void Reset()
        {
            _startValueIsSet = false;
            RunTime = 0;
            Options = new TimelineOptions();
            IsBlocking = false;
            _isDone = false;
            Priority = 0;
        }

        public void Init()
        {
            if (TweenDirection == TweenDirection.FromValue)
            {
                var crntVal = ValueGetter();
                ValueSetter(TargetValue);
                TargetValue = crntVal;
            }

            if (TweenDirection == TweenDirection.FromToValue)
            {
                _startValue = ValueGetter();
                _startValueIsSet = true;
            }
        }

        public Tween(TTarget target, TweenProgress.ProgressFunction<TValue> progressFunction, Action<TValue> valueSetter, Func<TValue> valueGetter, TValue targetValue, TweenDirection tweenDirection)
        {
            TweenDirection = tweenDirection;
            ProgressFunction = progressFunction;
            ValueSetter = valueSetter;
            ValueGetter = valueGetter;
            TargetValue = targetValue;
            Target = target;
            Init();
        }

        public override void Update(double passedSeconds)
        {
            if (IsDone)
                return;

            passedSeconds *= Options.SpeedMultiplier;
            RunTime += passedSeconds;
            var progress = RunTime / Options.Duration;
            if (progress < 0) return;
            if (progress > 1) progress = 1;

            if (!_startValueIsSet)
            {
                Options.OnBeginHandler?.Invoke(this, Options.OnBeginParams);
                _startValue = ValueGetter();
                _startValueIsSet = true;
            }

            var easedProgress = progress;
            if (Options.EasingFunction != null)
                easedProgress = Options.EasingFunction(progress);

            if (Options.AlwaysOnCurrentValue)
                ValueSetter(ProgressFunction(ValueGetter(), TargetValue, easedProgress));
            else
                ValueSetter(ProgressFunction(StartValue, TargetValue, easedProgress));

            Options.OnUpdateHandler?.Invoke(this, Options.OnUpdateParams);

            if (progress >= 1)
            {
                if (Options.Repeat > 0 || Options.Repeat == -1)
                {
                    if (Options.Repeat != -1)
                        Options.Repeat -= 1;

                    RunTime = -Options.RepeatDelay;

                    if (Options.Yoyo)
                    {
                        var tmp = StartValue;
                        StartValue = TargetValue;
                        TargetValue = tmp;
                        InYoyoToggle = !InYoyoToggle;
                        if (!InYoyoToggle)
                            RunTime = -Options.YoyoDelay;
                    }

                    Options.OnRepeatHandler?.Invoke(this, Options.OnRepeatParams);

                    return;
                }

                Options.OnCompleteHandler?.Invoke(this, Options.OnCompletedParams);
                _isDone = true;
            }

        }

        public bool HasTarget(object equalToThis)
        {
            return Target.Equals(equalToThis);
        }

        public bool TargetIsType<T>() => Target is T;

        public override void Dispose()
        {
            _isDone = true;
        }
    }

    public interface ITracksTarget
    {
        bool HasTarget(object equalToThis);
        bool TargetIsType<T>();
    }

    public enum TweenDirection
    {
        ToValue,
        FromValue,
        FromToValue
    }
}
