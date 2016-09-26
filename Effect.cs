using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    public abstract class Effect : ScriptableObject
    {
        static Transform m_audioSourceTransform;
        static AudioSource m_audioSource;

        public static Transform audioSourceTransform { get { return m_audioSourceTransform; } }
        public static AudioSource audioSource { get { return m_audioSource; } }

        [System.NonSerialized]
        protected bool initialized;
        [System.NonSerialized]
        protected bool failedInitilization;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitializeSound()
        {
            m_audioSource = new GameObject("Brush Effect Sound Source").AddComponent<AudioSource>();
            DontDestroyOnLoad(m_audioSource.gameObject);
            m_audioSource.loop = false;
            m_audioSource.playOnAwake = false;
            m_audioSourceTransform = m_audioSource.transform;
        }

        public abstract void Play(Vector3 point);
        public abstract void Emit(Vector3 point, float normalizedIntensity);
        public abstract void Play(Vector3 point, Vector3 euler);
        public abstract void Emit(Vector3 point, Vector3 euler, float normalizedIntensity);
        public abstract void Play(Vector3 point, int stage);
        public abstract void Emit(Vector3 point, float normalizedIntensity, int stage);
        public abstract void Play(Vector3 point, Vector3 euler, int stage);
        public abstract void Emit(Vector3 point, Vector3 euler, float normalizedIntensity, int stage);
    }

}
