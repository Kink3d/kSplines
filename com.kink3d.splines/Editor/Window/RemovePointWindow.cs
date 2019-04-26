using UnityEditor;
using UnityEngine;
using System.Collections;
using kTools.Splines;

namespace kTools.SplinesEditor
{
    public class RemovePointWindow : EditorWindow
    {
#region Data
        private static Vector2 s_WindowSize = new Vector2(340, 64); 
        private static string s_InfoMessage = @"Remove an existing Point in the Spline. 
Select a Point in the Scene then click Delete to remove it.";
        private Spline m_Target;
#endregion

#region Initialization
        public void Init(Spline target)
        {
            // Set data
            m_Target = target;

            // Initialize Window
            titleContent = new GUIContent("Remove Point");
            minSize = s_WindowSize;
            maxSize = s_WindowSize;
            Show();
        }
#endregion

#region GUI
        private void OnGUI()
        {
            // Draw Window GUI
            EditorGUILayout.HelpBox(s_InfoMessage, MessageType.Info);
            if(GUILayout.Button("Remove"))
                Remove();
        }

        private void Remove()
        {
            // Test if selection is a Point
            Point point = Selection.activeGameObject.GetComponent<Point>();
            if(point != null)
            {
                // Remove selected Point, close window and reselect Spline
                m_Target.RemovePointByReference(point);
                Close();
                Selection.activeGameObject = m_Target.gameObject;
            }
        }
#endregion
    }
}
