using UnityEngine;

public class DoorTube : MonoBehaviour
{
    #region Variables & References
    [Header("Environments")]
    [SerializeField] private DoorTube previousDoorTube;
    [SerializeField] private LivingRoom exitedRoom;
    [Space(10)]
    [SerializeField] private LivingRoom enteredRoom;
    [SerializeField] private DoorTube nextDoorTube;
    [Space(20)]
    [Header("Door Parts")]
    [SerializeField] private TubeDoor exitDoor;
    [SerializeField] private GameObject tubeExtenstion;
    [SerializeField] private TubeDoor enterDoor;
    [Space(30)]
    [Header("Final Door")]
    [SerializeField] private bool finalDoor;
    [SerializeField] private GameObject endRoom;
    #endregion

    #region Testing
#if UNITY_EDITOR
    [Space(20)]
    [Header("Testing")]
    [SerializeField] private bool testOpenExit;
    [SerializeField] private bool testCloseExit;
    [Space(10)]
    [SerializeField] private bool testOpenEntrance;
    [SerializeField] private bool testCloseEntrance;
    private void FixedUpdate()
    {
        if (testOpenExit)
        {
            OpenExit();
            testOpenExit = false;
        }
        if (testCloseExit)
        {
            CloseExit();
            testCloseExit = false;
        }
        if (testOpenEntrance)
        {
            OpenEntrance();
            testOpenEntrance = false;
        }
        if (testCloseEntrance)
        {
            CloseEntrance();
            testCloseEntrance = false;
        }
    }
#endif
    #endregion

    #region Configuration
    private void Awake()
    {
        //only show room exit
        tubeExtenstion.SetActive(false);
        enterDoor.gameObject.SetActive(false);
        if (finalDoor)
        {
            endRoom.SetActive(false);
        }
    }
    #endregion

    #region Door Sequence
    public void OpenExit()
    {
        //exit opened, show full tube
        tubeExtenstion.SetActive(true);
        enterDoor.gameObject.SetActive(true);
        exitDoor.Open();
    }

    public void CloseExit()
    {
        //entered tube, close door behind player
        exitDoor.Close();
        Invoke(nameof(DisableExitRoom), 1.5f);
        //enter final music state with silence for last room
        if (finalDoor)
        {
            FindFirstObjectByType<MusicManager>().StartRoomMusic();
        }
    }
    private void DisableExitRoom()
    {
        //exit door closed, disable exited room
        exitedRoom.gameObject.SetActive(false);
        if (previousDoorTube != null)
        {
            previousDoorTube.gameObject.SetActive(false);
        }
    }

    public void OpenEntrance()
    {
        //open door to next room
        if (finalDoor)
        {
            endRoom.SetActive(true);
        }
        else
        {
            enteredRoom.gameObject.SetActive(true);
            nextDoorTube.gameObject.SetActive(true);
        }

        enterDoor.Open();
    }
    
    public void CloseEntrance()
    {
        //entered new room, close door behind player
        enterDoor.Close();
        Invoke(nameof(TubeClosed), 1.5f);
    }
    private void TubeClosed()
    {
        //disable unseen parts of tube
        tubeExtenstion.SetActive(false);
        exitDoor.gameObject.SetActive(false);
        if(!finalDoor)
        {
            enteredRoom.PlayerEntered();
        }
    }
    #endregion
}