using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweenSharp.Animation
{
    public abstract class Timeline : ITimeline
    {
        protected bool InYoyoToggle;
        public double RunTime { get; set; }

        public bool IsDone => GetIsDone();
        protected abstract bool GetIsDone();

        public bool IsBlocking { get; set; }

        public int Priority { get; set; }

        public TimelineOptions Options { get; set; }

        public abstract void Update(double passedSeconds);

        public delegate void TimelineEventHandler(Timeline sender, params object[] parameter);

        public static Timeline Empty => new EmptyTimeline();

        protected Timeline()
        {
            Options = new TimelineOptions();
        }

        private sealed class PriorityRelationalComparer : IComparer<Timeline>
        {
            public int Compare(Timeline x, Timeline y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return x.Priority.CompareTo(y.Priority);
            }
        }

        public static IComparer<Timeline> PriorityComparer { get; } = new PriorityRelationalComparer();

        public async Task AwaitCompletion()
        {
            if (IsDone)
                return;

            if (Options.Repeat < 0)
                throw new Exception("You should not wait for completion for an infinite long tween.");

            while (!IsDone)
            {
                await Task.Delay(100);
            }
        }

        public virtual void Dispose()
        {
        }
    }

    public class EmptyTimeline : Timeline
    {
        protected override bool GetIsDone() => true;

        public override void Update(double passedSeconds)
        {
        }
    }

    public abstract class Timeline<T> : Timeline where T : Timeline<T>
    {
        public T In(double seconds)
        {
            Options.Duration = seconds;
            return this as T;
        }

        public T OnComplete(TimelineEventHandler onCompleteHandler)
        {
            Options.OnCompleteHandler += onCompleteHandler;
            return this as T;
        }

        public T OnCompleteParams(params object[] parameter)
        {
            Options.OnCompletedParams = parameter;
            return this as T;
        }

        public T OnBegin(TimelineEventHandler onCompleteHandler)
        {
            Options.OnBeginHandler += onCompleteHandler;
            return this as T;
        }

        public T OnUpdate(TimelineEventHandler onUpdateHandler)
        {
            Options.OnUpdateHandler += onUpdateHandler;
            return this as T;
        }

        public T Repeat(int repeats)
        {
            Options.Repeat = repeats;
            return this as T;
        }

        public T RepeatDelay(double repeatDelay)
        {
            Options.RepeatDelay = repeatDelay;
            return this as T;
        }

        public T Yoyo(bool doYoyo = true)
        {
            Options.Yoyo = doYoyo;
            return this as T;
        }

        public T OnRepeat(TimelineEventHandler onRepeatHandler)
        {
            Options.OnRepeatHandler += onRepeatHandler;
            return this as T;
        }

        public T Delay(double inSeconds)
        {
            RunTime = -inSeconds;
            return this as T;
        }

        public T Ease(Easing.EasingFunction easingFunction)
        {
            Options.EasingFunction = easingFunction;
            return this as T;
        }

        public T AlwaysOnCurrentValue(bool isActivated = true)
        {
            Options.AlwaysOnCurrentValue = isActivated;
            return this as T;
        }
    }
}