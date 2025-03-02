using System.Collections;
using UnityEngine;

public class MusicManagerOLD : MonoBehaviour
{
    [SerializeField] private float transitionInterval;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private AudioSource[] stems;
    [SerializeField] private float targetVolume;

    #region Testing
    public int testStemToEnable;
    public bool testEnableStem;
    private void Update()
    {
        if (testEnableStem)
        {
            EnableStem(testStemToEnable);
            testEnableStem = false;
        }
    }
    #endregion

    private void Start()
    {
        foreach(AudioSource stem in stems)
        {
            stem.volume = 0;
            stem.Play();
        }
    }

    public void EnableStem(int stemIndex)
    {
        StartCoroutine(EnablingStem(stems[stemIndex]));
    }

    private IEnumerator EnablingStem(AudioSource stem)
    {
        //find interval point
        bool stemPlaying = false;
        while(!stemPlaying)
        {
            if(Time.timeSinceLevelLoad % transitionInterval < 0.02f)
            {
                stemPlaying = true;
            }
            else
            {
                yield return null;
            }
        }

        //fade in stem
        float elapsedTime = 0;
        while(elapsedTime < fadeInDuration)
        {
            stem.volume = Mathf.Lerp(0, targetVolume, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log(stem.clip.name + " Stem Enabled");
        stem.volume = targetVolume;
        yield return null;
    }
}