using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetRayBehavior : MonoBehaviour
{
    //Input Actions
    public InputActionReference magnetAction; // Trigger Button
    public InputActionReference moveAwayAction; // B Button
    public InputActionReference moveTowardAction; // A Button
    public InputActionReference actionSwap; // Grip Button
    public InputActionReference rightStickAction; //Right Joystick


    //Raycast Variables
    private Ray _ray;
    private RaycastHit _hitInfo;
    public LayerMask _hitLayer; //Layer assigned in the Inspector
    [SerializeField] private float _rayDistance = 5f;

    private bool _isActionPressed;
    private bool _isHoldingObject;
    private bool _isPressingA;
    private bool _isPressingB;

    private MagneticObjectBehavior _currentMagneticObject; //Cache Reference

    //Object Manipulation States
    private enum InteractionMode { Move, RotateHorizontal, RotateVertical }
    private InteractionMode _currentMode = InteractionMode.Move;
    private void Start()
    {
        EnableInputActions();
    }

    private void Update()
    {
        //Initiates the Magnet Ray
        if (_isActionPressed == true && _isHoldingObject == false)
        {
            FireMagnetRay();
        }

        if (_isHoldingObject == true && _currentMagneticObject != null)
        {
            if (_isPressingA == true)
            {
                switch (_currentMode)
                {
                    case InteractionMode.Move:
                        _currentMagneticObject.TowardMovement();
                        Debug.Log("InteractionMode.Move is active");
                        break;
                    case InteractionMode.RotateHorizontal:
                        _currentMagneticObject.RotateRight();
                        break;
                    case InteractionMode.RotateVertical:
                        _currentMagneticObject.RotateDown();
                        break;
                }
            }

            if (_isPressingB == true)
            {
                switch (_currentMode)
                {
                    case InteractionMode.Move:
                        _currentMagneticObject.AwayMovement();
                        break;
                    case InteractionMode.RotateHorizontal:
                        _currentMagneticObject.RotateLeft();
                        break;
                    case InteractionMode.RotateVertical:
                        _currentMagneticObject.RotateUp();
                        break;
                }
            }
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
                _isHoldingObject = true;

                Debug.Log("Magnetic Object has attached to Right COntroller");
            }
        }

        Debug.DrawRay(transform.position, transform.forward * _rayDistance, Color.red,3f);
    }

    //private void OnDestroy()
    //{
    //    magnetAction.action.started -= MagnetActionStarted;
    //    magnetAction.action.canceled -= MagnetActionReleased;
    //    magnetAction.action.Disable();

    //    moveAwayAction.action.started -= AwayActionStarted;
    //    moveAwayAction.action.canceled -= AwayActionCanceled;
    //    moveAwayAction.action.Disable();

    //    moveTowardAction.action.started -= TowardActionStarted;
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
        magnetAction.action.canceled += MagnetActionReleased;


        moveAwayAction.action.Enable();
        moveAwayAction.action.started += AwayActionStarted;
        moveAwayAction.action.canceled += AwayActionCanceled;

        moveTowardAction.action.Enable();
        moveTowardAction.action.started += TowardActionStarted;
        moveTowardAction.action.canceled += TowardActionCanceled;

        actionSwap.action.Enable();
        actionSwap.action.performed += SwapAction;

        rightStickAction.action.Enable();

        if (moveAwayAction.action == null)
        {
            Debug.LogError("InputAction is null");
        }
    }

    //Grip Button
    private void SwapAction(InputAction.CallbackContext context)
    {
        //Change the function between magnet object movement and rotation
        if (_isHoldingObject == false)
        {
            return;
        }

        _currentMode = (InteractionMode)(((int)_currentMode + 1) % 3);
        Debug.Log($"Interaction Mode Changed: {_currentMode}");
    }

    //Trigger Button
    private void MagnetActionStarted(InputAction.CallbackContext context)
    {
        if (_isActionPressed == false)
        {
            _isActionPressed = true;
        }
    }

    private void MagnetActionReleased(InputAction.CallbackContext context)
    {
        if (_isActionPressed == true)
        {
            _isActionPressed = false;
            _isHoldingObject = false;

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
        _isPressingA = true;
    }
    private void TowardActionCanceled(InputAction.CallbackContext context)
    {
        _isPressingA = false;
        _currentMagneticObject?.StopMovementOrRotation();
    }

    //B Button
    private void AwayActionStarted(InputAction.CallbackContext context)
    {
        _isPressingB = true;
    }

    private void AwayActionCanceled(InputAction.CallbackContext context)
    {
        _isPressingB = false;
        _currentMagneticObject?.StopMovementOrRotation();
    }

}
