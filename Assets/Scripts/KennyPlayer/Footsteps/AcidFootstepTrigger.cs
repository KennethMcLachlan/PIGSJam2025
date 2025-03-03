using UnityEngine;

public class AcidFootstepTrigger : MonoBehaviour
{
    public FootstepData footstepData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FootstepManager.Instance.SetIsInAcid(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FootstepManager.Instance.SetIsInAcid(false);
        }
    }

}
