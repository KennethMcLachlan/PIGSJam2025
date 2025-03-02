using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private int roomIndex;
    [SerializeField] private RoomStemSet[] stemSets;
    private RoomStemSet activeStemSet;

    public void StartRoomMusic()
    {
        activeStemSet = stemSets[roomIndex];
        foreach(Stem persistentStem in activeStemSet.persistentStems)
        {
            persistentStem.SetStemEnabled(true);
        }
        foreach (Stem roomStem in activeStemSet.roomStems)
        {
            roomStem.SetStemEnabled(true);
        }
    }

    public void FadeRoomMusic()
    {
        foreach (Stem roomStem in activeStemSet.roomStems)
        {
            roomStem.SetStemEnabled(false);
        }
        roomIndex += 1;
    }
}

[System.Serializable]
public class RoomStemSet
{
    public Stem[] persistentStems;
    public Stem[] roomStems;
}