using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace kTools.Splines
{
    [AddComponentMenu("kTools/Spline")]
    public class Spline : MonoBehaviour
    {
#region Data
#if UNITY_EDITOR
        private static int s_DebugSegments = 20;
#endif

        [SerializeField]
        private List<Point> m_Points;
#endregion

#region Initialization
#if UNITY_EDITOR
        [MenuItem("GameObject/kTools/Spline", false, 10)]
        static void CreateSpline(MenuCommand menuCommand)
        {
            // Add a menu item for creating Splines
            // Parent, register undo and select
            GameObject go = new GameObject("Spline", typeof(Spline));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create Spline");
            Selection.activeObject = go;

            // Initiailize Spline
            Spline spline = go.GetComponent<Spline>();
            spline.Init();
        }

        private void Reset()
        {
            // Add validate to undo callback
            Undo.undoRedoPerformed += ValidateSpline;
        }

        private void OnDestroy()
        {
            // Remove validate from undo callback
            Undo.undoRedoPerformed -= ValidateSpline;
        }
#endif

        private void Init()
        {
            // Initialize data
            m_Points = new List<Point>();

            // Create initial points for convenience
            CreatePointNoValidate(transform.position, transform.rotation, 0);
			CreatePointNoValidate(transform.position + transform.forward, transform.rotation, 1);

            // Finalize
            ValidateSpline();
        }
#endregion

#region Validation
        void ValidateSpline()
        {
            // Validate Points
            for(int i = 0; i < m_Points.Count; i++)
            {
                m_Points[i].gameObject.name = string.Format("Point{0}", i);
                m_Points[i].UpdateHandles(i, m_Points.Count);
            }
        }
#endregion

#region Evaluation
        /// <summary>
        /// Evaluate a position along the Spline using normalized segment lengths.
        /// </summary>
        /// <param name="t">Position along the Spline to evaluate.</param>
        /// <param name="loop">Allow the t value to loop for inputs above 1.</param>
        public SplineValue EvaluateWithNormalizedSegments(float t, bool loop = false)
		{
            // Validate points
            if(m_Points == null || m_Points.Count == 0)
            {
                Debug.LogError("Invalid point list");
                return new SplineValue();
            }

            // Use fractional part for looping
            if(loop)
                t = t % 1;

			// Get segment count
			// Get current segment and T value within it
			var segmentCount = m_Points.Count - 1;
			var segment = (int)Mathf.Floor((float)segmentCount * t);
			var segmentT = segmentCount * t - segment;

            // Reached end of Spline
            if(segment == segmentCount)
            {
                return new SplineValue()
                {
                    position = m_Points[segment].transform.position,
                    normal = SplineUtil.EvaluateSplineSegmentNormal(m_Points[segment], m_Points[segment], segmentT),
                    segment = segment,
                };
            }

            // Evaluate Spline segment
            return new SplineValue()
            {
                position = SplineUtil.EvaluateSplineSegment(m_Points[segment], m_Points[segment + 1], segmentT),
                normal = SplineUtil.EvaluateSplineSegmentNormal(m_Points[segment], m_Points[segment + 1], segmentT),
                segment = segment,
            };
		}

        /// <summary>
        /// Evaluate a position along the Spline using accurate segment lengths.
        /// </summary>
        /// <param name="t">Position along the Spline to evaluate.</param>
        /// <param name="loop">Allow the t value to loop for inputs above 1.</param>
        public SplineValue EvaluateWithSegmentLengths(float t, bool loop = false)
		{
            // Validate points
            if(m_Points == null || m_Points.Count == 0)
            {
                Debug.LogError("Invalid point list");
                return new SplineValue();
            }

            // Use fractional part for looping
            if(loop)
                t = t % 1;

            // Get segment count
            var segmentCount = m_Points.Count - 1;

            // Get length data for Spline
            void GetLengthData(out float[] segments, out float spline)
            {
                segments = new float[segmentCount];
                spline = 0;
                for(int i = 0; i < segmentCount; i++)
                {
                    segments[i] = SplineUtil.GetSplineSegmentLength(m_Points[i], m_Points[i+1]);
                    spline += segments[i];
                }
            }

            // Get length data and t position in Spline
            float[] segmentLengths;
            float splineLength;
            GetLengthData(out segmentLengths, out splineLength);
            float positionInSpline = Mathf.Lerp(0, splineLength, t);

            // Get segment count
			// Get current segment index and T value within it
            var segment = 0;
            var segmentT = 0.0f;
            var minLength = 0.0f;
            for(int i = 0; i < segmentLengths.Length; i++)
            {  
                if(positionInSpline > minLength + segmentLengths[i])
                {
                    minLength += segmentLengths[i];
                    continue;
                }
                
                segment = i;
                segmentT = (positionInSpline - minLength) / segmentLengths[i];
                break;
            }

            // Reached end of Spline
            if(segment == segmentCount)
            {
                return new SplineValue()
                {
                    position = m_Points[segment].transform.position,
                    normal = SplineUtil.EvaluateSplineSegmentNormal(m_Points[segment], m_Points[segment], segmentT),
                    segment = segment,
                };
            }

            // Evaluate Spline segment
            return new SplineValue()
            {
                position = SplineUtil.EvaluateSplineSegment(m_Points[segment], m_Points[segment + 1], segmentT),
                normal = SplineUtil.EvaluateSplineSegmentNormal(m_Points[segment], m_Points[segment + 1], segmentT),
                segment = segment,
            };
		}
#endregion

#region Create Points
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
            Point point = CreatePointNoValidate(position, rotation, m_Points.Count);

            // Finalise
            ValidateSpline();
            return point;
        }

        /// <summary>
        /// Create a new Point at the start of the Spline.
        /// </summary>
        public Point CreatePointAtStart()
        {
            // Get position and rotation at start of Spline
            Transform startPoint = m_Points[0].transform;
            Vector3 position = startPoint.position - startPoint.forward;
            Quaternion rotation = startPoint.rotation;

            // Create new Point
            Point point = CreatePointNoValidate(position, rotation, 0);

            // Finalise
            ValidateSpline();
            return point;
        }

        /// <summary>
        /// Create a new Point at a position along the Spline.
        /// </summary>
        /// <param name="t">Position along the Spline to create the new Point.</param>
        public Point CreatePointAtPosition(float t)
        {
            // Evaluate spline at t position
            SplineValue splineValue = EvaluateWithSegmentLengths(t);
            Quaternion rotation = Quaternion.LookRotation(splineValue.normal);

            // Create new Point
            Point point = CreatePointNoValidate(splineValue.position, rotation, splineValue.segment + 1);

            // Finalise
            ValidateSpline();
            return point;
        }

        private Point CreatePointNoValidate(Vector3 position, Quaternion rotation, int index)
		{
            // Validate index
            if(index < 0 || index > m_Points.Count)
            {
                Debug.LogError(string.Format("Failed to create Point at invalid index {0}", index));
                return null;
            }

#if UNITY_EDITOR
            // Register undo for Spline state before creating Point
            Undo.RegisterCompleteObjectUndo(this, "Create Point");
#endif

            // Create new Point object
            // Set Transform
			GameObject go = new GameObject(string.Format("Point{0}", index), typeof(Point));
			go.transform.SetPositionAndRotation(position, rotation);
            go.transform.SetParent(this.transform);
            go.transform.SetSiblingIndex(index);
			go.transform.localScale = new Vector3(1.0f, 1.0f, 0.25f);

#if UNITY_EDITOR
            // Register undo for Point object creation
            Undo.RegisterCreatedObjectUndo(go, "Create Point");
#endif

            // Initiailize Point
            Point point = go.GetComponent<Point>();
			m_Points.Insert(index, point);

            // Finalise
            return point;
		}
#endregion

#region Remove Points
        /// <summary>
        /// Remove a Point at the end of the Spline.
        /// </summary>
        public void RemovePointAtEnd()
        {
            // Always maintain two points
            if(m_Points.Count <= 2)
                return;

            // Remove Point
            RemovePointNoValidate(m_Points.Count - 1);

            // Finalise
            ValidateSpline();
        }

        /// <summary>
        /// Remove a Point at the start of the Spline.
        /// </summary>
        public void RemovePointAtStart()
        {
            // Always maintain two points
            if(m_Points.Count <= 2)
                return;

            // Remove Point
            RemovePointNoValidate(0);

            // Finalise
            ValidateSpline();
        }

        /// <summary>
        /// Remove a Point by a reference.
        /// </summary>
        /// <param name="point">The Point to remove.</param>
        public void RemovePointByReference(Point point)
        {
            // Always maintain two points
            if(m_Points.Count <= 2)
                return;

            // Remove Point
            RemovePointNoValidate(m_Points.IndexOf(point));

            // Finalise
            ValidateSpline();
        }

        private void RemovePointNoValidate(int index)
        {
            // Validate index
            if(index < 0 || index > m_Points.Count - 1)
            {
                Debug.LogError(string.Format("Failed to remove Point at invalid index {0}", index));
                return;
            }

#if UNITY_EDITOR
            // Register undo for Spline state before creating Point
            Undo.RegisterCompleteObjectUndo(this, "Remove Point");
#endif

            // Remove point
            Point point = m_Points[index];
            m_Points.Remove(point);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(point.gameObject);
#endif
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
