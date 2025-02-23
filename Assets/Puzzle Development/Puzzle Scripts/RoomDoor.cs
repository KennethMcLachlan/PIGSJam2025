using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnim;
    public void SetDoorOpen(bool open)
    {
        doorAnim.SetBool("Open", open);
    }
}