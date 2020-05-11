using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance    
        {
            get { return s_Instance; }
        }
        protected static InputController s_Instance;


        [HideInInspector]
        public bool playerControllerInputBlocked;
        protected bool m_ExternalInputBlocked;

        protected Vector2 m_Movement;
        protected Vector2 m_Camera;

        protected bool m_Jump;
        protected bool m_Attack;
        protected bool m_Pause;
        //        protected bool


        #region 返回输入参数


        void Awake()
        {
            //ReleaseControl();
            //0.03f
            m_AttackInputWait = new WaitForSeconds(k_AttackInputDuration);
            //
            if (s_Instance == null)
                s_Instance = this;
            //不能有多个input脚本
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        public Vector2 MoveInput
        {
            get
            {
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;
               // Debug.Log(m_Movement + "\tmovement");
                return m_Movement;
            }
        }

        public Vector2 CameraInput
        {
            get
            {
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;
               // Debug.Log(m_Movement + "\tcameraMove");
                return m_Camera;
            }
        }

        public bool JumpInput
        { get
            {
                return m_Jump && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            } 
        }
        public bool Attack
        {
            get
            {
                return m_Attack && !playerControllerInputBlocked && !m_ExternalInputBlocked;
            }
        }
        public bool Pasue
        {
            get
            {
                return m_Pause;
            }
        }
        #endregion


        WaitForSeconds m_AttackInputWait;
        Coroutine m_AttackWaitCoroutine;

        const float k_AttackInputDuration = 0.03f;

        //连击攻击等待
        IEnumerator AttackWait()
        {
            m_Attack = true;
            yield return m_AttackInputWait;
            m_Attack = false;
        }

        public bool HaveControl()
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
            m_Jump = Input.GetKeyDown(KeyCode.Space);
            //m_Pause = Input.GetButtonDown("Pause");

            //连招 等待输入
            if(Input.GetButtonDown("Fire1"))
            {
                //Debug.Log("attack");
                if (m_AttackWaitCoroutine != null)
                    StopCoroutine(m_AttackWaitCoroutine);

                m_AttackWaitCoroutine = StartCoroutine(AttackWait());
            }
        }

    }
}
