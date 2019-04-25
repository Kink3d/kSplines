using UnityEngine;

namespace kTools.Splines
{
	internal static class SplineUtil
	{
#region Computation
		struct GaussLengendreCoefficient
		{
			public float abscissa;
			public float weight;

			public GaussLengendreCoefficient(float abscissa, float weight)
			{
				this.abscissa = abscissa;
				this.weight = weight;
			}
		};

		internal static float GetSplineSegmentLength(Point start, Point end)
		{
			Vector3 GetDerivative(float t)
			{
				// Cubic Hermite spline derivative coeffcients
				Vector3 c0 = start.GetHandle(Direction.Forward);
				Vector3 c1 = 6.0f * (end.position - start.position) - 4.0f * start.GetHandle(Direction.Forward) - 2.0f * end.GetHandle(Direction.Forward);
				Vector3 c2 = 6.0f * (start.position - end.position) + 3.0f * (start.GetHandle(Direction.Forward) + end.GetHandle(Direction.Forward));

				return c0 + t * (c1 + t * c2);
			}

			GaussLengendreCoefficient[] coefficients =
			{
				new GaussLengendreCoefficient(0.0f, 0.5688889f),
				new GaussLengendreCoefficient(-0.5384693f, 0.47862867f),
				new GaussLengendreCoefficient(0.5384693f, 0.47862867f),
				new GaussLengendreCoefficient(-0.90617985f, 0.23692688f),
				new GaussLengendreCoefficient(0.90617985f, 0.23692688f),
			};

			float length = 0.0f;
			foreach(GaussLengendreCoefficient coefficient in coefficients)
			{
				// This and the final (0.5 *) below are needed for a change of interval to [0, 1] from [-1, 1]
				float t = 0.5f * (1.0f + coefficient.abscissa); 
				length += GetDerivative(t).magnitude * coefficient.weight;
			}
			return 0.5f * length;
		}
#endregion

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
