using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float treadmillStrength;
    [Space(20)]
    [Header("References")]
    [SerializeField] private BallDetector detector;
    [SerializeField] private Transform launchDirection;

    public void TreadmillBounce()
    {
        BallCreature ball = detector.ball;
        ball.rb.linearVelocity = treadmillStrength * launchDirection.forward;
    }
}