using System.Collections.Generic;
using System.Linq;

namespace TweenSharp.Animation
{
    public class Sequence<TTimeline> : Timeline<Sequence<TTimeline>> where TTimeline : Timeline
    {

        protected override bool GetIsDone() => !Timelines.Any() || Timelines.All(x => x.IsDone);

        public List<TTimeline> Timelines { get; set; }

        public Sequence(IEnumerable<TTimeline> timelines)
        {
            Timelines = timelines.ToList();
#if DEBUG
            if (Timelines.Any(x => x == null))
                throw new Exception("Null timeline pushed into sequence. This is invalid");
#endif
        }

        public override void Update(double passedSeconds)
        {
            if (IsDone)
                return;

            passedSeconds *= Options.SpeedMultiplier;

            foreach (var timeline in Timelines)
            {
                timeline?.Update(passedSeconds);
            }

            foreach (var timeline in Timelines.Where(x => x.IsDone))
            {
                timeline.Dispose();
            }

            Timelines.RemoveAll(x => x.IsDone);

            if (Timelines.Any())
                return;
            
            Options.OnCompleteHandler?.Invoke(this, Options.OnCompletedParams);
        }

        public override void Dispose()
        {
            foreach (var timeline in Timelines)
            {
                timeline.Dispose();
            }
        }
    }

    public class Sequence : Sequence<Timeline>
    {
        public Sequence(IEnumerable<Timeline> timelines) : base(timelines)
        {

        }

        public Sequence(params Timeline[] timelines) : base(timelines)
        {

        }

    }
    public class SequenceOfTarget : Sequence, ITracksTarget
    {
        public SequenceOfTarget(object target, params Timeline[] timelines) : base(timelines)
        {
            Target = target;
        }

        public SequenceOfTarget(object target,  IEnumerable<Timeline> timelines) : base(timelines)
        {
            Target = target;
        }

        public object Target { get; set; }
        public bool HasTarget(object equalToThis)
        {
            return Target == equalToThis;
        }

        public bool TargetIsType<T>() => Target is T;
    }

    public class SequenceOfTargets : Sequence, ITracksTarget
    {

        public SequenceOfTargets(IEnumerable<Timeline> timelines, params object[] targets) : base(timelines)
        {
            Targets = targets;
        }

        public object[] Targets { get; set; }
        public bool HasTarget(object equalToThis)
        {
            return Targets.Any(t => t == equalToThis);
        }

        public bool TargetIsType<T>() => Targets.Any(x => x is T);
    }

    public class SequenceOfTarget<TTimeline> : Sequence<TTimeline>, ITracksTarget where TTimeline : Timeline
    {
        public SequenceOfTarget(object target, params TTimeline[] timelines) : base(timelines)
        {
            Target = target;
        }

        public object Target { get; set; }
        public bool HasTarget(object equalToThis)
        {
            return Target == equalToThis;
        }

        public bool TargetIsType<T>() => Target is T;
    }
}
