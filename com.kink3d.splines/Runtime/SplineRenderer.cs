using UnityEngine;
using kTools.Splines;

namespace kTools.Splines
{
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class SplineRenderer : MonoBehaviour
    {
#region Components
        private LineRenderer m_LineRenderer;

        /// <summary>
        /// LineRenderer component to use for rendering.
        /// </summary>
        public LineRenderer lineRenderer
        {
            get
            {
                if(m_LineRenderer == null)
                    m_LineRenderer = GetComponent<LineRenderer>();
                return m_LineRenderer;
            }
        }
#endregion

#region Properties
        /// <summary>
        /// Spline object to use for rendering.
        /// </summary>
        public Spline spline;

        /// <summary>
        /// Amount of segments to use when rendering.
        /// </summary>
        public int segments = 64;
#endregion

#region Data
        private Vector3[] m_Points;
#endregion

#region Update
        [ExecuteInEditMode]
        private void Update()
        {
            // If no Spline return
            if(spline == null)
                return;

            // Render Spline
            RenderSpline();
        }
#endregion

#region Spline
        private void RenderSpline()
        {
            // If no segments return
            if(segments == 0)
                return;

            // Create points array
            int pointCount = segments + 1;
            m_Points = new Vector3[pointCount];

            // Evaluate Spline
            for(int i = 0; i < pointCount; i++)
            {
                float t = (float)i / (float)pointCount;
                SplineValue value = spline.Evaluate(t, false);
                m_Points[i] = value.position;
            }

            // Set LineRenderer
            lineRenderer.positionCount = pointCount;
            lineRenderer.SetPositions(m_Points);
        }
#endregion
    }
}
