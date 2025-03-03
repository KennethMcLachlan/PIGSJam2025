using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float springStrength;
    [Space(20)]
    [Header("References")]
    [SerializeField] private BallDetector detector;
    [SerializeField] private Animator springAnim;
    [SerializeField] private RandomizedSound bounceSound;

    public void SpringBounce()
    {
        BallCreature ball = detector.ball;
        ball.rb.AddForce(springStrength * transform.up);
        springAnim.SetTrigger("Bounce");
        bounceSound.PlaySound();
    }
}