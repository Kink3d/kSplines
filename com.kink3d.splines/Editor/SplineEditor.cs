using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CustomEditor(typeof(Spline))]
	public class SplineEditor : Editor 
	{
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
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), "Points", EditorStyles.boldLabel);
			}
			
			// Add Points
			{
				Rect rect = EditorGUILayout.GetControlRect(true);
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), "Add", EditorStyles.label);
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 3, rect.y, rect.width / 6, rect.height), "Start"))
					m_ActualTarget.CreatePointAtStart();
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 4, rect.y, rect.width / 6, rect.height), "Position"))
				{
					PositionPointWindow window = (PositionPointWindow)EditorWindow.GetWindow(typeof(PositionPointWindow));
					window.Init(m_ActualTarget);
				}
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 5, rect.y, rect.width / 6, rect.height), "End"))
					m_ActualTarget.CreatePointAtEnd();
			}

			// Remove Points
			{
				Rect rect = EditorGUILayout.GetControlRect(true);
				EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), "Remove", EditorStyles.label);
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 3, rect.y, rect.width / 6, rect.height), "Start"))
					m_ActualTarget.RemovePointAtStart();
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 4, rect.y, rect.width / 6, rect.height), "Select"))
				{
					RemovePointWindow window = (RemovePointWindow)EditorWindow.GetWindow(typeof(RemovePointWindow));
					window.Init(m_ActualTarget);
				}
				if(GUI.Button(new Rect(rect.x + (rect.width / 6) * 5, rect.y, rect.width / 6, rect.height), "End"))
					m_ActualTarget.RemovePointAtEnd();
			}
		}
#endregion
	}
}
