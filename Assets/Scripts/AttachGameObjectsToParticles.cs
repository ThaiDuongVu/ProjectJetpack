using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject prefab;

    private new ParticleSystem particleSystem;
    private List<GameObject> instances = new List<GameObject>();
    private ParticleSystem.Particle[] particles;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void LateUpdate()
    {
        int count = particleSystem.GetParticles(particles);
        while (instances.Count < count) instances.Add(Instantiate(prefab, particleSystem.transform));

        bool worldSpace = (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace) instances[i].transform.position = particles[i].position;
                else instances[i].transform.localPosition = particles[i].position;

                instances[i].SetActive(true);
            }
            else instances[i].SetActive(false);
        }
    }
}
