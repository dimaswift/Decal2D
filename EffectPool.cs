using UnityEngine;
using System.Collections;
namespace Decal2D
{
    [CreateAssetMenu(fileName ="effectPool", menuName = "Decal2D/Effects/Effect Pool")]
    public class EffectPool : ScriptableObject
    {
        public int capacity = 10;
        public float rate = .1f;
        public float duration = 3f;

        public ParticleSystem[] particleSystemPrefabs = new ParticleSystem[0];

        EffectContainer[] m_pool;

        [System.NonSerialized]
        int m_poolIndex;
        [System.NonSerialized]
        bool m_inited;
    
        public bool Init()
        {
            if (particleSystemPrefabs.Length == 0 || capacity <= 0)
            {
                m_inited = false;
                return false;
            }

            m_pool = new EffectContainer[capacity];
            int prefabIndex = 0;
            for (int i = 0; i < capacity; i++)
            {
                var p = Instantiate(particleSystemPrefabs[prefabIndex]);
                p.playOnAwake = false;
                p.loop = false;
                m_pool[i] = new EffectContainer(p);
                prefabIndex++;
                if (prefabIndex >= particleSystemPrefabs.Length)
                    prefabIndex = 0;
            }
            m_inited = true;
            return m_inited;
        }

        public void Update()
        {
            if(m_inited)
            {
                for (int i = 0; i < m_pool.Length; i++)
                {
                    m_pool[i].Update();
                }
            }
        }

        public void Place(Vector2 point, Vector3 euler, Transform parent, float duration, float rate)
        {
            var effect = m_pool[m_poolIndex++];

            effect.Spawn(point, euler, parent, duration, rate);

            if (m_poolIndex >= capacity)
                m_poolIndex = 0;
        }

        public class EffectContainer
        {
            Transform transform;
            ParticleSystem system;
            bool isActive = false;
            float durationTimer = 0, rateTimer = 0, duration, rate;

            public EffectContainer (ParticleSystem system)
            {
                this.system = system;
                transform = system.transform;
            }

            public void Spawn(Vector3 point, Vector3 euler, Transform parent, float duration, float rate)
            {
                transform.position = point;
                transform.eulerAngles = euler;
                transform.SetParent(parent);
                durationTimer = 0;
                rateTimer = 0;
                this.duration = duration;
                this.rate = rate;
                isActive = true;
            }

            public void Update()
            {
                if(isActive)
                {
                    if (durationTimer < duration)
                    {
                        rateTimer += Time.deltaTime;
                        durationTimer += Time.deltaTime;
                        if(rateTimer >= rate)
                        {
                            system.Emit(1);
                        }
                    }
                    else isActive = false;
                }
            }
        }
    }
}
