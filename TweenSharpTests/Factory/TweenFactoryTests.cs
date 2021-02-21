using System.Globalization;
using System.Text;
using NUnit.Framework;
using TweenSharp;
using TweenSharp.Animation;
using TweenSharp.Factory;

namespace TweenSharpTests.Factory
{
    [TestFixture]
    public class TweenFactoryTests
    {
        private TweenHandler _tweenHandler;
        private StringBuilder _resultStringBuilder;

        [SetUp]
        public void SetUp()
        {
            _tweenHandler = new TweenHandler();
            _resultStringBuilder = new StringBuilder();
        }

        private double ArithmeticProgressFunction(double start, double end, double progress)
        {
            var result = (start + (end - start) * progress);
            _resultStringBuilder.AppendLine(result.ToString("F", CultureInfo.InvariantCulture));
            return result;
        }

        private void RunSimulation(int forSeconds, int timeStepsInMs = 100)
        {
            for (var i = 0; i < forSeconds*10; i++)
            {
                _tweenHandler.Update(timeStepsInMs);
            }
        }

        [Test]
        public void TweenFactoryTest()
        {
            var rect = new Rectangle();

            rect.Tween(x => x.X, ArithmeticProgressFunction).And(x => x.Y).To(5);
            rect.Tween(x => x.X, ArithmeticProgressFunction).To(5);

            var sequence = rect.Tween(x => x.X, ArithmeticProgressFunction).And(x => x.Y).To(5).In(2);
            var tween = rect.Tween(x => x.Z, ArithmeticProgressFunction).To(50).In(5).In(5);

            var seq = new Sequence(sequence, tween);
            _tweenHandler.Add(seq);

            RunSimulation(5);

            Assert.AreEqual("0.25\r\n0.25\r\n1.00\r\n0.50\r\n0.50\r\n2.00\r\n0.75\r\n0.75\r\n3.00\r\n1.00\r\n1.00\r\n4.00\r\n1.25\r\n1.25\r\n5.00\r\n1.50\r\n1.50\r\n6.00\r\n1.75\r\n1.75\r\n7.00\r\n2.00\r\n2.00\r\n8.00\r\n2.25\r\n2.25\r\n9.00\r\n2.50\r\n2.50\r\n10.00\r\n2.75\r\n2.75\r\n11.00\r\n3.00\r\n3.00\r\n12.00\r\n3.25\r\n3.25\r\n13.00\r\n3.50\r\n3.50\r\n14.00\r\n3.75\r\n3.75\r\n15.00\r\n4.00\r\n4.00\r\n16.00\r\n4.25\r\n4.25\r\n17.00\r\n4.50\r\n4.50\r\n18.00\r\n4.75\r\n4.75\r\n19.00\r\n5.00\r\n5.00\r\n20.00\r\n21.00\r\n22.00\r\n23.00\r\n24.00\r\n25.00\r\n26.00\r\n27.00\r\n28.00\r\n29.00\r\n30.00\r\n31.00\r\n32.00\r\n33.00\r\n34.00\r\n35.00\r\n36.00\r\n37.00\r\n38.00\r\n39.00\r\n40.00\r\n41.00\r\n42.00\r\n43.00\r\n44.00\r\n45.00\r\n46.00\r\n47.00\r\n48.00\r\n49.00\r\n50.00\r\n", _resultStringBuilder.ToString());
        }

        [Test]
        public void PointTweenTest()
        {
            var point = new Rectangle();
            var tween = point.Tween(x => x.X, ArithmeticProgressFunction).To(10).In(1);

            _tweenHandler.Add(tween);

            RunSimulation(1);

            Assert.AreEqual("1.00\r\n2.00\r\n3.00\r\n4.00\r\n5.00\r\n6.00\r\n7.00\r\n8.00\r\n9.00\r\n10.00\r\n", _resultStringBuilder.ToString());
        }
    }
}
