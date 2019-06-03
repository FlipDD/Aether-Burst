using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPickable : MonoBehaviour
{
    public ParticleSystem AffectedParticles = null;
	
    [Range(0.0f, 1.0f)]
    public float ActivationTreshold = 1.0f;

    private Transform m_rTransform = null;
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    private bool m_bWorldPosition = false;
    private float m_fCursorMultiplier = 1.0f;

    void Awake()
    {
        m_rTransform = this.transform;
        Setup();
    }

    private int m_iNumActiveParticles = 0;
    private Vector3 m_vParticlesTarget = Vector3.zero;
    private float m_fCursor = 0.0f;
    void LateUpdate()
    {
        if(AffectedParticles != null)
        {
            m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            m_vParticlesTarget = m_rTransform.position;
            if (!m_bWorldPosition)
                m_vParticlesTarget -= AffectedParticles.transform.position;

            for(int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++) { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {
                    m_fCursor -= ActivationTreshold;
                    m_fCursor *= m_fCursorMultiplier;
					
                    m_rParticlesArray[iParticle].velocity = Vector3.zero;
                    m_rParticlesArray[iParticle].position = Vector3.Lerp(m_rParticlesArray[iParticle].position, m_vParticlesTarget, m_fCursor * m_fCursor);
                }
            }
            AffectedParticles.SetParticles(m_rParticlesArray, m_iNumActiveParticles);
        }
    }

    public void Setup()
    {
        if (AffectedParticles != null)
        {
            // m_rParticlesArray = new ParticleSystem.Particle[AffectedParticles.maxParticles];
            // m_bWorldPosition = AffectedParticles.simulationSpace == ParticleSystemSimulationSpace.World;
            // m_fCursorMultiplier = 1.0f / (1.0f - ActivationTreshold);
        }
    }
    
     private PlayerInputController playerInputController;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] allParticles;

    void Start ()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        ps = GetComponent<ParticleSystem>();
        allParticles = new ParticleSystem.Particle[ps.particleCount];
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector3 dir = (transform.position - playerInputController.transform.position).normalized;
            int numParticlesAlive = ps.GetParticles(allParticles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                allParticles[i].velocity = dir * 6;
            }

            ps.SetParticles(allParticles, numParticlesAlive);

            playerInputController.IncreaseHealth(20);

            gameObject.SetActive(false);

        }
    }
}
