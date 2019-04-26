using UnityEditor;
using UnityEngine;
using System.Collections;
using kTools.Splines;

namespace kTools.SplinesEditor
{
    public class PositionPointWindow : EditorWindow
    {
#region Data
        private static Vector2 s_WindowSize = new Vector2(340, 84); 
        private static string s_InfoMessage = @"Create a new Point at a specific position along the Spline. 
Select a location with the Position field then click Create to create the Point and close this Window.";
        private Spline m_Target;
        private float t;
#endregion

#region Initialization
        public void OnEnable()
        {
            // Add callback
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        public void Init(Spline target)
        {
            // Set data
            m_Target = target;

            // Initialize Window
            titleContent = new GUIContent("Create Point");
            minSize = s_WindowSize;
            maxSize = s_WindowSize;
            Show();
        }
#endregion

#region Close
        void OnDestroy()
        {
            // Remove callback
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
#endregion

#region GUI
        private void OnGUI()
        {
            // Draw Window GUI
            EditorGUILayout.HelpBox(s_InfoMessage, MessageType.Info);
            t = EditorGUILayout.Slider("Position", t, 0, 1);
            if(GUILayout.Button("Create"))
                Create();
        }

        private void Create()
        {
            // Create a new Point and close Window
            m_Target.CreatePointAtPosition(t);
            Close();
        }
#endregion

#region Gizmos
#if UNITY_EDITOR
        void OnSceneGUI(SceneView sceneView) 
        {
            // If target is invalid exit
			if(m_Target == null)
				return;

            // Evaluate Spline at t position
            SplineValue splineValue = m_Target.EvaluateWithSegmentLengths(t);

            // Draw sphere gizmo at Point position
            Handles.color = DebugColors.white.wire;
            Handles.SphereHandleCap(0, splineValue.position, Quaternion.identity, 0.1f, EventType.Repaint);

            // Update SceneView
            sceneView.Repaint();
        }
#endif
#endregion
    }
}
