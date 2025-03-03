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
    private MusicManager musicManager;
    #endregion

    #region Configuration
    private void Start()
    {
        musicManager = FindFirstObjectByType<MusicManager>();
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
            Invoke(nameof(PlayerEntered), 1.5f);
        }
    }

    //configure room when the player enters
    public void PlayerEntered()
    {
        StartCoroutine(EnterSequence());
    }
    private IEnumerator EnterSequence()
    {
        musicManager.StartRoomMusic();

        yield return new WaitForSeconds(0.5f);

        foodTube.EnableTube();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i< mouths.Length; i++)
        {
            mouths[i].OpenMouth();
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
    #endregion

    #region Room Progression
    public void MouthFed()
    {
        //check if all mouths are fed
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
        StartCoroutine(ExitSequence());
    }
    private IEnumerator ExitSequence()
    {
        musicManager.FadeRoomMusic();
        foodTube.DisableTube();
        yield return new WaitForSeconds(3f);
        exitDoorTube.OpenExit();
    }
    #endregion
}