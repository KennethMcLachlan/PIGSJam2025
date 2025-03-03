using UnityEngine;

public class Eyeball : MonoBehaviour
{
    [SerializeField] private Transform eyeball;
    [SerializeField] private Transform playerCamera;

    private void Start()
    {
        playerCamera = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        eyeball.LookAt(playerCamera);
    }
}