using System.Collections;
using UnityEngine;

public class FloatingDevice : MonoBehaviour
{
    [SerializeField] private MagneticObject magneticObject;
    [SerializeField] private Rigidbody rb;

    #region Testing
    [Space(20)]
    [Header("Testing")]
    public bool grab;
    public bool release;
    private void Update()
    {
        if (grab)
        {
            Grabbed();
            grab = false;
        }
        if (release)
        {
            Released();
            release = false;
        }
    }
    #endregion

    #region Configuration
    private void Awake()
    {
        magneticObject.SetDefaultDamping(4);
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    #endregion

    public void Grabbed()
    {
        StopAllCoroutines();
        rb.isKinematic = false;
    }

    public void Released()
    {
        StartCoroutine(nameof(LockingPosition));
    }
    private IEnumerator LockingPosition()
    {
        bool floating = true;
        while(floating)
        {
            Vector3 velocity = rb.linearVelocity + rb.angularVelocity;
            if(velocity.magnitude < 0.01f)
            {
                floating = false;
            }
            yield return null;
        }
        rb.isKinematic = true;
    }
}