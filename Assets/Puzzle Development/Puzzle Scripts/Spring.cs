using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float springStrength;
    [Space(20)]
    [Header("References")]
    [SerializeField] private BallDetector detector;

    public void SpringBounce()
    {
        FoodBall ball = detector.ball;
        ball.rb.AddForce(springStrength * transform.up);
        ball.BallHit();
    }
}