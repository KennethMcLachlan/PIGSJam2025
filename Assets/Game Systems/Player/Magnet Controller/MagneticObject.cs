using UnityEngine;
using UnityEngine.Events;

public class MagneticObject : MonoBehaviour
{
    public Rigidbody rb;
    [Space(10)]
    [SerializeField] private UnityEvent grabbed;
    [SerializeField] private UnityEvent released;

    private float defaultDamping = 0.05f;
    private float magnetizedDamping = 30;

    public void SetDefaultDamping(float damping)
    {
        defaultDamping = damping;
    }

    public void SetMagnetized(bool magnetized)
    {
        if (magnetized)
        {
            rb.linearDamping = magnetizedDamping;
            rb.angularDamping = magnetizedDamping;
        }
        else
        {
            rb.linearDamping = defaultDamping;
            rb.angularDamping = defaultDamping;
        }
    }
}