using UnityEngine;

namespace kTools.Splines
{
    public class Point : MonoBehaviour
    {
#region Data
        [SerializeField]
        private bool m_HasForwardHandle;
		
        [SerializeField]
        private bool m_HasBackwardHandle;
#endregion

#region Mutators
        public Vector3 position => transform.position;
#endregion

#region Handles
        internal void UpdateHandles(int index, int pointCount)
        {
            // Update handle requirements
            m_HasBackwardHandle = (index != 0);
            m_HasForwardHandle = (index < pointCount - 1);
        }

        internal Vector3 GetHandle(Direction direction)
		{
            // Get handle vector for the given direction
			var vector = direction == Direction.Forward ? transform.forward : -transform.forward;
			return transform.position + vector * transform.lossyScale.z;
		}
#endregion

#region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
		{
            // Draw sphere gizmo at Point position
			Gizmos.color = DebugColors.handle.wire;
			Gizmos.DrawSphere(transform.position, 0.1f);

            // Draw handle gizmos
			if(m_HasForwardHandle)
				DebugUtil.DrawHandle(this, Direction.Forward, DebugColors.handle);

			if(m_HasBackwardHandle)
				DebugUtil.DrawHandle(this, Direction.Backward, DebugColors.handle);
		}
#endif
#endregion
    }
}
