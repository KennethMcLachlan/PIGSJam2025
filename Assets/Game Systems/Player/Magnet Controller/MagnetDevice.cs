using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MagnetDevice : MonoBehaviour
{
#if UNITY_EDITOR
    #region Testing
    [Header("Testing")]
    [SerializeField] private bool testMagnetize;
    [SerializeField] private bool testStopMagnetizing;
    [Space(10)]
    [SerializeField] private bool testMoveForward;
    [SerializeField] private bool testMoveBackward;
    [Space(10)]
    [SerializeField] private bool testRotateForward;
    [SerializeField] private bool testRotateBackward;
    [SerializeField] private bool testRotateRight;
    [SerializeField] private bool testRotateLeft;
    [SerializeField] private bool testRotateClockwise;
    [SerializeField] private bool testRotateCounterClockwise;
    [Space(10)]
    [SerializeField] private bool testStopInputting;

    private void Update()
    {
        #region Test Magnetizing
        if (testMagnetize)
        {
            TryMagnetize();
            testMagnetize = false;
        }
        if (testStopMagnetizing)
        {
            TryStopMagnetizing();
            testStopMagnetizing = false;
        }
        #endregion

        #region Test Movement
        if (testMoveForward)
        {
            TryMove(true);
            testMoveForward = false;
        }
        if (testMoveBackward)
        {
            TryMove(false);
            testMoveBackward = false;
        }
        #endregion

        #region Test Rotation
        if (testRotateForward)
        {
            TryRotate(RotationDirection.Forward);
            testRotateForward = false;
        }
        if (testRotateBackward)
        {
            TryRotate(RotationDirection.Backward);
            testRotateBackward = false;
        }
        if (testRotateRight)
        {
            TryRotate(RotationDirection.Right);
            testRotateRight = false;
        }
        if (testRotateLeft)
        {
            TryRotate(RotationDirection.Left);
            testRotateLeft = false;
        }
        if (testRotateClockwise)
        {
            TryRotate(RotationDirection.Clockwise);
            testRotateClockwise = false;
        }
        if (testRotateCounterClockwise)
        {
            TryRotate(RotationDirection.CounterClockwise);
            testRotateCounterClockwise = false;
        }
        #endregion

        if (testStopInputting)
        {
            TryStopInput();
            testStopInputting = false;
        }
    }
    [Space(30)]
    #endregion
#endif

    #region Variables & References
    [Header("Runtime Variables")]
    [SerializeField] private bool magnetizing;
    [SerializeField] private Collider detectedCollider;
    [SerializeField] private MagneticObject magnetizedObject;
    private Rigidbody magnetizedRB;
    [Space(20)]
    [Header("Magnetizing")]
    [SerializeField] private Transform targetPoint;
    [Space(10)]
    [SerializeField] private float positionalStrength;
    [SerializeField] private float rotationalStrength;
    [Space(10)]
    [SerializeField] private float maxTargetMovementTime;
    [SerializeField] private float maxTargetDistance;
    [SerializeField] private float minTargetDistance;
    [Space(20)]
    [Header("Raycasting")]
    [SerializeField] private float raycastRate;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Transform raycastOrigin;
    [Space(20)]
    [Header("Events")]
    [SerializeField] private UnityEvent objectDetected;
    [SerializeField] private UnityEvent objectLost;
    [Space(10)]
    [SerializeField] private UnityEvent objectMagnetized;
    [SerializeField] private UnityEvent objectReleased;
    [Space(10)]
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
        while (checking)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, raycastDistance, raycastMask))
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * hit.distance, Color.green);

                if (detectedCollider != hit.collider)
                {
                    objectDetected.Invoke();
                }

                detectedCollider = hit.collider;
            }
            else
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * raycastDistance, Color.red);

                if (detectedCollider != null)
                {
                    objectLost.Invoke();
                }
                detectedCollider = null;
            }
            yield return null;
        }
        yield return new WaitForSeconds(raycastRate);
        yield return null;
    }

    public void TryMagnetize()
    {
        if (detectedCollider != null && !magnetizing)
        {
            MagneticObject magneticObject = detectedCollider.GetComponentInParent<MagneticObject>();
            if (magneticObject != null)
            {
                magnetizedObject = magneticObject;
                magnetizedRB = magnetizedObject.rb;
                detectedCollider = null;

                SetStartingTargetPosition(magnetizedObject.transform.position);
                magnetizedObject.SetMagnetized(true);

                objectMagnetized.Invoke();

                StopAllCoroutines();
                magnetizing = true;
            }
        }
    }

    public void TryStopMagnetizing()
    {
        if(magnetizing)
        {
            magnetizedObject.SetMagnetized(false);
            magnetizedObject = null;
            magnetizedRB = null;

            objectReleased.Invoke();

            StopAllCoroutines();
            StartCoroutine(CheckingForObjects());
        }
    }

    private void SetStartingTargetPosition(Vector3 grabbedPosition)
    {
        float distanceToObject = Vector3.Distance(raycastOrigin.position, grabbedPosition);
        float startingTargetDistance = 0;
        if(distanceToObject > maxTargetDistance)
        {
            startingTargetDistance = maxTargetDistance;
        }
        else
        {
            if (distanceToObject < minTargetDistance)
            {
                startingTargetDistance = minTargetDistance;
            }
            else
            {
                startingTargetDistance = Mathf.Lerp(minTargetDistance, maxTargetDistance, distanceToObject / (maxTargetDistance - minTargetDistance));
            }
        }
        targetPoint.localPosition = new Vector3(0, 0, startingTargetDistance);
    }
    #endregion

    #region Controls
    public void TryMove(bool forward)
    {
        if (magnetizing)
        {
            StopAllCoroutines();
            StartCoroutine(Moving(forward));
        }
    }

    public void TryRotate(RotationDirection direction)
    {
        if (magnetizing)
        {
            Vector3 rotationTorque = Vector3.zero;
            if (direction == RotationDirection.Forward)
            {
                rotationTorque = targetPoint.right * rotationalStrength;
            }
            if (direction == RotationDirection.Backward)
            {
                rotationTorque = targetPoint.right * -rotationalStrength;
            }
            if (direction == RotationDirection.Right)
            {
                rotationTorque = targetPoint.up * rotationalStrength;
            }
            if (direction == RotationDirection.Left)
            {
                rotationTorque = targetPoint.up * -rotationalStrength;
            }
            if (direction == RotationDirection.Clockwise)
            {
                rotationTorque = targetPoint.forward * -rotationalStrength;
            }
            if (direction == RotationDirection.CounterClockwise)
            {
                rotationTorque = targetPoint.forward * rotationalStrength;
            }
            StopAllCoroutines();
            StartCoroutine(Rotating(rotationTorque));
        }
    }

    public void TryStopInput()
    {
        if(magnetizing)
        {
            StopAllCoroutines();
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

        float startingDistance = targetPoint.localPosition.z;
        float moveTime = maxTargetMovementTime * (Mathf.Abs(targetDistance - startingDistance) / (maxTargetDistance - minTargetDistance));

        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            targetPoint.localPosition = new Vector3(0, 0, Mathf.Lerp(startingDistance, targetDistance, elapsedTime/moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPoint.localPosition = new Vector3 (0, 0, targetDistance);
        yield return null;
    }
    #endregion
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