using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using kTools.Splines;

namespace kTools.Splines.Tests
{
    internal class SplineAgentTests
    {
        [UnityTest]
        public IEnumerator CanCreateSplineAgent()
        {
            Spline spline = Spline.Create();
            SplineAgent splineAgent = new GameObject("SplineAgent", typeof(SplineAgent)).GetComponent<SplineAgent>();

            yield return new WaitForFixedUpdate();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineAgent, "SplineAgent was null.");
        }

        [UnityTest]
        public IEnumerator CanEvaluateSplineAgentNone()
        {
            Spline spline = Spline.Create();
            SplineAgent splineAgent = new GameObject("SplineAgent", typeof(SplineAgent)).GetComponent<SplineAgent>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineAgent, "SplineAgent was null.");

            splineAgent.spline = spline;
            splineAgent.Execute();

            yield return new WaitForSeconds(1.0f);

            Assert.AreEqual(spline.GetPoint(1).transform.position, splineAgent.transform.position);
        }

        [UnityTest]
        public IEnumerator CanEvaluateSplineAgentLoop()
        {
            Spline spline = Spline.Create();
            SplineAgent splineAgent = new GameObject("SplineAgent", typeof(SplineAgent)).GetComponent<SplineAgent>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineAgent, "SplineAgent was null.");

            splineAgent.spline = spline;
            splineAgent.loopMode = LoopMode.Loop;
            splineAgent.Execute();

            yield return new WaitForSeconds(1.25f);

            Assert.LessOrEqual(Vector3.Distance(spline.Evaluate(0.25f).position, splineAgent.transform.position), 0.1f, "Spline evaluated to wrong position.");
        }

        [UnityTest]
        public IEnumerator CanEvaluateSplineAgentPingPong()
        {
            Spline spline = Spline.Create();
            SplineAgent splineAgent = new GameObject("SplineAgent", typeof(SplineAgent)).GetComponent<SplineAgent>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineAgent, "SplineAgent was null.");

            splineAgent.spline = spline;
            splineAgent.loopMode = LoopMode.PingPong;
            splineAgent.Execute();

            yield return new WaitForSeconds(1.25f);

            Assert.LessOrEqual(Vector3.Distance(spline.Evaluate(0.75f).position, splineAgent.transform.position), 0.1f, "Spline evaluated to wrong position.");
        }

        [UnityTest]
        public IEnumerator CanEvaluateAtSpeed()
        {
            Spline spline = Spline.Create();
            SplineAgent splineAgent = new GameObject("SplineAgent", typeof(SplineAgent)).GetComponent<SplineAgent>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineAgent, "SplineAgent was null.");

            splineAgent.spline = spline;
            splineAgent.speed = 0.5f;
            splineAgent.Execute();

            yield return new WaitForSeconds(1.0f);

            Assert.LessOrEqual(Vector3.Distance(spline.Evaluate(0.5f).position, splineAgent.transform.position), 0.1f, "Spline evaluated to wrong position.");
        }

        [UnityTest]
        public IEnumerator CanResetOnComplete()
        {
            Spline spline = Spline.Create();
            SplineAgent splineAgent = new GameObject("SplineAgent", typeof(SplineAgent)).GetComponent<SplineAgent>();

            Assert.IsNotNull(spline, "Spline was null.");
            Assert.IsNotNull(splineAgent, "SplineAgent was null.");

            splineAgent.spline = spline;
            splineAgent.resetOnComplete = true;
            splineAgent.Execute();

            yield return new WaitForSeconds(1.0f);

            Assert.AreEqual(spline.GetPoint(0).transform.position, splineAgent.transform.position);
        }
    }
}
