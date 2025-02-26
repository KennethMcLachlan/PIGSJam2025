using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LivingRoom : MonoBehaviour
{
    #region References
    [Header("Variables")]
    [SerializeField] private bool firstRoom;
    [Space(20)]
    [Header("References")]
    [SerializeField] private FoodTube foodTube;
    [SerializeField] private Mouth[] mouths;
    [Space(10)]
    [SerializeField] private DoorTube exitDoorTube;
    #endregion

    #region Configuration
    private void Start()
    {
        if (!firstRoom)
        {
            this.gameObject.SetActive(false);
            if(exitDoorTube != null)
            {
                exitDoorTube.gameObject.SetActive(false);
            }
        }
        else
        {
            Invoke(nameof(PlayerEntered), 3);
        }
    }

    //configure room when the player enters
    public void PlayerEntered()
    {
        StartCoroutine(EnterSequence());
    }
    private IEnumerator EnterSequence()
    {
        yield return new WaitForSeconds(2f);

        for(int i = 0; i< mouths.Length; i++)
        {
            mouths[i].OpenMouth();
            yield return new WaitForSeconds(1f);
        }

        foodTube.EnableTube();

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
        foodTube.DisableTube();
        yield return new WaitForSeconds(3f);
        exitDoorTube.OpenExit();
    }
    #endregion
}