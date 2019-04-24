using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CustomEditor(typeof(Spline))]
	public class SplineEditor : Editor 
	{
#region InspectorGUI
		public override void OnInspectorGUI()
		{
			Spline actualTarget = (Spline)target;
		}
#endregion
	}
}
