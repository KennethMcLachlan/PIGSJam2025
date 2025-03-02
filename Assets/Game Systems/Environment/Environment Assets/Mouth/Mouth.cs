using UnityEngine;

public class Mouth : MonoBehaviour
{
    [Header("Variables")]
    public bool fed;
    [Space(20)]
    [Header("References")]
    [SerializeField] private LivingRoom room;
    [Space(10)]
    [SerializeField] private Animator mouthAnim;
    [SerializeField] private Collider openCollider;
    [SerializeField] private Collider closedCollider;
    [Space(10)]
    [SerializeField] private RandomizedSound openSound;
    [SerializeField] private RandomizedSound closeSound;

    public void OpenMouth()
    {
        closedCollider.gameObject.SetActive(false);
        mouthAnim.SetBool("Open", true);
        openCollider.gameObject.SetActive(true);
        openSound.PlaySound();
    }

    public void MouthFed()
    {
        mouthAnim.SetBool("Open", false);
        Invoke(nameof(MouthClosed), 0.2f);
        closeSound.PlaySound();
        fed = true;
        room.MouthFed();
    }
    private void MouthClosed()
    {
        openCollider.gameObject.SetActive(false);
        closedCollider.gameObject.SetActive(true);
    }
}