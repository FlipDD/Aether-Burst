using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    // The particle system to operate on
    public ParticleSystem AffectedParticles = null;
	
    // Normalized treshold on the particle lifetime
    // 0: affect particles right after they are born
    // 1: never affect particles
    [Range(0.0f, 1.0f)]
    public float ActivationTreshold = 1.0f;

    // Transform cache
    private Transform m_rTransform = null;
    // Array to store particles info
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    // Is this particle system simulating in world space?
    private bool m_bWorldPosition = false;
    // Multiplier to normalize movement cursor after treshold is crossed
    private float m_fCursorMultiplier = 1.0f;

    void Awake()
    {
        // Let's cache the transform
        m_rTransform = this.transform;
        // Setup particle system info
        Setup();
    }

    // To store how many particles are active on each frame
    private int m_iNumActiveParticles = 0;
    // The attractor target
    private Vector3 m_vParticlesTarget = Vector3.zero;
    // A cursor for the movement interpolation
    private float m_fCursor = 0.0f;
    void LateUpdate()
    {
	    // Work only if we have something to work on :)
        if(AffectedParticles != null)
        {
            // Let's fetch active particles info
            m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            // The attractor's target is it's world space position
            m_vParticlesTarget = m_rTransform.position;
            // If the system is not simulating in world space, let's project the attractor's target in the system's local space
            if (!m_bWorldPosition)
                m_vParticlesTarget -= AffectedParticles.transform.position;

            // For each active particle...
            for(int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++) { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {
                    // Let's project the overall cursor in the "over treshold" normalized space
                    m_fCursor -= ActivationTreshold;
                    m_fCursor *= m_fCursorMultiplier;
					
                    // Take over the particle system imposed velocity
                    m_rParticlesArray[iParticle].velocity = Vector3.zero;
                    // Interpolate the movement towards the target with a nice quadratic easing					
                    m_rParticlesArray[iParticle].position = Vector3.Lerp(m_rParticlesArray[iParticle].position, m_vParticlesTarget, m_fCursor * m_fCursor);
                }
            }

            // Let's update the active particles
            AffectedParticles.SetParticles(m_rParticlesArray, m_iNumActiveParticles);
        }
    }

    public void Setup()
    {
        // If we have a system to setup...
        if (AffectedParticles != null)
        {
            // // Prepare enough space to store particles info
            // m_rParticlesArray = new ParticleSystem.Particle[AffectedParticles.maxParticles];
            // // Is the particle system working in world space? Let's store this info
            // m_bWorldPosition = AffectedParticles.simulationSpace == ParticleSystemSimulationSpace.World;
            // // This the ratio of the total lifetime cursor to the "over treshold" section
            // m_fCursorMultiplier = 1.0f / (1.0f - ActivationTreshold);
        }
    }
}
