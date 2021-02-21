using System;
using System.Collections.Generic;
using System.Linq;
using TweenSharp.Animation;

namespace TweenSharp
{
    public class TweenHandler : HashSet<Timeline>
    {
        public static double DoubleTolerance = 0.000001;

        public bool ForbidRemoving { get; set; }

        /// <summary>
        /// Gets or sets the time modifier, that gets added to each update call.
        /// </summary>
        public double TimeModifier { get; set; } = 1.0;

        public void Update(int passedMs)
        {
            if (Math.Abs(TimeModifier) < DoubleTolerance)
                return;

            var passedSeconds = (Convert.ToDouble(passedMs) / 1000) * TimeModifier;

            var asList = this.ToList();
            foreach (var timeline in asList)
            {
                timeline.Update(passedSeconds);
                if (timeline.IsBlocking)
                    break;
            }
            RemoveAll(x => x.IsDone);
        }

        public void ClearTweensOf<T>(T target)
        {
            if (ForbidRemoving)
                throw new Exception("Removing timelines is forbidden on this instance.");
            RemoveAll(x => x is ITracksTarget targetTracker && targetTracker.HasTarget(target));
        }

        public void ClearTweens(Func<ITracksTarget, bool> predicate)
        {
            if (ForbidRemoving)
                throw new Exception("Removing timelines is forbidden on this instance.");
            RemoveAll(x => x is ITracksTarget targetTracker && predicate(targetTracker));
        }

        public void ClearTweensWithTargetOfType<T>()
        {
            if (ForbidRemoving)
                throw new Exception("Removing timelines is forbidden on this instance.");
            RemoveAll(x => x is ITracksTarget targetTracker && targetTracker.TargetIsType<T>());
        }

        public bool HasTween<T>(T target)
        {
            return this.OfType<ITracksTarget>().Any(x => x.HasTarget(target));
        }

        private void RemoveAll(Predicate<Timeline> match)
        {
            var matches = this.Where(match.Invoke).ToList();
            foreach (var timeline in matches)
            {
                Remove(timeline);
            }
        }
    }
}