using UnityEngine;
using UnityEngine.Events;

public class BallDetector : MonoBehaviour
{
    public FoodBall ball;
    [SerializeField] private UnityEvent ballDetected;

    #region Debugging
    [Space(20)]
    private bool showDebug;
    [SerializeField] private MeshRenderer debugMesh;
#if UNITY_EDITOR
    private void Awake()
    {
        if(debugMesh!= null)
        {
            debugMesh.enabled = true;
        }
    }
#endif
    private void Start()
    {
        if (debugMesh != null)
        {
            debugMesh.enabled = showDebug;
        }
    }
    #endregion

    public void BallDetected(FoodBall detectedBall)
    {
        ball = detectedBall;
        ballDetected.Invoke();
    }
}