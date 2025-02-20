using System.Collections;
using UnityEngine;

public class FoodBall : MonoBehaviour
{
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
    [SerializeField] private ParticleSystem burst;

    #region Configuration
    private void OnEnable()
    {
        rb.isKinematic = false;
        BallLaunched();
    }
    #endregion

    #region Collision
    private void OnCollisionEnter(Collision collision)
    {
        BallDetector detector = collision.transform.GetComponentInParent<BallDetector>();
        if (detector != null)
        {
            detector.BallDetected(this);
        }
        else
        {
            StartCoroutine(nameof(Burst));
        }
    }

    private IEnumerator Burst()
    {
        transform.parent = null;
        ballColl.enabled = false;
        rb.isKinematic = true;
        ballMesh.enabled = false;
        PlayClip(burstClips);
        burst.Play();
        yield return new WaitForSeconds(6);
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