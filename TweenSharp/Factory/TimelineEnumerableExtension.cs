using System.Collections.Generic;
using TweenSharp.Animation;

namespace TweenSharp.Factory
{
    public static class TimelineEnumerableExtension
    {
        public static Sequence ToSequence(this IEnumerable<Timeline> timelines)
        {
            return new Sequence(timelines);
        }

        public static Sequence ToSequenceWithTarget(this IEnumerable<Timeline> timelines, object target)
        {
            return new SequenceOfTarget(target, timelines);
        }

        public static Sequence ToSequenceWithTargets(this IEnumerable<Timeline> timelines, params object[] targets)
        {
            return new SequenceOfTargets(timelines, targets);
        }
    }
}
