using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MagnetDeviceOLD : MonoBehaviour
{
    #region Variables & References
    [Header("Runtime Variables")]
    [SerializeField] private Collider detectedCollider;
    [SerializeField] private MagneticObject magnetizedObject;
    private Rigidbody magnetizedRB;
    [Space(20)]
    [Header("Magnetizing")]
    [SerializeField] private SpringJoint attachJoint;
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
    [SerializeField] private UnityEvent objectGrabbed;
    [SerializeField] private UnityEvent objectReleased;
    [Space(10)]
    [SerializeField] private UnityEvent moveModeSet;
    [SerializeField] private UnityEvent rotateXModeSet;
    [SerializeField] private UnityEvent rotateYModeSet;
    [SerializeField] private UnityEvent rotateZModeSet;
    #endregion

    #region Testing
    [Space(20)]
    [Header("Testing")]
    [SerializeField] private bool testTryGrab;
    private void Update()
    {
        if (testTryGrab)
        {
            TryGrab();
            testTryGrab = false;
        }
    }
    #endregion


    #region Configuration
    private void Awake()
    {
        StartCoroutine(CheckingForObjects());
    }
    #endregion

    #region Obejct Detection
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
            yield return new WaitForSeconds(raycastRate);
            yield return null;
        }
        yield return null;
    }

    public void TryGrab()
    {
        if (detectedCollider != null)
        {
            MagneticObject magneticObject = detectedCollider.GetComponentInParent<MagneticObject>();
            if (magneticObject != null)
            {
                magnetizedObject = magneticObject;
                magnetizedRB = magneticObject.rb;
                StopAllCoroutines();
                GrabObject();
                objectGrabbed.Invoke();
            }
        }
    }
    #endregion

    #region Object Manipulation
    private void GrabObject()
    {
        attachJoint.connectedBody = magnetizedRB;
    }
    #endregion
}