using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class AllInputMgr : MonoBehaviour
    {
        public static AllInputMgr Instance
        {
            get { return s_Instance; }
        }
        protected static AllInputMgr s_Instance;

        [HideInInspector]
        public bool playerControllerInputBlocked;
        protected bool m_ExternalInputBlocked;

        protected Vector2 m_Movement;
        protected Vector2 m_Camera;

        protected bool m_Space;
        protected bool m_Fire1Attack;
        protected bool m_Pause;

        protected bool m_Q;
        protected bool m_E;
        protected bool m_F;

        WaitForSeconds m_AttackInputWait;
        Coroutine m_AttackWaitCoroutine;
        const float k_AttackInputDuration = 0.03f;

        private void Awake()
        {
            m_AttackInputWait = new WaitForSeconds(k_AttackInputDuration);

            if (s_Instance == null) s_Instance = this;

        }

        public Vector2 MoveInput
        {
            get
            {
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;
                return m_Movement;
            }
        }

        public Vector2 CameraInput
        {
            get
            {
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;
                return m_Camera;
            }
        }

        public bool SpaceInput
        {
            get
            {
                return m_Space && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            }
        }
        public bool Fire1AttackInput
        {

            get
            {
                return m_Fire1Attack && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            }
        }

        public bool QInput
        {

            get
            {
                return m_Q && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            }
        }
        public bool EInput
        {

            get
            {
                return m_E && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            }
        }

        public bool FInput
        {

            get
            {
                return m_F && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            }
        }
        public bool Pause
        {
            get
            {
                return m_Pause;
            }
        }

        IEnumerator AttackWait()
        {
            m_Fire1Attack = true;
            yield return m_AttackInputWait;
            m_Fire1Attack = false;
        }

        public bool HavaControl()
        {
            return !m_ExternalInputBlocked;
        }

        public void ReleaseControl()
        {
            m_ExternalInputBlocked = true;
        }

        public void GainControl()
        {
            m_ExternalInputBlocked = false;
        }

        void Update()
        {
            m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            m_Camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            m_Space = Input.GetKeyDown(KeyCode.Space);

            m_Q = Input.GetKeyDown(KeyCode.Q);// 变身  或  解体
            m_E = Input.GetKeyDown(KeyCode.E);// 大招
            m_F = Input.GetKeyDown(KeyCode.F);// 附体

    

            if(Input.GetMouseButtonDown(0))
            {
                if (m_AttackWaitCoroutine != null)
                    StopCoroutine(m_AttackWaitCoroutine);
                m_AttackWaitCoroutine = StartCoroutine(AttackWait());
            }
        }
    }
}
