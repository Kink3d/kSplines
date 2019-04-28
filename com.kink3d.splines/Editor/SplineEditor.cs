using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Spline))]
	public class SplineEditor : Editor 
	{
#region Styles
		internal class Styles
        {
			public static GUIContent pointsText = EditorGUIUtility.TrTextContent("Points");
			public static GUIContent addText = EditorGUIUtility.TrTextContent("Add", "Add Points to the Spline.");
            public static GUIContent addStartText = EditorGUIUtility.TrTextContent("Start", "Add a new Point before the current start Point.");
            public static GUIContent addPositionText = EditorGUIUtility.TrTextContent("Position", "Open a window to create a new Point at a position along the Spline.");
			public static GUIContent addEndText = EditorGUIUtility.TrTextContent("End", "Add a new Point after the current end Point.");
			public static GUIContent removeText = EditorGUIUtility.TrTextContent("Remove", "Remove Points from the Spline.");
			public static GUIContent removeStartText = EditorGUIUtility.TrTextContent("Start", "Remove the Point at the start of the Spline.");
            public static GUIContent removeSelectText = EditorGUIUtility.TrTextContent("Select", "Open a window to remove a Point by selecting it in the Scene view.");
			public static GUIContent removeEndText = EditorGUIUtility.TrTextContent("End", "Remove the Point at the end of the Spline.");
        }
#endregion

#region Data
		Spline m_ActualTarget;
#endregion

#region InspectorGUI
		public override void OnInspectorGUI()
		{
			// Get target
			m_ActualTarget = (Spline)target;

			// Draw sections
			PointsSection();
		}

		private void PointsSection()
		{
			// Title field
			EditorGUILayout.Space();
			{
				Rect rect = EditorGUILayout.GetControlRect(true);
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), Styles.pointsText, EditorStyles.boldLabel);
			}
			
			// Add Points
			{
				Rect rect = EditorGUILayout.GetControlRect(true);
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), Styles.addText, EditorStyles.label);
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 3, rect.y, rect.width / 6, rect.height), Styles.addStartText))
					m_ActualTarget.CreatePointAtStart();
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 4, rect.y, rect.width / 6, rect.height), Styles.addPositionText))
				{
					PositionPointWindow window = (PositionPointWindow)EditorWindow.GetWindow(typeof(PositionPointWindow));
					window.Init(m_ActualTarget);
				}
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 5, rect.y, rect.width / 6, rect.height), Styles.addEndText))
					m_ActualTarget.CreatePointAtEnd();
			}

			// Remove Points
			{
				Rect rect = EditorGUILayout.GetControlRect(true);
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), Styles.removeText, EditorStyles.label);
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 3, rect.y, rect.width / 6, rect.height), Styles.removeStartText))
					m_ActualTarget.RemovePointAtStart();
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 4, rect.y, rect.width / 6, rect.height), Styles.removeSelectText))
				{
					RemovePointWindow window = (RemovePointWindow)EditorWindow.GetWindow(typeof(RemovePointWindow));
					window.Init(m_ActualTarget);
				}
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 5, rect.y, rect.width / 6, rect.height), Styles.removeEndText))
					m_ActualTarget.RemovePointAtEnd();
			}
		}
#endregion
	}
}
