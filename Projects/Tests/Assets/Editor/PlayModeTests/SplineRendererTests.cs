using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using kTools.Splines;

namespace kTools.Splines.Tests
{
    internal class SplineRendererTests
    {
        [UnityTest]
        public IEnumerator CanCreateSplineRenderer()
        {
            Spline spline = Spline.Create();
            SplineRenderer splineRenderer = new GameObject("SplineRenderer", typeof(SplineRenderer)).GetComponent<SplineRenderer>();

            yield return new WaitForFixedUpdate();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineRenderer, "SplineRenderer was null.");
        }

        [UnityTest]
        public IEnumerator CanCreateLineRenderer()
        {
            Spline spline = Spline.Create();
            SplineRenderer splineRenderer = new GameObject("SplineRenderer", typeof(SplineRenderer)).GetComponent<SplineRenderer>();

            yield return new WaitForFixedUpdate();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineRenderer, "SplineRenderer was null.");
            Assert.IsNotNull(splineRenderer.lineRenderer, "LineRenderer was null.");
        }

        [UnityTest]
        public IEnumerator CanSetSpline()
        {
            Spline spline = Spline.Create();
            SplineRenderer splineRenderer = new GameObject("SplineRenderer", typeof(SplineRenderer)).GetComponent<SplineRenderer>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineRenderer, "SplineRenderer was null.");

            splineRenderer.spline = spline;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Assert.IsNotNull(splineRenderer.lineRenderer, "LineRenderer was null.");
            Assert.AreEqual(splineRenderer.segments + 1, splineRenderer.lineRenderer.positionCount, "LineRenderer had wrong point count.");
        }

        [UnityTest]
        public IEnumerator CanSetSegments()
        {
            Spline spline = Spline.Create();
            SplineRenderer splineRenderer = new GameObject("SplineRenderer", typeof(SplineRenderer)).GetComponent<SplineRenderer>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineRenderer, "SplineRenderer was null.");

            splineRenderer.segments = 32;
            splineRenderer.spline = spline;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            Assert.IsNotNull(splineRenderer.lineRenderer, "LineRenderer was null.");
            Assert.AreEqual(splineRenderer.segments + 1, splineRenderer.lineRenderer.positionCount, "LineRenderer had wrong point count.");
        }
    }
}