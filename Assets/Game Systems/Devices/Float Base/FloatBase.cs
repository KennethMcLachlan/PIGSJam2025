using UnityEngine;

public class FloatBase : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] floatingParticles;
    public void SetFloating(bool floating)
    {
        if (floating)
        {
            foreach(ParticleSystem particle in floatingParticles)
            {
                particle.Play();
            }
        }
        else
        {
            foreach (ParticleSystem particle in floatingParticles)
            {
                particle.Stop();
            }
        }
    }
}