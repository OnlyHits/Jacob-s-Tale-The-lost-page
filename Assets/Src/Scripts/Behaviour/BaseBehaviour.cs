using UnityEngine;

namespace CustomArchitecture
{
    public class BaseBehaviour : MonoBehaviour
    {
        [Header("BaseBehaviour")]
        [ReadOnly, SerializeField] private bool m_pause;

        protected virtual void OnUpdate(float elapsed_time) { }
        protected virtual void OnFixedUpdate(float elapsed_time) { }
        protected virtual void OnLateUpdate(float elapsed_time) { }

        public virtual void Pause(bool pause = true)
        {
            m_pause = pause;
        }

        protected void Update()
        {
            if (m_pause)
                return;

            OnUpdate(Time.deltaTime);
        }

        protected void FixedUpdate()
        {
            if (m_pause)
                return;

            OnFixedUpdate(Time.fixedDeltaTime);
        }

        protected void LateUpdate()
        {
            if (m_pause)
                return;

            OnLateUpdate(Time.deltaTime);
        }
    }
}