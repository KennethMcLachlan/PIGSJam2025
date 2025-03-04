using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MagneticObject : MonoBehaviour
{
    [SerializeField] private FloatBase floatBase;
    [Space(10)]
    public Rigidbody rb;
    [Space(10)]
    [SerializeField] private UnityEvent grabbed;
    [SerializeField] private UnityEvent released;

    private float defaultDamping = 0.05f;
    //private float defaultFloatDamping = 8f;
    private float magnetizedDamping = 30f;

    private void Awake()
    {
        if(floatBase != null)
        {
            rb.isKinematic = true;
            //rb.linearDamping = defaultFloatDamping;
            //rb.angularDamping = defaultFloatDamping;
        }
    }

    public void SetRotationConstraints(Mode mode)
    {
        if (mode == Mode.Move)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (mode == Mode.RotateClockwise)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        if (mode == Mode.RotateSideways)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        if (mode == Mode.RotateForward)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }

    public void SetMagnetized(bool magnetized)
    {
        if (magnetized)
        {
            StopAllCoroutines();
            rb.isKinematic = false;

            rb.linearDamping = magnetizedDamping;
            rb.angularDamping = magnetizedDamping;

            if(floatBase != null)
            {
                floatBase.SetFloating(false);
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;

            if (floatBase == null)
            {
                rb.linearDamping = defaultDamping;
                rb.angularDamping = defaultDamping;
            }
            else
            {
                //rb.linearDamping = defaultFloatDamping;
                //rb.angularDamping = defaultFloatDamping;

                floatBase.SetFloating(true);
                rb.isKinematic = true;
                //StartCoroutine(LockingFloatPosition());
            }
        }
    }
    private IEnumerator LockingFloatPosition()
    {
        bool floating = true;
        while (floating)
        {
            Vector3 velocity = rb.linearVelocity + rb.angularVelocity;
            if (velocity.magnitude < 0.02f)
            {
                floating = false;
            }
            yield return null;
        }
        floatBase.SetFloating(true);
        rb.isKinematic = true;
        yield return null;
    }

}