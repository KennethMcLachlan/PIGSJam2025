using UnityEngine;
using UnityEngine.Events;

public class PuzzleRoom : MonoBehaviour
{
    [SerializeField] private UnityEvent playerEnteredRoom;
    [SerializeField] private UnityEvent roomComplete;

    public void PlayerEntered()
    {
        playerEnteredRoom.Invoke();
    }

    public void RoomComplete()
    {
        roomComplete.Invoke();
    }
}