using System.Reflection;
using TweenSharp.Animation;

namespace TweenSharp.Factory
{
    public abstract class TweenTargetBase<TTarget, TValue, TTimeline> where TTimeline : Timeline
    {
        internal TTarget Target { get; }

        protected TweenProgress.ProgressFunction<TValue> ProgressFunction { get; }

        protected TweenTargetBase(TTarget target, TweenProgress.ProgressFunction<TValue> progressFunction)
        {
            Target = target;
            ProgressFunction = progressFunction;
        }

        public abstract TTimeline To(TValue toValue);

        public abstract TTimeline FromTo(TValue toValue);

        public abstract TTimeline From(TValue fromValue);

        protected Tween<TTarget, TValue> CreateTween(TValue value, TweenDirection tweenDirection, PropertyInfo prop)
        {
            void Setter(TValue x) => prop.SetValue(Target, x);
            TValue Getter() => (TValue) prop.GetValue(Target);
            return new Tween<TTarget, TValue>(Target, ProgressFunction, Setter, Getter, value, tweenDirection);
        }
    }
}