using System;
using System.Collections.Generic;
using TweenSharp.Animation;

namespace TweenSharp
{
    internal static class TweenPool
    {
        private static Dictionary<Type, Queue<object>> Queues { get; } = new Dictionary<Type, Queue<object>>();
        
        public static Tween<TTarget, TValue> Dequeue<TTarget, TValue>(TTarget target, TweenProgress.ProgressFunction<TValue> progressFunction, Action<TValue> valueSetter, Func<TValue> valueGetter, TValue value, TweenDirection tweenDirection)
        {
            if (!Queues.TryGetValue(typeof(Tween<TTarget, TValue>), out var queue))
            {
                queue = new Queue<object>();
                Queues.Add(typeof(Tween<TTarget, TValue>), queue);
            }

            if (queue.Count == 0)
                queue.Enqueue(new Tween<TTarget, TValue>());

            var tween = (Tween<TTarget, TValue>)queue.Dequeue();
            tween.Reset();
            tween.Target = target;
            tween.ProgressFunction = progressFunction;
            tween.ValueSetter = valueSetter;
            tween.ValueGetter = valueGetter;
            tween.TargetValue = value;
            tween.TweenDirection = tweenDirection;
            tween.Init();

            return tween;
        }

        public static void Enqueue<TTarget, TValue>(Tween<TTarget, TValue> tween)
        {
            if (!Queues.ContainsKey(tween.GetType()))
                return;
            Queues[tween.GetType()].Enqueue(tween);
        }
    }
}
