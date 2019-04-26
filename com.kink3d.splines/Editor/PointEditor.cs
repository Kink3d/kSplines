using UnityEngine;
using UnityEditor;
using kTools.Splines;

namespace kTools.SplinesEditor
{
	[CustomEditor(typeof(Point))]
	public class PointEditor : Editor 
	{
#region InspectorGUI
		public override void OnInspectorGUI()
		{
			Point actualTarget = (Point)target;
		}
#endregion
	}
}
