using UnityEngine;

public class Mouth : MonoBehaviour
{
    public bool fed;
    [SerializeField] private LivingRoom room;

    public void OpenMouth()
    {

    }

    public void MouthFed()
    {
        room.MouthFed();
    }
}