using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class MagnetRayBehavior : MonoBehaviour
{
    //Input Actions
    public InputActionReference magnetAction; // Trigger Button
    public InputActionReference moveAwayAction; // B Button
    public InputActionReference moveTowardAction; // A Button
    public InputActionReference actionSwap; // Grip Button
    public InputActionReference rightStickAction; //Right Joystick
    private Vector2 _rightStickInput;


    //Raycast Variables
    private Ray _ray;
    private RaycastHit _hitInfo;
    public LayerMask _hitLayer; //Layer assigned in the Inspector
    [SerializeField] private float _rayDistance = 5f;

    private bool _isActionPressed;
    private bool _isHoldingObject;
    private bool _isSwappingAction;

    private MagneticObjectBehavior _currentMagneticObject; //Cache Reference

    [SerializeField] private ContinuousTurnProvider _continuousTurnProvider;
    [SerializeField] private SnapTurnProvider _snapTurnProvider;
    private void Start()
    {
        EnableInputActions();

        if (_continuousTurnProvider == null)
        {
            _continuousTurnProvider = GameObject.Find("XR Origin (XR Rig)/Locomotion/Turn")?.GetComponent<ContinuousTurnProvider>();
            Debug.Log("ContinuousTurnProvider Was Null and reassigned");
        }

        if (_snapTurnProvider == null)
        {
            _snapTurnProvider = GameObject.Find("XR Origin (XR Rig)/Locomotion/Turn")?.GetComponent<SnapTurnProvider>();
            Debug.Log("SnapTurnProvider Was Null and reassigned");
        }
    }

    private void Update()
    {
        //Initiates the Magnet Ray
        if (_isActionPressed == true && _isHoldingObject == false)
        {
            FireMagnetRay();
            Debug.Log("Trigger Button is being held");
        }

        //Rotates the Magnetized Object ---- Need something to prevent object rotation
        _rightStickInput = rightStickAction.action.ReadValue<Vector2>();
        if (_isHoldingObject == true && _currentMagneticObject != null && _currentMagneticObject._isRotating == true)
        {
            _currentMagneticObject.RotateObject(_rightStickInput);
        }
    }

    private void FireMagnetRay()
    {
        _ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(_ray, out _hitInfo, _rayDistance, _hitLayer))
        {
            Debug.Log("Magnet Ray has hit a Magnetic Object");

            MagneticObjectBehavior magneticObject = _hitInfo.transform.GetComponent<MagneticObjectBehavior>();
            if (magneticObject != null)
            {
                _currentMagneticObject = magneticObject;
                _currentMagneticObject.AttachToMagnet(_hitInfo.point, transform);
                _currentMagneticObject.InitiateSway();

                _isSwappingAction = false;
                _isHoldingObject = true;
                Debug.Log("Magnetic Object has attached to Right COntroller");
            }
        }

        Debug.DrawRay(transform.position, transform.forward * _rayDistance, Color.red,3f);
    }

    //Deactivate Turning
    private void DeactivatePlayerTurning()
    {
        _continuousTurnProvider.turnSpeed = 0;
        _snapTurnProvider.turnAmount = 0;
    }

    private void ReactivatePlayerTurning()
    {
        _continuousTurnProvider.turnSpeed = 180;
        _snapTurnProvider.turnAmount = 45;
    }

    //private void OnDestroy()
    //{
    //    magnetAction.action.started -= MagnetActionStarted;
    //    //magnetAction.action.performed -= MagnetActionHeld;
    //    magnetAction.action.canceled -= MagnetActionReleased;
    //    magnetAction.action.Disable();

    //    moveAwayAction.action.started -= AwayActionStarted;
    //    //moveAwayAction.action.performed -= AwayActionPerformed;
    //    moveAwayAction.action.canceled -= AwayActionCanceled;
    //    moveAwayAction.action.Disable();

    //    moveTowardAction.action.started -= TowardActionStarted;
    //    //moveTowardAction.action.performed -= TowardActionPerformed;
    //    moveTowardAction.action.canceled -= TowardActionCanceled;
    //    moveTowardAction.action.Disable();

    //    actionSwap.action.Disable();
    //    actionSwap.action.performed -= SwapAction;

    //    rightStickAction.action.Disable();
    //    rightStickAction.action.performed -= ThumbStickInput;
    //}

    private void EnableInputActions()
    {
        magnetAction.action.Enable();
        magnetAction.action.started += MagnetActionStarted;
        //magnetAction.action.performed += MagnetActionHeld;
        magnetAction.action.canceled += MagnetActionReleased;


        moveAwayAction.action.Enable();
        moveAwayAction.action.started += AwayActionStarted;
        //moveAwayAction.action.performed += AwayActionPerformed;
        moveAwayAction.action.canceled += AwayActionCanceled;

        moveTowardAction.action.Enable();
        moveTowardAction.action.started += TowardActionStarted;
        //moveTowardAction.action.performed += TowardActionPerformed;
        moveTowardAction.action.canceled += TowardActionCanceled;

        actionSwap.action.Enable();
        actionSwap.action.performed += SwapAction;

        rightStickAction.action.Enable();
        //rightStickAction.action.performed += ThumbStickInput;

        if (moveAwayAction.action == null)
        {
            Debug.LogError("InputAction is null");
        }
    }

    //InputAction to Rotate Object
    //private void ThumbStickInput(InputAction.CallbackContext context)
    //{
    //    _rightStickInput = context.ReadValue<Vector2>();

    //    if (_isHoldingObject == true && _currentMagneticObject != null)
    //    {
    //        _currentMagneticObject.RotateObject(_rightStickInput);
    //    }
    //}

    private void SwapAction(InputAction.CallbackContext context)
    {
        //Change the function between magnet object movement and rotation
        Debug.Log("Grip Button has Been pressed");
        if (_isHoldingObject == true)
        {
            _isSwappingAction = !_isSwappingAction;

            if (_isSwappingAction == true)
            {
                _currentMagneticObject.ActivateObjectRotation();
                DeactivatePlayerTurning();
            }

            if (_isSwappingAction == false)
            {
                _currentMagneticObject.DeactivateObjectRotation();
                ReactivatePlayerTurning();
            }
        }
        
    }

    //Trigger Button
    private void MagnetActionStarted(InputAction.CallbackContext context)
    {
        if (_isActionPressed == false)
        {
            Debug.Log("Grip has been pressed");
            _isActionPressed = true;
        }
    }

    //private void MagnetActionHeld(InputAction.CallbackContext context)
    //{
    //    //Decided to not use this
    //    //May save for later
    //}

    private void MagnetActionReleased(InputAction.CallbackContext context)
    {
        if (_isActionPressed == true)
        {
            Debug.Log("Grip Button was released");
            _isActionPressed = false;
            _isHoldingObject = false;
            ReactivatePlayerTurning();

            if (_currentMagneticObject != null)
            {
                _currentMagneticObject.DetachMagnet();
                _currentMagneticObject.DeactivateObjectRotation();
                _currentMagneticObject = null;
            }
        }
    }

    //A Button
    private void TowardActionStarted(InputAction.CallbackContext context)
    {
        if (_isHoldingObject == true && _currentMagneticObject != null)
        {
            _currentMagneticObject.ActivateAButton();
            Debug.Log("A Button is being pressed");
        }
    }
    private void TowardActionCanceled(InputAction.CallbackContext context)
    {
        if (_currentMagneticObject != null)
        {
            _currentMagneticObject?.DeactivateAButton();
            Debug.Log("A Button has been released");
        }
    }

    //private void TowardActionPerformed(InputAction.CallbackContext context)
    //{
    //    throw new System.NotImplementedException();
    //}


    //B Button
    private void AwayActionStarted(InputAction.CallbackContext context)
    {
        if (_isHoldingObject == true && _currentMagneticObject != null)
        {
            _currentMagneticObject.ActivateBButton();
            Debug.Log("B Button is being pressed");
        }
    }

    //private void AwayActionPerformed(InputAction.CallbackContext context)
    //{
    //    throw new System.NotImplementedException();
    //}

    private void AwayActionCanceled(InputAction.CallbackContext context)
    {
        if (_currentMagneticObject != null)
        {
            _currentMagneticObject!.DeactivateBButton();
            Debug.Log("B Button has been released");
        }
    }

}
