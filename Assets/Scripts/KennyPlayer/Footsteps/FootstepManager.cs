using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootstepManager : MonoBehaviour
{
    public static FootstepManager Instance;

    public AudioSource footstepSource;
    public CharacterController characterController;
    public LayerMask groundLayer;

    [Header("Footstep Sounds")]
    public FootstepData metalFootsteps;
    public FootstepData squishFootsteps;
    public FootstepData acidFootsteps;

    public bool _isInAcid = false;
    [SerializeField] private float _stepInterval = 0.5f;
    private float _nextStepTime = 0f;

    public InputActionReference movementAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        movementAction.action.Enable();
    }

    private void OnDisable()
    {
        movementAction.action.Disable();
    }

    private void Update()
    {
        Vector3 moveInput = movementAction.action.ReadValue<Vector2>();
        if (moveInput.magnitude > 0.1f && characterController.isGrounded)
        {
            if (Time.time >= _nextStepTime)
            {
                _nextStepTime = Time.time + _stepInterval;
                PlayFootstep();
            }
        }
    }


    void PlayFootstep()
    {
        FootstepData surfaceData = GetSurfaceFootstepData();
        if (surfaceData != null && surfaceData.footstepSounds.Length > 0)
        {
            AudioClip randomClip = surfaceData.footstepSounds[Random.Range(0, surfaceData.footstepSounds.Length)];
            footstepSource.PlayOneShot(randomClip);
        }
    }

    FootstepData GetSurfaceFootstepData()
    {
        if (_isInAcid) return acidFootsteps;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 2f, groundLayer))
        {
            FootstepSurface surface = hit.collider.GetComponent<FootstepSurface>();
            if (surface != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                return surface.footstepData;
            }
        }

        return null;
    }

    public void SetIsInAcid(bool isInAcid)
    {
        _isInAcid = isInAcid;
    }
}
