using UnityEngine;

namespace kTools.Splines
{
	public static class DebugUtil
	{
#region Draw
		/// <summary>
        /// Draw a line gizmo between two Points.
        /// </summary>
        /// <param name="pointA">Start point.</param>
		/// <param name="pointB">End point.</param>
		/// <param name="color">DebugColor for the wire.</param>
		public static void DrawLine(Vector3 pointA, Vector3 pointB, DebugColors.DebugColor color)
		{
			// Draw a line gizmo
			Gizmos.color = color.wire;
			Gizmos.DrawLine(pointA, pointB);
		}

		/// <summary>
        /// Draw a handle gizmo for a Point.
        /// </summary>
        /// <param name="point">Point to draw handle for.</param>
		/// <param name="direction">Handle direction.</param>
		/// <param name="color">DebugColor for the handle.</param>
		public static void DrawHandle(Point point, Direction direction, DebugColors.DebugColor color)
		{
			// Draw a Handle gizmo
			var endPoint = point.GetHandle(direction);
			Gizmos.color = color.wire;
			Gizmos.DrawLine(point.transform.position, endPoint);
			Gizmos.DrawSphere(endPoint, 0.05f);
		}
#endregion
	}

	public static class DebugColors
	{
#region Definitions
		public static DebugColor spline = new DebugColor(new Vector4(0.43f, 0.81f, 0.96f, 1.0f), new Vector4(0.43f, 0.81f, 0.96f, 0.25f));
		public static DebugColor handle = new DebugColor(new Vector4(0.99f, 0.82f, 0.64f, 1.0f), new Vector4(0.99f, 0.82f, 0.64f, 0.5f));
		public static DebugColor white = new DebugColor(new Vector4(1.00f, 1.00f, 1.00f, 1.0f), new Vector4(1.00f, 1.00f, 1.00f, 0.5f));
		public static DebugColor black = new DebugColor(new Vector4(0.00f, 0.00f, 0.00f, 1.0f), new Vector4(0.00f, 0.00f, 0.00f, 0.5f));
#endregion

#region Data Structures
		public struct DebugColor
		{
			public Vector4 wire;
			public Vector4 fill;

			public DebugColor(Vector4 wire, Vector4 fill)
			{
				// Define a new DebugColor
				this.wire = wire;
				this.fill = fill;
			}
		}
#endregion
	}
}
