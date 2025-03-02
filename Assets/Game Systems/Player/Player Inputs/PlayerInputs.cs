using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    #region Variables & References
    private PlayerControls playerControls;

    private InputAction trigger;
    [SerializeField] private UnityEvent triggerPressed;
    [SerializeField] private UnityEvent triggerReleased;
    [Space(20)]
    private InputAction grip;
    [SerializeField] private UnityEvent gripPressed;
    [SerializeField] private UnityEvent gripReleased;
    [Space(20)]
    private InputAction upButton;
    [SerializeField] private UnityEvent upButtonPressed;
    [SerializeField] private UnityEvent upButtonReleased;
    [Space(20)]
    private InputAction downButton;
    [SerializeField] private UnityEvent downButtonPressed;
    [SerializeField] private UnityEvent downButtonReleased;
    private InputAction lanternButton;
    [SerializeField] private UnityEvent lanternButtonPressed;
    #endregion

    #region Configuration
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        //trigger
        trigger = playerControls.RightHandedPlayerControls.Trigger;
        trigger.Enable();
        trigger.started += TriggerPressed;
        trigger.canceled += TriggerReleased;

        //grip
        grip = playerControls.RightHandedPlayerControls.Grip;
        grip.Enable();
        grip.started += GripPressed;
        grip.canceled += GripReleased;

        //up button
        upButton = playerControls.RightHandedPlayerControls.UpButton;
        upButton.Enable();
        upButton.started += UpButtonPressed;
        upButton.canceled += UpButtonReleased;

        //down button
        downButton = playerControls.RightHandedPlayerControls.DownButton;
        downButton.Enable();
        downButton.started += DownButtonPressed;
        downButton.canceled += DownButtonReleased;

        //lantern button
        lanternButton = playerControls.RightHandedPlayerControls.LanternButton;
        lanternButton.Enable();
        lanternButton.started += LanternButtonPressed;
    }

    private void OnDisable()
    {
        trigger.Disable();
        grip.Disable();
        upButton.Disable();
        downButton.Disable();
        lanternButton.Disable();
    }
    #endregion

    #region Input Events
    private void TriggerPressed(InputAction.CallbackContext context)
    {
        triggerPressed.Invoke();
    }
    private void TriggerReleased(InputAction.CallbackContext context)
    {
        triggerReleased.Invoke();
    }

    private void GripPressed(InputAction.CallbackContext context)
    {
        gripPressed.Invoke();
    }
    private void GripReleased(InputAction.CallbackContext context)
    {
        gripReleased.Invoke();
    }

    private void UpButtonPressed(InputAction.CallbackContext context)
    {
        upButtonPressed.Invoke();
    }
    private void UpButtonReleased(InputAction.CallbackContext context)
    {
        upButtonReleased.Invoke();
    }

    private void DownButtonPressed(InputAction.CallbackContext context)
    {
        downButtonPressed.Invoke();
    }
    private void DownButtonReleased(InputAction.CallbackContext context)
    {
        downButtonReleased.Invoke();
    }

    private void LanternButtonPressed(InputAction.CallbackContext context)
    {
        lanternButtonPressed.Invoke();
    }
    #endregion
}
