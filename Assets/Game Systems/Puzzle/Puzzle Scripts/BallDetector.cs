using UnityEngine;
using UnityEngine.Events;

public class BallDetector : MonoBehaviour
{
    [HideInInspector] public FoodBall ball;
    [SerializeField] private UnityEvent ballDetected;
    [SerializeField] private UnityEvent ballLost;

    #region Debugging
    [Space(20)]
    [SerializeField] private MeshRenderer[] debugMeshes;
    private void Awake()
    {
        foreach (MeshRenderer renderer in debugMeshes)
        {
            renderer.enabled = false;
        }
    }
    #endregion

    public void BallDetected(FoodBall detectedBall)
    {
        ball = detectedBall;
        ballDetected.Invoke();
        Debug.Log("Ball Detected");
    }

    public void BallLost()
    {
        ballLost.Invoke();
        Debug.Log("Ball Lost");
    }
}