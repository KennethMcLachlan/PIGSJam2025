using UnityEngine;

public class TubeDoor : MonoBehaviour
{
    [SerializeField] private RandomizedSound openSound;
    [SerializeField] private RandomizedSound closeSound;
    [SerializeField] private Animator doorAnim;

    public void Open()
    {
        openSound.PlaySound();
        doorAnim.SetBool("Open", true);
    }

    public void Close()
    {
        closeSound.PlaySound();
        doorAnim.SetBool("Open", false);
    }
}