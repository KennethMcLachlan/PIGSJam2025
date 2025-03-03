using UnityEngine;

[CreateAssetMenu(fileName = "New FootstepData", menuName = "Footsteps/FootstepData")]
public class FootstepData : ScriptableObject
{
    public AudioClip[] footstepSounds;
}
