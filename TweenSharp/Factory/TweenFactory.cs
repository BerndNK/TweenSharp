using System;
using System.Linq.Expressions;
using TweenSharp.Animation;

namespace TweenSharp.Factory
{
    public static class TweenFactory
    {
        private static readonly Random Rnd = new Random();

        public static TweenTarget<T, TValue> Tween<T, TValue>(this T target, Expression<Func<T, TValue>> memberExpression, TweenProgress.ProgressFunction<TValue> progressFunction)
        {
            return new TweenTarget<T, TValue>(target, (MemberExpression)memberExpression.Body, progressFunction);
        }

        public static TweenTarget<T, double> Tween<T>(this T target, Expression<Func<T, double>> memberExpression)
        {
            return new TweenTarget<T, double>(target, (MemberExpression)memberExpression.Body, ArithmeticProgressFunction);
        }

        public static TweenTarget<T, int> Tween<T>(this T target, Expression<Func<T, int>> memberExpression)
        {
            return new TweenTarget<T, int>(target, (MemberExpression)memberExpression.Body, ArithmeticProgressFunction);
        }

        public static TweenTarget<T, int> Shake<T>(this T target, Expression<Func<T, int>> memberExpression)
        {
            return new TweenTarget<T, int>(target, (MemberExpression)memberExpression.Body, ShakeProgressFunction);
        }

        public static TweenTarget<T, double> Shake<T>(this T target, Expression<Func<T, double>> memberExpression)
        {
            return new TweenTarget<T, double>(target, (MemberExpression)memberExpression.Body, ShakeProgressFunction);
        }

        public static TweenTarget<T, int> ShakeWithIncreasingIntensity<T>(this T target, Expression<Func<T, int>> memberExpression)
        {
            return new TweenTarget<T, int>(target, (MemberExpression)memberExpression.Body, ShakeWithIncreasingIntensityProgressFunction);
        }

        public static TweenTarget<T, double> ShakeWithIncreasingIntensity<T>(this T target, Expression<Func<T, double>> memberExpression)
        {
            return new TweenTarget<T, double>(target, (MemberExpression)memberExpression.Body, ShakeWithIncreasingIntensityProgressFunction);
        }

        private static int ShakeProgressFunction(int startValue, int endValue, double progress)
        {
            return (int)(startValue + (Rnd.NextDouble() - 0.5) * (1.0 - progress) * endValue);
        }

        private static double ShakeProgressFunction(double startValue, double endValue, double progress)
        {
            return startValue + (Rnd.NextDouble() - 0.5) * (1.0 - progress) * endValue;
        }

        private static int ShakeWithIncreasingIntensityProgressFunction(int startValue, int endValue, double progress)
        {
            if (progress >= 1)
                return startValue;
            return (int)(startValue + (Rnd.NextDouble() - 0.5) * progress * endValue);
        }

        private static double ShakeWithIncreasingIntensityProgressFunction(double startValue, double endValue, double progress)
        {
            if (progress >= 1)
                return startValue;
            return startValue + (Rnd.NextDouble() - 0.5) * progress * endValue;
        }

        private static int ArithmeticProgressFunction(int start, int end, double progress)
        {
            return (int)(start + (end - start) * progress);
        }

        private static double ArithmeticProgressFunction(double start, double end, double progress)
        {
            return (start + (end - start) * progress);
        }
    }
}
