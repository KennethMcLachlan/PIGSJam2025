using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticControls : MonoBehaviour
{
    [SerializeField] private bool testPlayHapticImpulse;
    [SerializeField] private HapticImpulsePlayer hapticPlayer;

    private void Update()
    {
        if (testPlayHapticImpulse)
        {
            PlayHapticImpulse();
            testPlayHapticImpulse = false;
        }
    }

    public void PlayHapticImpulse()
    {
        hapticPlayer.SendHapticImpulse(1, 0.3f);
    }
}