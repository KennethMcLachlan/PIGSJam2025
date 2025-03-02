using System.Collections;
using UnityEngine;

public class Stem : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float enabledVolume = 0.5f;
    [SerializeField] private float fadeInTime;
    private float fadeOutTime = 5;
    [SerializeField] private AudioSource stemSound;

    public void SetStemEnabled(bool enabled)
    {
        if(enabled)
        {
            StartCoroutine(StemFadingIn());
        }
        else
        {
            StartCoroutine(StemFadingOut());
        }
    }

    private IEnumerator StemFadingIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeInTime)
        {
            stemSound.volume = Mathf.Lerp(0f, enabledVolume, elapsedTime / fadeInTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        stemSound.volume = enabledVolume;
        yield return null;
    }

    private IEnumerator StemFadingOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeOutTime)
        {
            stemSound.volume = Mathf.Lerp(enabledVolume, 0f, elapsedTime / fadeOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        stemSound.volume = 0f;
        yield return null;
    }
}