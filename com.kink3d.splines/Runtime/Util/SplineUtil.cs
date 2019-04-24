using UnityEngine;

namespace kTools.Splines
{
	internal static class SplineUtil
	{
#region Evaluation
		internal static Vector3 EvaluateSplineSegment(Point pointA, Point pointB, float t)
		{
			var omt = 1.0f - t;
			var omt2 = omt * omt;
			var t2 = t * t;

			// Get position in world space of value t between points A and B
			return pointA.transform.position * ( omt2 * omt ) +
			pointA.GetHandle(Direction.Forward) * ( 3.0f * omt2 * t ) +
			pointB.GetHandle(Direction.Backward) * ( 3.0f * omt * t2 ) +
			pointB.transform.position * ( t2 * t );
		}
#endregion
	}
}
