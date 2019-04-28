using System.Collections;
using UnityEngine;
using kTools.Splines;

namespace kTools.Splines
{
    [AddComponentMenu("kTools/Spline Agent")]
    public class SplineAgent : MonoBehaviour
    {
#region Properties
        /// <summary>
        /// Spline object to use for evaluation.
        /// </summary>
        public Spline spline
        {
            get => m_Spline;
            set => m_Spline = value;
        }

        /// <summary>
        /// Speed to move along the spline.
        /// </summary>
        public float speed
        {
            get => m_Speed;
            set => m_Speed = value;
        }

        /// <summary>
        /// None: Do not loop.
        /// Loop: Return to start of spline after completion
        /// Ping Pong: Move along spline backwards after completion
        /// </summary>
        public LoopMode loopMode
        {
            get => m_LoopMode;
            set => m_LoopMode = value;
        }

        /// <summary>
        /// Evaluate the Spline immediately when the Agent wakes.
        /// </summary>
        public bool playOnAwake
        {
            get => m_PlayOnAwake;
            set => m_PlayOnAwake = value;
        }

        /// <summary>
        /// Reset the Agent position to the start of the spline on completion.
        /// </summary>
        public bool resetOnComplete
        {
            get => m_ResetOnComplete;
            set => m_ResetOnComplete = value;
        }
    #endregion

#region Data
        [SerializeField] private Spline m_Spline;
        [SerializeField] private float m_Speed = 1.0f;
        [SerializeField] private LoopMode m_LoopMode = LoopMode.None;
        [SerializeField] private bool m_PlayOnAwake = false;
        [SerializeField] private bool m_ResetOnComplete = false;
        private Direction m_Direction = Direction.Forward;
#endregion

#region Initialization
        private void Awake()
        {
            // Play on Awake
            if(playOnAwake)
                Execute();
        }
#endregion

#region Execution
        /// <summary>
        /// Evaluate the Agent along the Spline.
        /// </summary>
        public void Execute()
        {
            StartCoroutine(ExecutionEnumerator());
        }

        private IEnumerator ExecutionEnumerator()
        {
            // If theres no Spline dont calculate
            if(spline == null)
                yield break;
            
            // Move along Spline
            float time = 0.0f;
            while(time < 1.0f)
            {
                // Get t value
                time += Time.deltaTime * speed;
                float t = Mathf.Clamp(time, 0.0f, 1.0f);

                // Set direction
                if(m_Direction == Direction.Backward)
                    t = 1.0f - t;
                
                // Evaluate and position
                bool looping = loopMode == LoopMode.Loop;
                SplineValue value = spline.Evaluate(t, looping);
                transform.position = value.position;
                yield return null;
            }
            
            // If ping pong change direction
            if(loopMode == LoopMode.PingPong)
                m_Direction = m_Direction == Direction.Forward ? Direction.Backward : Direction.Forward;

            // If looping continue
            if(loopMode != LoopMode.None)
                StartCoroutine(ExecutionEnumerator());

            // If reset on complete set back to 0
            else if(resetOnComplete)
            {
                SplineValue value = spline.Evaluate(0, false);
                transform.position = value.position;
            }
        }
#endregion
    }
}
