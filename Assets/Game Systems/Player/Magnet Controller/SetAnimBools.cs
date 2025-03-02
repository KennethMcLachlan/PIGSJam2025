using UnityEngine;

public class SetAnimBools : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string[] animBools;

    public void SetBoolTrue(int boolIndex)
    {
        anim.SetBool(animBools[boolIndex], true);
    }

    public void SetBoolFalse(int boolIndex)
    {
        anim.SetBool(animBools[boolIndex], false);
    }
}