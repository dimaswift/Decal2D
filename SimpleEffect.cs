using UnityEngine;
using System.Collections;

namespace Decal2D
{
    [CreateAssetMenu(fileName = "effect", menuName = "Decal2D/Effects/Simple Effect")]
    public sealed class SimpleEffect : Effect
    {
        public float maxRate = 50f;
        public bool randomizeEuler = true;
        public ParticleSystem particleSystemPrefab;
        public AudioClip clip;

        [SerializeField]
        [HideInInspector]
        ParticleSystem m_particleSystem;
        [SerializeField]
        [HideInInspector]
        Transform m_transform;

        Vector3 m_startEuler;
        bool m_hasClip = false;
        bool m_hasParticle = false;

        public bool Init()
        {
            if (!initialized)
            {
                m_hasClip = clip != null;
                m_hasParticle = particleSystemPrefab != null;
                if (m_hasParticle)
                {
                    m_particleSystem = Instantiate(particleSystemPrefab);
                    m_transform = m_particleSystem.transform;
                    m_startEuler = m_transform.eulerAngles;
                    initialized = true;
                }
                else failedInitilization = true;
            }
            return initialized;
        }

        Vector3 GetEuler()
        {
            return randomizeEuler ? new Vector3(0, 0, Random.Range(0, 360)) : m_startEuler;
        }

        public override void Emit(Vector3 point, Vector3 euler, float normalizedIntensity)
        {
            if(Init())
            {
                normalizedIntensity = Mathf.Clamp(normalizedIntensity, 0f, 1f);
                if (m_hasParticle)
                {
                    m_transform.position = point;
                    m_transform.eulerAngles = euler;
                    m_particleSystem.Emit((int) (maxRate * normalizedIntensity));
                }
                if (m_hasClip)
                {
                    audioSourceTransform.position = point;
                    audioSource.PlayOneShot(clip, normalizedIntensity);
                }
            }
        }

        public override void Play(Vector3 point, Vector3 euler)
        {
            if(Init())
            {
                if (m_hasParticle)
                {
                    m_transform.position = point;
                    m_transform.eulerAngles = euler;
                    m_particleSystem.Play();
                }
                if (m_hasClip)
                {
                    audioSourceTransform.position = point;
                    audioSource.PlayOneShot(clip, .5f);
                }
            }
        }

        public override void Emit(Vector3 point, float normalizedIntensity)
        {
            Emit(point, GetEuler(), normalizedIntensity);
        }

        public override void Play(Vector3 point)
        {
            Play(point, GetEuler());
        }

        public override void Play(Vector3 point, int stage)
        {
            Play(point, GetEuler());
        }

        public override void Emit(Vector3 point, Vector3 euler, float normalizedIntensity, int stage)
        {
            Emit(point, euler, normalizedIntensity);
        }

        public override void Emit(Vector3 point, float normalizedIntensity, int stage)
        {
            Emit(point, normalizedIntensity);
        }

        public override void Play(Vector3 point, Vector3 euler, int stage)
        {
            Play(point, euler);
        }

    }
}
