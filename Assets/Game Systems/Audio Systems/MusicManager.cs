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

        foreach (Stem stemToDisable in activeStemSet.stemsToDisable)
        {
            stemToDisable.SetStemEnabled(false);
        }
        foreach (Stem roomStem in activeStemSet.roomStems)
        {
            roomStem.SetStemEnabled(true);
        }
        foreach (Stem persistentStem in activeStemSet.persistingStems)
        {
            persistentStem.SetStemEnabled(true);
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
    public Stem[] stemsToDisable;
    public Stem[] roomStems;
    public Stem[] persistingStems;
}