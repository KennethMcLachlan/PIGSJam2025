using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleRoom : MonoBehaviour
{
    #region References
    [Header("References")]
    [SerializeField] private GameObject room;
    [SerializeField] private FoodTube foodTube;
    [SerializeField] private Mouth[] mouths;
    [Space(10)]
    [SerializeField] private RoomDoor entrance;
    [SerializeField] private RoomDoor exit;
    [Space(10)]
    [SerializeField] private PuzzleRoom previousRoom;
    [SerializeField] private PuzzleRoom nextRoom;
    #endregion

    #region Configuration
    //configure room when the player enters
    public void PlayerEntered()
    {
        StartCoroutine(EnterSequence());
    }
    private IEnumerator EnterSequence()
    {
        entrance.SetDoorOpen(false);

        yield return new WaitForSeconds(2f);

        for(int i = 0; i< mouths.Length; i++)
        {
            mouths[i].OpenMouth();
            yield return new WaitForSeconds(1f);
        }

        foodTube.SetTubeEnabled(true);
        previousRoom.SetRoomActive(false);

        yield return null;
    }
    #endregion

    #region Room Progression
    //check if all mouths are fed
    public void MouthFed()
    {
        bool allMouthsFed = true;
        for(int i = 0; i<mouths.Length; i++)
        {
            if (!mouths[i].fed)
            {
                allMouthsFed = false;
            }
        }

        if (allMouthsFed)
        {
            RoomComplete();
        }
    }

    //configure room for exit
    public void RoomComplete()
    {
        StartCoroutine (ExitSequence());
    }
    private IEnumerator ExitSequence()
    {
        foodTube.enabled = false;
        nextRoom.SetRoomActive(true);
        yield return new WaitForSeconds(3f);
        exit.SetDoorOpen(true);
    }
    #endregion

    #region Room Management
    public void SetRoomActive(bool active)
    {
        if(active)
        {
            room.SetActive(true);
            exit.gameObject.SetActive(true);
        }
        else
        {
            room.SetActive(false);
            entrance.gameObject.SetActive(false);
        }
    }
    #endregion
}