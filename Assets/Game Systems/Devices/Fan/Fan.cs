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
        Debug.Log("Ball Entered Fan");
        StartCoroutine(nameof(BlowingBall));
    }

    public void BallExitedFan()
    {
        Debug.Log("Ball Exited Fan");
        StopAllCoroutines();
        ball = null;
    }

    private IEnumerator BlowingBall()
    {
        while(ball != null)
        {
            ball.rb.AddForce(fanDirection.up * fanStrength);
            yield return new WaitForSeconds(0.0111f);
            yield return null;
        }
        yield return null;
    }
}