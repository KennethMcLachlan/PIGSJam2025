using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MagnetDevice : MonoBehaviour
{
    #region Variables & References
    [Header("Runtime Variables")]
    [SerializeField] private bool magnetizing;
    [SerializeField] private ManipulationMode activeMode;
    [Space(10)]
    private Collider detectedCollider;
    private bool detected;
    [SerializeField] private MagneticObject magnetizedObject;
    private Rigidbody magnetizedRB;
    [Space(20)]
    [Header("Magnetizing")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Transform rotationOrientation;
    [Space(10)]
    [SerializeField] private float positionalStrength;
    [SerializeField] private float rotationalStrength;
    [Space(10)]
    [SerializeField] private float maxTargetMovementTime;
    [SerializeField] private float maxTargetDistance;
    [SerializeField] private float minTargetDistance;
    [Space(10)]
    [SerializeField] private int activeModeIndex;
    [SerializeField] private ManipulationMode[] manipulationModes;
    [Space(20)]
    [Header("Raycasting")]
    [SerializeField] private float raycastRate;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Transform raycastOrigin;
    [Space(20)]
    [Header("UI")]
    [SerializeField] private MagnetDeviceScreenUI magnetDeviceScreenUI;
    [Space(20)]
    [Header("Object Detection Events")]
    [SerializeField] private UnityEvent objectDetected;
    [SerializeField] private UnityEvent objectLost;
    [Header("Magnetize Events")]
    [SerializeField] private UnityEvent failedMagnetize;
    [SerializeField] private UnityEvent objectMagnetized;
    [SerializeField] private UnityEvent objectReleased;
    [Header("Mode Events")]
    [SerializeField] private UnityEvent modeCycled;
    [SerializeField] private UnityEvent moveModeSet;
    [SerializeField] private UnityEvent rotateXModeSet;
    [SerializeField] private UnityEvent rotateYModeSet;
    [SerializeField] private UnityEvent rotateZModeSet;
    #endregion

    #region Configuration
    private void Awake()
    {
        StartCoroutine(CheckingForObjects());
    }
    #endregion


    #region Object Detection
    private IEnumerator CheckingForObjects()
    {
        bool checking = true;
        detected = false;
        while (checking)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, raycastDistance, raycastMask))
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * hit.distance, Color.green);

                if (detectedCollider != hit.collider && !detected)
                {
                    magnetDeviceScreenUI.SetObjectFound(true);
                    objectDetected.Invoke();
                    detected = true;
                }

                detectedCollider = hit.collider;
            }
            else
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * raycastDistance, Color.red);

                if (detectedCollider != null && detected)
                {
                    magnetDeviceScreenUI.SetObjectFound(false);
                    objectLost.Invoke();
                    detected = false;
                }
                detectedCollider = null;
            }
            yield return null;
        }
        yield return new WaitForSeconds(raycastRate);
        yield return null;
    }
    #endregion

    #region Player Inputs
    public void TriggerToggleMagnetize()
    {
        if (!magnetizing)
        {
            TryMagnetize();
        }
        else
        {
            StopMagnetizing();
        }
    }

    public void GripCycleMode()
    {
        if(magnetizing)
        {
            activeModeIndex += 1;
            if(activeModeIndex == manipulationModes.Length)
            {
                activeModeIndex = 0;
            }
            activeMode = manipulationModes[activeModeIndex];
            magnetizedObject.SetRotationConstraints(activeMode.mode);
            magnetDeviceScreenUI.UpdateMode(activeMode.mode);
            modeCycled.Invoke();
            activeMode.modeSet.Invoke();
        }
    }

    public void ButtonPressed(bool upButton)
    {
        if (magnetizing)
        {
            if(upButton)
            {
                activeMode.inputUp.Invoke();
                if (activeMode.mode == Mode.RotateForward)
                {
                    Rotate(RotationDirection.Forward);
                }
                if (activeMode.mode == Mode.RotateSideways)
                {
                    Rotate(RotationDirection.Left);
                }
                if (activeMode.mode == Mode.RotateClockwise)
                {
                    Rotate(RotationDirection.CounterClockwise);
                }
            }
            else
            {
                activeMode.inputDown.Invoke();
                if (activeMode.mode == Mode.RotateForward)
                {
                    Rotate(RotationDirection.Backward);
                }
                if (activeMode.mode == Mode.RotateSideways)
                {
                    Rotate(RotationDirection.Right);
                }
                if (activeMode.mode == Mode.RotateClockwise)
                {
                    Rotate(RotationDirection.Clockwise);
                }
            }

            magnetDeviceScreenUI.Inputting(upButton);

            if (activeMode.mode == Mode.Move)
            {
                Move(upButton);
            }
        }
    }

    public void ButtonReleased()
    {
        if (magnetizing)
        {
            StopAllCoroutines();
            magnetDeviceScreenUI.InputReleased();
        }
    }
    #endregion

    #region Input Responses
    private void TryMagnetize()
    {
        bool magnetizeFailed = true;
        if (detectedCollider != null)
        {
            MagneticObject magneticObject = detectedCollider.GetComponentInParent<MagneticObject>();
            if (magneticObject != null)
            {
                magnetizedObject = magneticObject;
                magnetizedRB = magnetizedObject.rb;
                detectedCollider = null;

                targetPoint.position = magnetizedObject.transform.position;
                magnetizedObject.SetRotationConstraints(activeMode.mode);
                magnetizedObject.SetMagnetized(true);

                magnetDeviceScreenUI.UpdateMode(activeMode.mode);
                objectMagnetized.Invoke();
                magnetizeFailed = false;

                StopAllCoroutines();
                magnetizing = true;
            }
        }
        if(magnetizeFailed)
        {
            failedMagnetize.Invoke();
        }
    }

    private void StopMagnetizing()
    {
        magnetizedObject.SetMagnetized(false);
        magnetizedObject.rb.constraints = RigidbodyConstraints.None;
        magnetizedObject = null;
        magnetizedRB = null;
        activeModeIndex = 0;

        magnetDeviceScreenUI.ClearMode();
        objectReleased.Invoke();

        StopAllCoroutines();
        StartCoroutine(CheckingForObjects());

        magnetizing = false;
    }

    private void Move(bool forward)
    {
        if (magnetizing)
        {
            StopAllCoroutines();
            StartCoroutine(Moving(forward));
        }
    }

    private void Rotate(RotationDirection direction)
    {
        if (magnetizing)
        {
            Vector3 rotationTorque = Vector3.zero;
            if (direction == RotationDirection.Forward)
            {
                rotationTorque = magnetizedObject.transform.forward * rotationalStrength;
            }
            if (direction == RotationDirection.Backward)
            {
                rotationTorque = magnetizedObject.transform.forward * -rotationalStrength;
            }
            if (direction == RotationDirection.Right)
            {
                rotationTorque = magnetizedObject.transform.up * rotationalStrength;
            }
            if (direction == RotationDirection.Left)
            {
                rotationTorque = magnetizedObject.transform.up * -rotationalStrength;
            }
            if (direction == RotationDirection.Clockwise)
            {
                rotationTorque = magnetizedObject.transform.right * -rotationalStrength;
            }
            if (direction == RotationDirection.CounterClockwise)
            {
                rotationTorque = magnetizedObject.transform.right * rotationalStrength;
            }
            /*
            if (direction == RotationDirection.Forward)
            {
                rotationTorque = magnetizedObject.transform.right * rotationalStrength;
                //rotationTorque = rotationOrientation.right * rotationalStrength;
            }
            if (direction == RotationDirection.Backward)
            {
                rotationTorque = rotationOrientation.right * -rotationalStrength;
            }
            if (direction == RotationDirection.Right)
            {
                rotationTorque = rotationOrientation.up * rotationalStrength;
            }
            if (direction == RotationDirection.Left)
            {
                rotationTorque = rotationOrientation.up * -rotationalStrength;
            }
            if (direction == RotationDirection.Clockwise)
            {
                rotationTorque = rotationOrientation.forward * -rotationalStrength;
            }
            if (direction == RotationDirection.CounterClockwise)
            {
                rotationTorque = rotationOrientation.forward * rotationalStrength;
            }
            */
            StopAllCoroutines();
            StartCoroutine(Rotating(rotationTorque));
        }
    }
    #endregion

    #region Object Manipulation
    private void FixedUpdate()
    {
        if (magnetizing)
        {
            //move magnetized object toward target point
            Vector3 positionalForce = positionalStrength * (targetPoint.position - magnetizedObject.transform.position);
            magnetizedRB.AddForce(positionalForce);
        }
    }

    private IEnumerator Rotating(Vector3 torque)
    {
        //add torque to magnetized object to rotate along an axis
        bool rotating = true;
        while (rotating)
        {
            magnetizedRB.AddTorque(torque);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator Moving(bool movingForward)
    {
        //move target point towards and away from the player
        float targetDistance = maxTargetDistance;
        if(!movingForward)
        {
            targetDistance = minTargetDistance;
        }
        float xPosition = targetPoint.localPosition.x;
        float yPosition = targetPoint.localPosition.y;

        float startingDistance = targetPoint.localPosition.z;
        float moveTime = maxTargetMovementTime * (Mathf.Abs(targetDistance - startingDistance) / (maxTargetDistance - minTargetDistance));

        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            targetPoint.localPosition = new Vector3(xPosition, yPosition, Mathf.Lerp(startingDistance, targetDistance, elapsedTime/moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPoint.localPosition = new Vector3 (xPosition, yPosition, targetDistance);
        yield return null;
    }
    #endregion
}

#region Additional Classes
[System.Serializable]
public class ManipulationMode
{
    public Mode mode;
    public UnityEvent modeSet;
    public UnityEvent inputUp;
    public UnityEvent inputDown;
    public bool freezeXRot;
    public bool freezeYRot;
    public bool freezeZRot;
}

public enum Mode
{
    Move,
    RotateForward,
    RotateSideways,
    RotateClockwise
}

public enum RotationDirection
{
    Forward,
    Backward,
    Right,
    Left,
    Clockwise,
    CounterClockwise
}
#endregion