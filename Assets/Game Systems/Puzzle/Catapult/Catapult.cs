using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField] float velocityMultiplier;
    [SerializeField] private Transform velocityStart;
    [SerializeField] private Transform velocityEnd;
    [Space(20)]
    [SerializeField] private BallDetector detector;
    [SerializeField] private Transform cradle;
    [SerializeField] private List<FoodBall> balls;
    [SerializeField] private List<FoodBall> launchedBalls;
    [SerializeField] private Animator catapultAnim;
    private bool launching;

    public void BallInCatapult()
    {
        if (!launching)
        {
            catapultAnim.SetTrigger("Launch");
            launching = true;
        }
        FoodBall detectedBall = detector.ball;
        if (!balls.Contains(detectedBall))
        {
            balls.Add(detectedBall);
        }
        detectedBall.transform.parent = cradle;
        detectedBall.BallHit();
    }

    public void LaunchBall()
    {
        Vector3 force = velocityMultiplier * (velocityEnd.position - velocityStart.position);

        for(int i = 0; i<balls.Count; i++)
        {
            balls[i].transform.parent = null;
            balls[i].rb.linearVelocity = force;
            balls[i].BallLaunched();
        }

        balls.Clear();
    }

    public void LaunchComplete()
    {
        launching = false;
        if (balls.Count > 0)
        {
            catapultAnim.SetTrigger("Launch");
            launching = true;
        }
    }
}