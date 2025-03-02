using System.Collections;
using UnityEngine;

public class BallCreature : MonoBehaviour
{
    #region Variables & References
    private bool landed;
    [SerializeField] private bool immune;
    [SerializeField] private BallDetector detector;

    [Header("Audio")]
    [SerializeField] private AudioClip[] launchClips;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] eatenClips;
    [SerializeField] private AudioClip[] burstClips;
    [SerializeField] private AudioSource ballAudio;

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
        BallLaunched();
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
            PlayClip(burstClips);
            burstParticles.Play();
            yield return new WaitForSeconds(6);
            Destroy(this.gameObject);
            yield return null;
        }
    }

    private IEnumerator Eaten()
    {
        landed = true;
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
        yield return null;
    }
    #endregion

    #region Audio
    public void BallLaunched()
    {
        PlayClip(launchClips);
    }
    public void BallHit()
    {
        PlayClip(hitClips);
    }
    public void BallEaten()
    {
        PlayClip(eatenClips);
    }

    private void PlayClip(AudioClip[] audioClips)
    {
        ballAudio.Stop();
        ballAudio.pitch = Random.Range(0.85f, 1.15f);
        ballAudio.clip = audioClips[Random.Range(0, audioClips.Length)];
        ballAudio.Play();
    }
    #endregion
}