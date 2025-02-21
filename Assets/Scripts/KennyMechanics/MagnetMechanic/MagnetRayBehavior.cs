using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetRayBehavior : MonoBehaviour
{
    //Input Actions
    public InputActionReference magnetAction; // Grip Button
    public InputActionReference moveAwayAction; // B Button
    public InputActionReference moveTowardAction; // A Button

    //Raycast Variables
    private Ray _ray;
    private RaycastHit _hitInfo;
    public LayerMask _hitLayer; //Layer assigned in the Inspector
    [SerializeField] private float _rayDistance = 5f;

    private bool _isActionPressed;
    private bool _isHoldingObject;

    private MagneticObjectBehavior _currentMagneticObject; //Cache Reference
    private void Start()
    {
        EnableInputActions();
    }

    private void Update()
    {
        if (_isActionPressed == true && _isHoldingObject == false)
        {
            FireMagnetRay();
            Debug.Log("Grip Button is being held");
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

        if (moveAwayAction.action == null)
        {
            Debug.LogError("InputAction is null");
        }
    }

    //Grip Button
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

            if (_currentMagneticObject != null)
            {
                _currentMagneticObject.DetachMagnet();
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
