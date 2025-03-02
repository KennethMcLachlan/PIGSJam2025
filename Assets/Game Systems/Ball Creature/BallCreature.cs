using System.Collections;
using UnityEngine;

public class BallCreature : MonoBehaviour
{
    #region Variables & References
    private bool landed;
    [SerializeField] private bool immune;
    [SerializeField] private BallDetector detector;

    [Header("Audio")]
    [SerializeField] private RandomizedSound screamSound;
    [SerializeField] private RandomizedSound hitSound;
    [SerializeField] private RandomizedSound burstSound;
    [SerializeField] private RandomizedSound eatenSound;

    [Header("References")]
    public Rigidbody rb;
    [SerializeField] private Collider ballColl;
    [SerializeField] private MeshRenderer ballMesh;
    [SerializeField] private ParticleSystem burstParticles;
    #endregion

    #region Configuration
    private void OnEnable()
    {
        rb.isKinematic = false;
        landed = false;
        screamSound.PlaySound();
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.forward);
    }
    #endregion

    #region Collision
    private void OnCollisionEnter(Collision collision)
    {
        if (!landed)
        {
            detector = collision.collider.GetComponentInParent<BallDetector>();
            if (detector != null)
            {
                hitSound.PlaySound();
                screamSound.PlaySound();
                detector.BallDetected(this);
            }
            else
            {
                if (!immune)
                {
                    StartCoroutine(nameof(Burst));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!landed)
        {
            Mouth mouth = other.GetComponentInParent<Mouth>();
            if (mouth != null)
            {
                mouth.MouthFed();
                StartCoroutine(Eaten());
            }
            else
            {
                BallImmunityTrigger immunityTrigger = other.GetComponentInParent<BallImmunityTrigger>();
                if(immunityTrigger != null)
                {
                    immune = true;
                    StopAllCoroutines();
                    StartCoroutine(Immune(immunityTrigger.immunityTime));
                }

                detector = other.GetComponentInParent<BallDetector>();
                if (detector != null)
                {
                    detector.BallDetected(this);
                }
                else
                {
                    StartCoroutine(nameof(Burst));
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!landed && detector != null)
        {
            detector.BallLost();
        }
    }
    private IEnumerator Immune(float time)
    {
        yield return new WaitForSeconds(time);
        immune = false;
    }

    private IEnumerator Burst()
    {
        if(!immune)
        {
            landed = true;
            if (detector != null)
            {
                detector.BallLost();
            }
            transform.parent = null;
            ballColl.enabled = false;
            rb.isKinematic = true;
            ballMesh.enabled = false;

            screamSound.StopSound();
            burstSound.PlaySound();

            burstParticles.Play();
            yield return new WaitForSeconds(6);
            Destroy(this.gameObject);
            yield return null;
        }
    }

    private IEnumerator Eaten()
    {
        landed = true;
        yield return new WaitForSeconds(0.07f);
        rb.isKinematic = true;
        screamSound.StopSound();
        burstSound.PlaySound();
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
        yield return null;
    }
    #endregion
}