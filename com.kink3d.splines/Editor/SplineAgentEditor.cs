using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(SplineAgent))]
	public class SplineAgentEditor : Editor 
	{
#region Styles
		internal class Styles
        {
			private static string loopModeTooltip = @"None: Do not loop.
Loop: Return to start of spline after completion
Ping Pong: Move along spline backwards after completion";

			public static GUIContent splineText = EditorGUIUtility.TrTextContent("Spline", "Spline object to use for evaluation.");
			public static GUIContent speedText = EditorGUIUtility.TrTextContent("Speed", "Speed to move along the Spline.");
			public static GUIContent loopModeText = EditorGUIUtility.TrTextContent("Loop Mode", loopModeTooltip);
			public static GUIContent playOnAwakeText = EditorGUIUtility.TrTextContent("Play On Awake", "Evaluate the Spline immediately when the Agent wakes.");
			public static GUIContent resetOnCompleteText = EditorGUIUtility.TrTextContent("Reset On Complete", "Reset the Agent position to the start of the Spline on completion.");
        }
#endregion

#region Serialized Properties
        SerializedProperty m_SplineProp;
		SerializedProperty m_SpeedProp;
		SerializedProperty m_LoopModeProp;
		SerializedProperty m_PlayOnAwakeProp;
		SerializedProperty m_ResetOnCompleteProp;
#endregion

#region Initializtion
		private void OnEnable()
        {
			m_SplineProp = serializedObject.FindProperty("m_Spline");
			m_SpeedProp = serializedObject.FindProperty("m_Speed");
			m_LoopModeProp = serializedObject.FindProperty("m_LoopMode");
			m_PlayOnAwakeProp = serializedObject.FindProperty("m_PlayOnAwake");
			m_ResetOnCompleteProp = serializedObject.FindProperty("m_ResetOnComplete");
        }
#endregion

#region InspectorGUI
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(m_SplineProp, Styles.splineText);
			EditorGUILayout.PropertyField(m_SpeedProp, Styles.speedText);
			EditorGUILayout.PropertyField(m_LoopModeProp, Styles.loopModeText);
			EditorGUILayout.PropertyField(m_PlayOnAwakeProp, Styles.playOnAwakeText);
			EditorGUILayout.PropertyField(m_ResetOnCompleteProp, Styles.resetOnCompleteText);
			serializedObject.ApplyModifiedProperties();
		}
#endregion
	}
}
