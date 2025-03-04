using UnityEngine;

public class MagnetDeviceScreenUI : MonoBehaviour
{
    [Header("Runtime Variables")]
    [SerializeField] private UIMode activeMode;
    [Space(20)]
    [Header("UI References")]
    [SerializeField] private GameObject noObjectFound;
    [SerializeField] private GameObject objectFound;
    [Space(10)]
    [SerializeField] private UIMode[] modes;

    public void SetObjectFound(bool found)
    {
        ClearAll();
        if (found)
        {
            objectFound.SetActive(true);
        }
        else
        {
            noObjectFound.SetActive(true);
        }
    }

    public void Inputting(bool up)
    {
        /*
        ClearAll();
        if (up)
        {
            activeMode.inputtingUp.SetActive(true);
        }
        else
        {
            activeMode.inputtingDown.SetActive(true);
        }
        */
    }

    public void InputReleased()
    {
        ClearAll();
        activeMode.neutral.SetActive(true);
    }

    public void UpdateMode(Mode newMode)
    {
        for(int i = 0; i<modes.Length; i++)
        {
            if(newMode == modes[i].uiMode)
            {
                activeMode = modes[i];
            }
        }
        InputReleased();
    }

    public void ClearMode()
    {
        SetObjectFound(false);
    }

    private void ClearAll()
    {
        noObjectFound.SetActive(false);
        objectFound.SetActive(false);
        foreach(UIMode mode in modes)
        {
            mode.neutral.SetActive(false);
            //mode.inputtingUp.SetActive(false);
            //mode.inputtingDown.SetActive(false);
        }
    }
}

[System.Serializable]
public class UIMode
{
    public Mode uiMode;
    public GameObject neutral;
    public GameObject inputtingUp;
    public GameObject inputtingDown;
}