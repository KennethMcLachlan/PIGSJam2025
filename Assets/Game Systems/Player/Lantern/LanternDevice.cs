using UnityEngine;
using UnityEngine.Events;

public class LanternDevice : MonoBehaviour
{
    #region Variables & References
    [Header("Runtime Variables")]
    [SerializeField] private FoodTube activeFoodTube;
    [Header("Object Detection Events")]
    [SerializeField] private UnityEvent connectedToFoodTube;
    [SerializeField] private UnityEvent buttonPressed;
    [SerializeField] private UnityEvent buttonPressFailed;
    [SerializeField] private UnityEvent buttonPressSucceeded;
    #endregion

    #region Controls
    public void SetActiveFoodTube(FoodTube tube)
    {
        activeFoodTube = tube;
        connectedToFoodTube.Invoke();
    }

    public void ButtonPressed()
    {
        buttonPressed.Invoke();
        if (activeFoodTube != null)
        {
            activeFoodTube.LaunchBall();
            buttonPressSucceeded.Invoke();
        } else
        {
            buttonPressFailed.Invoke();
        }
    }
    #endregion
}