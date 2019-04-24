using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace kTools.Splines
{
    public class Spline : MonoBehaviour
    {
#region Data
        private static int s_DebugSegments = 20;
        
        [SerializeField]
        private List<Point> m_Points;
#endregion

#region Initialization
        [MenuItem("GameObject/kTools/Spline", false, 10)]
        static void CreateSpline(MenuCommand menuCommand)
        {
            // Add a menu item for creating Splines
            // Parent, register undo and select
            GameObject go = new GameObject("Spline", typeof(Spline));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

            // Initiailize Spline
            Spline spline = go.GetComponent<Spline>();
            spline.Init();
        }

        private void Init()
        {
            // Initialize data
            m_Points = new List<Point>();

            // Create initial points for convenience
            CreatePoint(transform.position, transform.rotation, 0);
			CreatePoint(transform.position + transform.forward, transform.rotation, 1);
        }
#endregion

#region Validation
        private void ValidateSpline()
        {
            // Validate Points
            foreach(Point point in m_Points)
            {
                int index = m_Points.IndexOf(point);
                point.gameObject.name = string.Format("Point{0}", index);
                point.UpdateHandles(index, m_Points.Count);
            }
        }
#endregion

#region Evaluation
        /// <summary>
        /// Evaluate a position along the Spline using normalized segment lengths.
        /// </summary>
        /// <param name="t">Position along the Spline to evaluate.</param>
        public Vector3 EvaluateSplineWithNormalizedSegments(float t)
		{
            // Validate points
            if(m_Points == null || m_Points.Count == 0)
            {
                Debug.LogError("Invalid point list");
                return Vector3.zero;
            }

			// Get segment count
			// Get current segment and T value within it
			var segmentCount = m_Points.Count - 1;
			var currentSegment = (int)Mathf.Floor((float)segmentCount * t);
			var currentSegmentT = segmentCount * t - currentSegment;

            // Reached end of Spline
            if(currentSegment == segmentCount)
                return m_Points[currentSegment].transform.position;

			// Interpolate spline segment
            var startPoint = m_Points[currentSegment];
			var endPoint = m_Points[currentSegment + 1];
			return SplineUtil.EvaluateSplineSegment(startPoint, endPoint, currentSegmentT);
		}
#endregion

#region Points
        /// <summary>
        /// Create a new Point at the end of the Spline.
        /// </summary>
        public Point CreatePointAtEnd()
        {
            // Get position and rotation at end of Spline
            Transform endPoint = m_Points[m_Points.Count - 1].transform;
            Vector3 position = endPoint.position + endPoint.forward;
            Quaternion rotation = endPoint.rotation;

            // Create new Point
            return CreatePoint(position, rotation, m_Points.Count);
        }

        /// <summary>
        /// Create a new Point at the start of the Spline.
        /// </summary>
        public Point CreatePointAtStart()
        {
            // Get position and rotation at end of Spline
            Transform startPoint = m_Points[0].transform;
            Vector3 position = startPoint.position - startPoint.forward;
            Quaternion rotation = startPoint.rotation;

            // Create new Point
            return CreatePoint(position, rotation, 0);
        }

        private Point CreatePoint(Vector3 position, Quaternion rotation, int index)
		{
            // Validate index
            if(index < 0 || index > m_Points.Count)
            {
                Debug.LogError(string.Format("Failed to create Point at invalid index {0}", index));
                return null;
            }

            // Create new Point object
            // Set Transform
			GameObject go = new GameObject(string.Format("Point{0}", index), typeof(Point));
			go.transform.SetPositionAndRotation(position, rotation);
            go.transform.SetParent(this.transform);
            go.transform.SetSiblingIndex(index);
			go.transform.localScale = new Vector3(1.0f, 1.0f, 0.25f);

            // Initiailize Point
            Point point = go.GetComponent<Point>();
			m_Points.Insert(index, point);

            // Finalise
            ValidateSpline();
            return point;
		}

        /// <summary>
        /// Remove a Point at the end of the Spline.
        /// </summary>
        public void RemovePointAtEnd()
        {
            // Remove Point
            RemovePoint(m_Points.Count - 1);
        }

        /// <summary>
        /// Remove a Point at the start of the Spline.
        /// </summary>
        public void RemovePointAtStart()
        {
            // Remove Point
            RemovePoint(0);
        }

        private void RemovePoint(int index)
        {
            // Validate index
            if(index < 0 || index > m_Points.Count - 1)
            {
                Debug.LogError(string.Format("Failed to remove Point at invalid index {0}", index));
                return;
            }

            // Remove point
            Point point = m_Points[index];
            m_Points.Remove(point);
            DestroyImmediate(point.gameObject);

            // Finalise
            ValidateSpline();
        }
#endregion

#region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
		{
			// If all points are invalid exit
			if(m_Points == null || m_Points.Count == 0)
				return;

			Point previousPoint = m_Points[0];
			for(int p = 1; p < m_Points.Count; p++)
			{
				// If next point is null end
				if(m_Points[p] == null)
					return;

				// Draw Spline segment
				Vector3 previousPosition = previousPoint.transform.position;
				for(int i = 1; i <= s_DebugSegments; i++)
				{
					var t = i/(float)s_DebugSegments;
					var position = SplineUtil.EvaluateSplineSegment(previousPoint, m_Points[p], t);
					UnityEditor.Handles.color = DebugColors.spline.wire;
					UnityEditor.Handles.DrawLine(previousPosition, position);
					previousPosition = position;
				}

				// Store previous point for next iteration
				previousPoint = m_Points[p];
			}
		}
#endif
#endregion
    }
}
