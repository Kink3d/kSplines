using UnityEngine;
using NUnit.Framework;
using kTools.Splines;

namespace kTools.Splines.Tests
{
    internal class SplineTests
    {
        [Test]
        public void CanCreateSpline()
        {
            Spline spline = Spline.Create();

            Assert.IsNotNull(spline, "Spline was null.");
        }

        [Test]
        public void CanInitSpline()
        {
            Spline spline = Spline.Create();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.AreEqual(2, spline.pointCount, "Spline failed to create Points");
        }

        [Test]
        public void CanEvaluateSpline()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplineValue value = spline.Evaluate(0.5f);

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, 0.5f), value.position, "Spline evaluation failed.");
        }

        [Test]
        public void CanCreatePointAtEnd()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplinePoint point = spline.CreatePointAtEnd();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(point, "Created Point was null");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, 2.0f), point.transform.position, "Created Point was at wrong position.");
            Assert.AreEqual(point, spline.GetPoint(2), "Created Point had wrong index.");
        }

        [Test]
        public void CanCreatePointAtStart()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplinePoint point = spline.CreatePointAtStart();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(point, "Created Point was null");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, -1.0f), point.transform.position, "Created Point was at wrong position.");
            Assert.AreEqual(point, spline.GetPoint(0), "Created Point had wrong index.");
        }

        [Test]
        public void CanCreatePointAtPosition()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplinePoint point = spline.CreatePointAtPosition(0.5f);

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(point, "Created Point was null");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, 0.5f), point.transform.position, "Created Point was at wrong position.");
            Assert.AreEqual(point, spline.GetPoint(1), "Created Point had wrong index.");
        }

        [Test]
        public void CanRemovePointAtEnd()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplinePoint point = spline.CreatePointAtEnd();
            spline.RemovePointAtEnd();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.AreEqual(2, spline.pointCount, "Failed to remove Point.");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, 1.0f), spline.GetPoint(1).transform.position, "Removed wrong Point.");
        }

        [Test]
        public void CanRemovePointAtStart()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplinePoint point = spline.CreatePointAtStart();
            spline.RemovePointAtStart();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.AreEqual(2, spline.pointCount, "Failed to remove Point.");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, 0.0f), spline.GetPoint(0).transform.position, "Removed wrong Point.");
        }

        [Test]
        public void CanRemovePointByReference()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            SplinePoint point = spline.CreatePointAtEnd();
            spline.RemovePointByReference(point);

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.AreEqual(2, spline.pointCount, "Failed to remove Point.");
            Assert.AreEqual(new Vector3(0.0f, 0.0f, 1.0f), spline.GetPoint(1).transform.position, "Removed wrong Point.");
        }

        [Test]
        public void CannotReducePointCountBelowTwo()
        {
            Spline spline = Spline.Create();
            spline.transform.position = Vector3.zero;
            spline.RemovePointAtEnd();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.AreEqual(2, spline.pointCount, "Removed invalid Point.");
        }
    }
}
