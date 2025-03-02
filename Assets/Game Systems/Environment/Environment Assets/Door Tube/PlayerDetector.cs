using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent playerDetected;
    [SerializeField] private MeshRenderer[] debugMeshes;

    private void Awake()
    {
        foreach (MeshRenderer mesh in debugMeshes)
        {
            mesh.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerDetected.Invoke();
        gameObject.SetActive(false);
    }
}