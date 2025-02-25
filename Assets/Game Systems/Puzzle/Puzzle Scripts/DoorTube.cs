using UnityEngine;

public class DoorTube : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private GameObject exitedRoom;
    [SerializeField] private GameObject enteredRoom;
    [Space(20)]
    [Header("Door Parts")]
    [SerializeField] private Animator exitDoorAnim;
    [SerializeField] private GameObject tubeExtenstion;
    [SerializeField] private Animator enterDoorAnim;

    private void Awake()
    {
        //only show room exit
        tubeExtenstion.SetActive(false);
        enterDoorAnim.gameObject.SetActive(false);
    }

    public void OpenExit()
    {
        //exit opened, show full tube
        tubeExtenstion.SetActive(true);
        exitDoorAnim.gameObject.SetActive(true);
        exitDoorAnim.SetBool("Open", true);
    }

    public void CloseExit()
    {
        //entered tube, close door behind player
        exitDoorAnim.SetBool("Open", false);
        Invoke(nameof(DisableExitRoom), 1.5f);
    }
    private void DisableExitRoom()
    {
        //exit door closed, disable exited room
        exitedRoom.SetActive(false);
    }

    public void OpenEntrance()
    {
        //open door to next room
        enteredRoom.SetActive(true);
        enterDoorAnim.SetBool("Open", true);
    }
    
    public void CloseEntrance()
    {
        //entered new room, close door behind player
        enterDoorAnim.SetBool("Open", false);
        Invoke(nameof(TubeClosed), 1.5f);
    }
    private void TubeClosed()
    {
        //disable unseen parts of tube
        tubeExtenstion.SetActive(false);
        exitDoorAnim.gameObject.SetActive(false);
    }
}