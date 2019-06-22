using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TweenSharp.Animation;

namespace TweenSharp.Factory
{
    public class TweenTargets<TTarget, TValue> : TweenTargetBase<TTarget, TValue, Sequence<Tween<TTarget, TValue>>>
    {
        internal List<PropertyInfo> TargetProperties { get; }

        public TweenTargets(TTarget target, TweenProgress.ProgressFunction<TValue> progressFunction, params PropertyInfo[] propertyInfos) : base(target, progressFunction)
        {
            TargetProperties = propertyInfos.ToList();
        }

        public TweenTargets<TTarget, TValue> And(Expression<Func<TTarget, TValue>> memberExpression)
        {
            var prop = (PropertyInfo)((MemberExpression)memberExpression.Body).Member;
            TargetProperties.Add(prop);
            return this;
        }

        public override Sequence<Tween<TTarget, TValue>> To(TValue toValue)
        {
            return CreateSequence(toValue, TweenDirection.ToValue);
        }

        public override Sequence<Tween<TTarget, TValue>> FromTo(TValue toValue)
        {
            return CreateSequence(toValue, TweenDirection.FromToValue);
        }

        public override Sequence<Tween<TTarget, TValue>> From(TValue fromValue)
        {
            return CreateSequence(fromValue, TweenDirection.FromValue);
        }

        private Sequence<Tween<TTarget, TValue>> CreateSequence(TValue toValue, TweenDirection tweenDirection)
        {
            var tweens = TargetProperties.Select(prop => CreateTween(toValue, tweenDirection, prop)).ToList();
            TimelineOptions options = null;
            foreach (var tween in tweens)
            {
                if (options == null)
                    options = tween.Options;
                else
                    tween.Options = options;
            }
            var sequence = new SequenceOfTarget<Tween<TTarget, TValue>>(Target, tweens.ToArray()) {Options = options};
            return sequence;
        }
    }
}
