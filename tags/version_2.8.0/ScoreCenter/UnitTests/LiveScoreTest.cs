using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaPortal.Plugin.ScoreCenter;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class LiveScoreTest
    {
        [Test]
        public void TestScoreDiff()
        {
            var a = ScoreDifference.ListFromString("Paris 1:0 Marseille");
            Assert.AreEqual(4, a.Count());
            Assert.AreEqual(new ScoreDifference("Paris", " "), a[0]);
            Assert.AreEqual(new ScoreDifference("1", ":"), a[1]);
            Assert.AreEqual(new ScoreDifference("0", " "), a[2]);
            Assert.AreEqual(new ScoreDifference("Marseille", ""), a[3]);
            Assert.AreEqual("Paris 1:0 Marseille", ScoreDifference.StringFromList(a, "*"));

            BaseScore sc = new GenericScore();
            a = sc.GetDifferences("Paris 0:0 Marseille", "Paris 1:0 Marseille");
            Assert.AreEqual("Paris **1**:0 Marseille", ScoreDifference.StringFromList(a, "**"));
        }
    }
}
