using UnityEngine;

public class BallVelocityModifier : MonoBehaviour
{
    [SerializeField] float velocityMultiplier;
    [SerializeField] private Transform velocityStart;
    [SerializeField] private Transform velocityEnd;
    [Space(20)]
    [SerializeField] private BallDetector detector;

    public void AddBallVelocity()
    {
        Vector3 force = velocityMultiplier * (velocityEnd.position - velocityStart.position);

        FoodBall ball = detector.ball;
        ball.rb.AddForce(force);
        ball.BallHit();
    }

    public void SetBallVelocity()
    {
        Vector3 velocity = velocityMultiplier * (velocityEnd.position - velocityStart.position);

        FoodBall ball = detector.ball;
        ball.rb.linearVelocity = velocity;
        ball.rb.angularVelocity = Vector3.zero;
        ball.BallHit();
    }
}