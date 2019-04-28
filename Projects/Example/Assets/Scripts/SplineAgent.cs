using System.Collections;
using UnityEngine;
using kTools.Splines;

public class SplineAgent : MonoBehaviour
{
#region Enumerations
    public enum LoopMode
    {
        None,
        Loop,
        PingPong
    }
#endregion

#region Properties
    public Spline spline;
    public float speed = 1.0f;
    public LoopMode loopMode = LoopMode.None;
    public bool playOnAwake = false;
    public bool resetOnComplete = false;
#endregion

#region Data
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
            SplineValue value = spline.EvaluateWithSegmentLengths(t, looping);
            transform.position = value.position;
            yield return null;
        }
        
        // If ping pong change direction
        if(loopMode == LoopMode.PingPong)
            m_Direction = m_Direction == Direction.Forward ? Direction.Backward : Direction.Forward;

        // If looping continue
        if(loopMode != LoopMode.None)
            StartCoroutine(ExecutionEnumerator());

        // If reset on complete
        else if(resetOnComplete)
        {
            SplineValue value = spline.EvaluateWithSegmentLengths(0, false);
            transform.position = value.position;
        }
    }
#endregion
}
