using System.Threading.Tasks;
using UnityEngine;

public class landingEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private ParticleSystem landingParticles;

    void Start()
    {
        landingParticles = GetComponent<ParticleSystem>();
        landingParticles.Play();
        Destroy(gameObject, 0.5f);
    }

}
