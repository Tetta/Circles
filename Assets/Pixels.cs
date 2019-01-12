using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixels : MonoBehaviour {
    public GameObject prefab;
    //List<ParticleCollisionEvent> collisionEvents;
    // Use this for initialization
    void Start () {
        /*
		for (int i = 0; i < 2000; i ++) {
            GameObject g = Instantiate(prefab);
            Random.InitState(i);
            g.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 3f), 0);
        }
        */
        //collisionEvents = new List<ParticleCollisionEvent>();
    }

    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    public float m_Drift = 0.01f;
    Dictionary<ParticleSystem.Particle, Vector3> particles = new Dictionary<ParticleSystem.Particle, Vector3>();
    int indexOf;
    private void LateUpdate() {
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = m_System.GetParticles(m_Particles);
        //Debug.Log(numParticlesAlive);
        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++) {
            for (int j = 0; j < i; j++) {
                if (Mathf.Abs(m_Particles[i].position.x - m_Particles[j].position.x) < 0.01f &&
                     Mathf.Abs(m_Particles[i].position.y - m_Particles[j].position.y) < 0.01f) {
                    //m_Particles[i].remainingLifetime = 0;
                    /*
                    try {
                        //indexOf = particles.IndexOf(m_Particles[i]);
                        if (particles[m_Particles[i]] != null) {
                            m_Particles[i].position = particles[m_Particles[i]];
                            Debug.Log("find ");
                        }
                        else
                        //m_Particles[i].position = m_Particles[i].position + new Vector3(0.1f, 0.2f);
                        {
                            m_Particles[i].position = m_Particles[i].position + new Vector3(0.1f, 0.2f);
                            particles[m_Particles[i]] = m_Particles[i].position;
                        }
                    } catch (System.Exception e) {
                        m_Particles[i].position = m_Particles[i].position + new Vector3(0.1f, 0.2f);
                        particles[m_Particles[i]] = m_Particles[i].position;
                    }
                    //m_Particles[i].tot = new Vector3(0, 0, 0);
                    //m_Particles[i].
                    */
                }
                //m_Particles[i].
            }
        }

        // Apply the particle changes to the Particle System
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    void InitializeIfNeeded() {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }
}
