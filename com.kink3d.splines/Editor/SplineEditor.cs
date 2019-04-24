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
			EditorGUILayout.LabelField("Points", EditorStyles.boldLabel);
			
			// Add Points
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Add", EditorStyles.label);
			if(GUILayout.Button("Start"))
				m_ActualTarget.CreatePointAtStart();
			if(GUILayout.Button("Position"))
				m_ActualTarget.CreatePointAtStart();
			if(GUILayout.Button("End"))
				m_ActualTarget.CreatePointAtEnd();
			EditorGUILayout.EndHorizontal();

			// Remove Points
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Remove", EditorStyles.label);
			if(GUILayout.Button("Start"))
				m_ActualTarget.RemovePointAtStart();
			if(GUILayout.Button("Select"))
				m_ActualTarget.RemovePointAtStart();
			if(GUILayout.Button("End"))
				m_ActualTarget.RemovePointAtEnd();
			EditorGUILayout.EndHorizontal();
		}
#endregion
	}
}
