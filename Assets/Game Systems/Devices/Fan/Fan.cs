using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float fanStrength;
    [Space(20)]
    [Header("References")]
    [SerializeField] private Transform fanDirection;
    [Space(20)]
    [SerializeField] private BallDetector detector;
    private BallCreature ball;

    #region Debugging
    private void Awake()
    {
        fanDirection.gameObject.SetActive(false);
    }
    #endregion

    public void BallEnteredFan()
    {
        ball = detector.ball;
    }

    private void FixedUpdate()
    {
        if (ball != null)
        {
            ball.rb.AddForce(fanDirection.up * fanStrength);
        }
    }

    public void BallExitedFan()
    {
        ball = null;
    }
}