using System;
using System.Linq.Expressions;
using System.Reflection;
using TweenSharp.Animation;

namespace TweenSharp.Factory
{
    public class TweenTarget<TTarget, TValue> : TweenTargetBase<TTarget, TValue, Tween<TTarget, TValue>>
    {
        internal PropertyInfo TargetProperty { get; }

        public TweenTarget(TTarget target, MemberExpression memberExpression, TweenProgress.ProgressFunction<TValue> progressFunction) : base(target, progressFunction)
        {
            TargetProperty = (PropertyInfo)memberExpression.Member;
        }

        public TweenTargets<TTarget, TValue> And(Expression<Func<TTarget, TValue>> memberExpression)
        {
            var prop = (PropertyInfo)((MemberExpression)memberExpression.Body).Member;
            return new TweenTargets<TTarget, TValue>(Target, ProgressFunction, TargetProperty, prop);
        }

        public override Tween<TTarget, TValue> To(TValue toValue)
        {
            return CreateTween(toValue, TweenDirection.ToValue, TargetProperty);
        }

        public override Tween<TTarget, TValue> FromTo(TValue toValue)
        {
            return CreateTween(toValue, TweenDirection.FromToValue, TargetProperty);
        }

        public override Tween<TTarget, TValue> From(TValue fromValue)
        {
            return CreateTween(fromValue, TweenDirection.FromValue, TargetProperty);
        }
    }
}