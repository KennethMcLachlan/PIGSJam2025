using UnityEngine;

public class RandomizedSound : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private AudioClip[] clips;
    [Range(0,2)]
    [SerializeField] private float minPitch = 0.85f;
    [Range(0, 2)]
    [SerializeField] private float maxPitch = 1.15f;
    [Space(20)]
    [Header("References")]
    [SerializeField] private AudioSource soundSource;

    public void PlaySound()
    {
        soundSource.Stop();
        soundSource.pitch = Random.Range(minPitch, maxPitch);
        soundSource.clip = clips[Random.Range(0, clips.Length)];
        soundSource.Play();
    }

    public void StopSound()
    {
        soundSource.Stop();
    }
}