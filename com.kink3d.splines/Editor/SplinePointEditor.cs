using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(SplinePoint))]
	public class SplinePointEditor : Editor 
	{
#region InspectorGUI
		public override void OnInspectorGUI()
		{
			SplinePoint actualTarget = (SplinePoint)target;
		}
#endregion
	}
}
