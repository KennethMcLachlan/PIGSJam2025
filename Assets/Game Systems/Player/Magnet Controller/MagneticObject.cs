using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MagneticObject : MonoBehaviour
{
    [SerializeField] private bool floating;
    [Space(10)]
    public Rigidbody rb;
    [Space(10)]
    [SerializeField] private UnityEvent grabbed;
    [SerializeField] private UnityEvent released;

    private float defaultDamping = 0.05f;
    private float defaultFloatDamping = 4f;
    private float magnetizedDamping = 30f;

    private void Awake()
    {
        if(floating)
        {
            rb.isKinematic = true;
            rb.linearDamping = defaultFloatDamping;
            rb.angularDamping = defaultFloatDamping;
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
        }
        else
        {
            if(!floating)
            {
                rb.linearDamping = defaultDamping;
                rb.angularDamping = defaultDamping;
            }
            else
            {
                rb.linearDamping = defaultFloatDamping;
                rb.angularDamping = defaultFloatDamping;
                StartCoroutine(LockingFloatPosition());
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
        rb.isKinematic = true;
        yield return null;
    }

}