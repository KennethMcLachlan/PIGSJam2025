using System.Collections;
using UnityEngine;

public class FloatingDevice : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    #region Testing
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