using UnityEngine;

public class PCFramerateTarget : MonoBehaviour
{
    [SerializeField] private int targetFramerate = 120;

    private void Awake()
    {
        if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            Application.targetFrameRate = targetFramerate;
            Debug.Log("Running on a Windows machine");
        }
    }
}