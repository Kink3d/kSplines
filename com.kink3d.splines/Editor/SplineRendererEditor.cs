using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(SplineRenderer))]
	public class SplineRendererEditor : Editor 
	{
#region Styles
		internal class Styles
        {
            public static GUIContent splineText = EditorGUIUtility.TrTextContent("Spline", "Spline object to use for rendering.");
            public static GUIContent segmentsText = EditorGUIUtility.TrTextContent("Segments", "Amount of segments to use when rendering.");
        }
#endregion

#region Serialized Properties
        SerializedProperty m_SplineProp;
		SerializedProperty m_SegmentsProp;
#endregion

#region Initializtion
		private void OnEnable()
        {
			m_SplineProp = serializedObject.FindProperty("m_Spline");
			m_SegmentsProp = serializedObject.FindProperty("m_Segments");
        }
#endregion

#region InspectorGUI
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(m_SplineProp, Styles.splineText);
			EditorGUILayout.PropertyField(m_SegmentsProp, Styles.segmentsText);
			serializedObject.ApplyModifiedProperties();
		}
#endregion
	}
}
