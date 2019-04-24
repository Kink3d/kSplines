using UnityEngine;

namespace kTools.Splines
{
	internal static class DebugUtil
	{
#region Draw
		internal static void DrawLine(Vector3 pointA, Vector3 pointB, DebugColors.DebugColor color)
		{
			// Draw a line gizmo
			Gizmos.color = color.wire;
			Gizmos.DrawLine(pointA, pointB);
		}

		internal static void DrawHandle(Point point, Direction direction, DebugColors.DebugColor color)
		{
			// Draw a Handle gizmo
			var endPoint = point.GetHandle(direction);
			Gizmos.color = color.wire;
			Gizmos.DrawLine(point.transform.position, endPoint);
			Gizmos.DrawSphere(endPoint, 0.05f);
		}
#endregion
	}

	internal static class DebugColors
	{
#region Definitions
		internal static DebugColor spline = new DebugColor(new Vector4(0.43f, 0.81f, 0.96f, 1.0f), new Vector4(0.43f, 0.81f, 0.96f, 0.25f));
		internal static DebugColor handle = new DebugColor(new Vector4(0.99f, 0.82f, 0.64f, 1.0f), new Vector4(0.99f, 0.82f, 0.64f, 0.5f));
		internal static DebugColor white = new DebugColor(new Vector4(1.00f, 1.00f, 1.00f, 1.0f), new Vector4(1.00f, 1.00f, 1.00f, 0.5f));
		internal static DebugColor black = new DebugColor(new Vector4(0.00f, 0.00f, 0.00f, 1.0f), new Vector4(0.00f, 0.00f, 0.00f, 0.5f));
#endregion

#region Data Structures
		internal struct DebugColor
		{
			internal Vector4 wire;
			internal Vector4 fill;

			internal DebugColor(Vector4 wire, Vector4 fill)
			{
				// Define a new DebugColor
				this.wire = wire;
				this.fill = fill;
			}
		}
#endregion
	}
}
