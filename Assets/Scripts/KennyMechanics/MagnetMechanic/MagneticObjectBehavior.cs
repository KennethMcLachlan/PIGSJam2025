using UnityEngine;
using UnityEngine.InputSystem;

public class MagneticObjectBehavior : MonoBehaviour
{
    //Object variables
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _objectMoveSpeed = 1f;
    private Rigidbody _rb;
    private Vector3 _grabOffset;
    private Transform _controllerTransform;

    //Magnet Ray Script
    private MagnetRayBehavior _magnetRayBehavior;

    //Object Sway Variables
    private Vector3 _centerOfGravity;
    [SerializeField] private float _swayForce = 0.1f;
    [SerializeField] private float _damping = 0.01f;

    //Object Rotate Variables
    [SerializeField] private float _rotationSpeed = 1f;

    private bool _isSwayActive;
    private bool _isPressingA;
    private bool _isPressingB;
    public bool _isRotating;
    
    private void Start()
    {
        _magnetRayBehavior = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Right Controller")?.GetComponent<MagnetRayBehavior>(); ;
        if (_magnetRayBehavior == null)
        {
            Debug.LogError("Magnet Ray was not found by the Magnetic object prefab");
        }

        _rb = this.gameObject.GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody on Magnetic Object not found");
        }
    }

    private void FixedUpdate()
    {
        //Sway Behavior on Object (Forces push toward center of object)
        if (_isSwayActive && _magnetRayBehavior != null)
        {
            Vector3 directionToCenter = _magnetRayBehavior.transform.TransformPoint(_centerOfGravity) - transform.position;
            Vector3 swayForceDirection = directionToCenter * _swayForce;

            _rb.AddForce(swayForceDirection * _damping, ForceMode.Acceleration);
            _rb.useGravity = false;

            if (_controllerTransform != null)
            {
                Vector3 targetPosition = _controllerTransform.TransformPoint(_grabOffset);
                transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _controllerTransform.rotation, _speed * Time.deltaTime);
            }
        }

        //Initiate moving the object toward and away from the player
        if (_isRotating == false && _magnetRayBehavior != null)
        {
            if (_isPressingA == true)
            {
                TowardMovement();
            }

            if (_isPressingB == true)
            {
                AwayMovement();
            }
        }
    }

    public void AttachToMagnet(Vector3 grabPoint, Transform controller)
    {
        _controllerTransform = controller;
        _grabOffset = controller.InverseTransformPoint(grabPoint);
        _centerOfGravity = transform.InverseTransformPoint(grabPoint);
        InitiateSway();
    }

    public void DetachMagnet()
    {
        _controllerTransform = null;
        DeactivateAButton();
        DeactivateBButton();
        DeactivateSway();

        if (_rb != null)
        {
            _rb.useGravity = true;
        }
    }
    public void InitiateSway()
    {
        _isSwayActive = true;
    }

    public void DeactivateSway()
    {
        _isSwayActive = false;
    }

    //Move Object Away & Toward the Player
    public void AwayMovement()
    {
        Vector3 movementDirection = _magnetRayBehavior.transform.forward;
        transform.position += movementDirection * _objectMoveSpeed * Time.deltaTime;
        //_rb.MovePosition(transform.position + movementDirection * _objectMoveSpeed * Time.deltaTime);
        Debug.Log("Object is moving away from the player");
    }

    public void TowardMovement()
    {
        Vector3 movementDirection = -_magnetRayBehavior.transform.forward;
        transform.position += movementDirection * _objectMoveSpeed * Time.deltaTime;
        //_rb.MovePosition(transform.position + movementDirection * _objectMoveSpeed * Time.deltaTime);
        Debug.Log("The object is moving toward the player");
    }

    //Object Rotation Methods
    public void RotateLeft()
    {
        float rotationAmount = _rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, _rotationSpeed, Space.World);
    }

    public void RotateRight()
    {
        float rotationAmount = _rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, -_rotationSpeed, Space.World);
    }

    public void RotateUp()
    {
        float rotationAmount = _rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right, _rotationSpeed, Space.World);
    }

    public void RotateDown()
    {
        float rotationAmount = _rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right, -_rotationSpeed, Space.World);
    }

    public void StopMovementOrRotation()
    {
        //Stops all movement and rotation
    }

    //Thumbstick style Rotation (Different Style Implementation)
    //public void RotateObject(Vector2 thumbstickInput)
    //{
    //    Vector3 controllerForward = _controllerTransform.forward;
    //    Vector3 controllerUp = _controllerTransform.up;
    //    Vector3 controllerRight = Vector3.Cross(controllerUp, controllerForward);

    //    if (Mathf.Abs(thumbstickInput.y) > Mathf.Abs(thumbstickInput.x))
    //    {
    //        float rotationY = thumbstickInput.y * _rotationSpeed * Time.deltaTime;
    //        transform.Rotate(controllerRight, rotationY, Space.World);
    //    }
    //    else if (Mathf.Abs(thumbstickInput.x) > Mathf.Abs(thumbstickInput.y))
    //    {
    //        float rotationX = -thumbstickInput.x * _rotationSpeed * Time.deltaTime;
    //        transform.Rotate(controllerUp, rotationX, Space.World);
    //    }
    //}

    public void ActivateAButton()
    {
        _isPressingA = true;
    }
    
    public void DeactivateAButton()
    {
        _isPressingA = false;
    }

    public void ActivateBButton()
    {
        _isPressingB = true;
    }

    public void DeactivateBButton()
    {
        _isPressingB = false;
    }

    public void ActivateObjectRotation()
    {
        _isRotating = true;
    }

    public void DeactivateObjectRotation()
    {
        _isRotating = false;
    }
}
