using UnityEngine;
using System.Collections;
using HandyUtilities;
namespace Decal2D
{
    [CreateAssetMenu(fileName = "effect", menuName = "Decal2D/Effects/Compound Effect")]
    public sealed class CompoundEffect : Effect
    {
        public float maxRate = 50f;
        public bool randomizeEuler = true;
        public ParticleSystem[] particleSystemPrefabs;
        public AudioClip[] clips;

        ParticleContainer[] m_particles;

        class ParticleContainer
        {
            public Vector3 startEuler;
            public Transform transform;
            public ParticleSystem particleSystem;


            public ParticleContainer(ParticleSystem source)
            {
                particleSystem = Instantiate(source);
                transform = particleSystem.transform;
                startEuler = transform.eulerAngles;
            }
        }

        bool Init()
        {
            if (failedInitilization) return false;
            if(!initialized)
            {
                m_particles = new ParticleContainer[particleSystemPrefabs.Length];

                for (int i = 0; i < m_particles.Length; i++)
                {
                    m_particles[i] = new ParticleContainer(particleSystemPrefabs[i]);
                }
               
                if(m_particles.Length == 0 || clips.Length == 0)
                {
                    failedInitilization = true;
                    Debug.Log(string.Format("Compound Effect {0} should have at least one audio clip and one particle effect!", name)); 
                    return false;
                }
                initialized = true;
            }
            return initialized;
        }

        void PlayAtIndex( Vector3 point, Vector3 euler, int index)
        {
            var p = m_particles[index];
            p.transform.position = point;
            p.transform.eulerAngles = euler;
            p.particleSystem.Play();
            index = Random.Range(0, clips.Length);
            var c = clips[index];
            audioSourceTransform.position = point;
            audioSource.PlayOneShot(c);
        }

        void EmitAtIndex(Vector3 point, Vector3 euler, float normalizedIntensity, int index)
        {
            var p = m_particles[index];
            normalizedIntensity = Mathf.Clamp(normalizedIntensity, 0f, 1f);
            p.transform.position = point;
            p.transform.eulerAngles = euler;
            p.particleSystem.Emit((int) (maxRate * normalizedIntensity));
            index = Random.Range(0, clips.Length);
            var c = clips[index];
            audioSourceTransform.position = point;
            audioSource.PlayOneShot(c, normalizedIntensity);
        }

        public override void Play(Vector3 point, Vector3 euler)
        {
            if (Init())
            {
                var index = Random.Range(0, m_particles.Length);
                PlayAtIndex(point, euler, index);
            }
        }

        public override void Emit(Vector3 point, Vector3 euler, float normalizedIntensity)
        {
            if (Init())
            {
                var index = Random.Range(0, m_particles.Length);
                EmitAtIndex(point, euler, normalizedIntensity, index);
            }
        }

        public override void Play(Vector3 point)
        {
            if (Init())
            {
                var index = Random.Range(0, m_particles.Length);
                var euler = randomizeEuler ? new Vector3(0, 0, Random.Range(0, 360)) : m_particles[index].startEuler;
                PlayAtIndex(point, euler, index);
            }
        }

        public override void Emit(Vector3 point, float normalizedIntensity)
        {
            if (Init())
            {
                var index = Random.Range(0, m_particles.Length);
                var euler = randomizeEuler ? new Vector3(0, 0, Random.Range(0, 360)) : m_particles[index].startEuler;
                EmitAtIndex(point, euler, normalizedIntensity, index);
            }
        }

        public override void Play(Vector3 point, Vector3 euler, int stage)
        {
            if (Init())
            {
                var index = Mathf.Clamp(stage, 0, m_particles.Length - 1);
                PlayAtIndex(point, euler, index);
            }
        }

        public override void Emit(Vector3 point, Vector3 euler, float normalizedIntensity, int stage)
        {
            if (Init())
            {
                normalizedIntensity = Mathf.Clamp(normalizedIntensity, 0f, 1f);
                var index = Mathf.Clamp(stage, 0, m_particles.Length - 1);
                EmitAtIndex(point, euler, normalizedIntensity, index);
            }
        }

        public override void Play(Vector3 point, int stage)
        {
            if (Init())
            {
                var index = Mathf.Clamp(stage, 0, m_particles.Length - 1);
                var euler = randomizeEuler ? new Vector3(0, 0, Random.Range(0, 360)) : m_particles[index].startEuler;
                PlayAtIndex(point, euler, index);
            }
        }

        public override void Emit(Vector3 point, float normalizedIntensity, int stage)
        {
            if (Init())
            {
                normalizedIntensity = Mathf.Clamp(normalizedIntensity, 0f, 1f);
                var index = Mathf.Clamp(stage, 0, m_particles.Length - 1);
                var euler = randomizeEuler ? new Vector3(0,0, Random.Range(0, 360)) : m_particles[index].startEuler;
                EmitAtIndex(point, euler, normalizedIntensity, index);
            }
        }
    }
}