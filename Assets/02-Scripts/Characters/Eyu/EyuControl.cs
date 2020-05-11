using GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eyu
{
    public class EyuControl : MonoBehaviour
    {
        protected static EyuControl s_Instance;
        public static EyuControl Instance { get { return s_Instance; } }

        //人物移动属性
        public float maxForwardSpeed = 15f;
        public float gravity = 20f;
        public float jumpSpeed = 10f;
        public float minTurnSpeed = 400f;
        public float maxTurnSpeed = 1200f;
        public float idleTimeout = 5f;// 待机5s  回到idle
        

        protected AnimatorStateInfo m_CurrentStateInfo;
        protected AnimatorStateInfo m_NextStateInfo;
        protected bool m_IsAnimatorTransitioning;
        protected AnimatorStateInfo m_PreviousCurrentStateInfo;
        protected AnimatorStateInfo m_PreviousNextStateInfo;
        protected bool m_PreviousIsAnimatorTransitioning;


        protected bool m_IsGrounded = true;
        protected bool m_PreviouslyGrounded = true;
        protected bool m_ReadyToJump;

        protected float m_DesiredForwardSpeed;
        protected float m_ForwardSpeed;
        protected float m_VerticalSpeed;

        protected float m_IdleTimer;


        private AllInputMgr m_Input;
        private Animator m_Animator;
        private CharacterController m_CharCtrl;
        public CameraSettings cameraSettings;

        protected Quaternion m_TargetRotation;
        protected float m_AngleDiff;

        //Constants are used to behaves properly;
        const float k_AirborneTurnSpeedProportion = 5.4f;
        const float k_GroundedRayDistance = 1f;
        const float k_JumpAbortSpeed = 10f;
        const float k_MinEnemyDotCoeff = 0.2f;
        const float k_InverseOneEighty = 1f / 180f;
        const float k_StickingGravityProportion = 0.3f;
        const float k_GroundAcceleration = 20f;
        const float k_GroundDeceleration = 25f;

        //Animator  parameters   控制位移 转向
        readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");
        readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");
        readonly int m_HashTimeoutToIdle = Animator.StringToHash("TimeoutToIdle");
        readonly int m_HashStateTime = Animator.StringToHash("StateTime");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");

        //Skill  Animator
        protected bool m_InAttack = false;
        protected bool m_InCombo = false;
        public bool canAttack = true;

        //Skill Animator
        readonly int m_HashFire1Attack = Animator.StringToHash("");
        readonly int m_HashYuangong = Animator.StringToHash("");//Fire1
        readonly int m_HashBigAttack = Animator.StringToHash("");//E

        //Animator States
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborne = Animator.StringToHash("Airborne");

        //释放技能时 阻止输入,  tag
        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");

        void Reset()
        {
            m_Input = GameObject.Find("GameManager").GetComponent<AllInputMgr>();
            cameraSettings = GameObject.Find("CameraRig").GetComponent<CameraSettings>();
        }

        void Awake()
        {
            m_Input = GameObject.Find("GameManager").GetComponent<AllInputMgr>();
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();
            cameraSettings = GameObject.Find("CameraRig").GetComponent<CameraSettings>();
            //lemonTX = GetComponent<LemonTX>();
            s_Instance = this;
        }

        protected bool IsMoveInput
        {
            get
            {
                return !Mathf.Approximately(m_Input.MoveInput.sqrMagnitude, 0f);
            }
        }

        public void SetCanAttack(bool canAttack)
        {
            this.canAttack = canAttack;
        }

        void CacheAnimatorState()
        {
            m_PreviousCurrentStateInfo = m_CurrentStateInfo;
            m_PreviousNextStateInfo = m_NextStateInfo;
            m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimatorTransitioning = m_Animator.IsInTransition(0);
        }

        void UpdateInputBlocking()
        {
            bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransitioning;

            inputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;
            m_Input.playerControllerInputBlocked = inputBlocked;
//            print(inputBlocked);
        }

        public void Jump(float f)
        {
            m_VerticalSpeed = f;
            m_IsGrounded = false;
            m_ReadyToJump = false;
        }


        void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.MoveInput;
            if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;//8

            //20 : 25
            float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        void CalculateVerticalMovement()
        {
            if (!m_Input.SpaceInput && m_IsGrounded)
                m_ReadyToJump = true;

            if (m_IsGrounded)
            {
                m_VerticalSpeed = -gravity * k_StickingGravityProportion;//-6

                if (m_Input.SpaceInput && m_ReadyToJump && !m_InCombo)
                {
                    //m_VerticalSpeed = jumpSpeed;//10 
                    //m_IsGrounded = false;
                    //m_ReadyToJump = false;

                    Jump(jumpSpeed);
                }
            }
            else
            {
                if (!m_Input.SpaceInput && m_VerticalSpeed > 0.0f)
                {
                    m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;//10 * time

                }

                if (Mathf.Approximately(m_VerticalSpeed, 0f))
                {
                    m_VerticalSpeed = 0f;
                }
                m_VerticalSpeed -= gravity * Time.deltaTime;

            }
        }


        void SetTargetRotation()
        {
            //移动的方向
            Vector2 moveInput = m_Input.MoveInput;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            //相机的方向
            Vector3 forward = Quaternion.Euler(0f, cameraSettings.Current.m_XAxis.Value, 0f) * Vector3.forward;
            forward.y = 0;
            forward.Normalize();

            Quaternion targetRotation;

            if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1f))
            {
                targetRotation = Quaternion.LookRotation(-forward);
            }
            else
            {
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
                targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
            }


            Vector3 resultingForward = targetRotation * Vector3.forward;
            //计算 angleDiff
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
            m_TargetRotation = targetRotation;
        }

        bool IsOrientationUpdated()
        {
            bool updateOrientationForLocomotion = !m_IsAnimatorTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashLocomotion ||
                m_NextStateInfo.shortNameHash == m_HashLocomotion;

            bool updateOrientationForAirborne = !m_IsAnimatorTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashAirborne ||
                m_NextStateInfo.shortNameHash == m_HashAirborne;

            return updateOrientationForLocomotion || updateOrientationForAirborne;
        }

        void UpdateOrientation()
        {
            m_Animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

            Vector3 localInput = new Vector3(m_Input.MoveInput.x, 0f, m_Input.MoveInput.y);
            float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            float actualTurnSpeed = m_IsGrounded ? groundedTurnSpeed :
                Vector3.Angle(transform.forward, localInput) *
                k_InverseOneEighty * k_AirborneTurnSpeedProportion * groundedTurnSpeed;

            m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);

            transform.rotation = m_TargetRotation;
        }
        void OnAnimatorMove()
        {
            Vector3 movement;

            m_CharCtrl.transform.rotation *= m_Animator.deltaRotation;


            movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
            movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;
            //print("movement" + movement);
            m_CharCtrl.Move(movement);

            m_IsGrounded = m_CharCtrl.isGrounded;

            if (!m_IsGrounded)
                m_Animator.SetFloat(m_HashAirborneVerticalSpeed, m_VerticalSpeed);

            m_Animator.SetBool(m_HashGrounded, m_IsGrounded);
        }

        void TimeoutToIdle()
        {
            bool inputDetected = IsMoveInput /*|| m_Input.AttackInput */|| m_Input.SpaceInput;

            if (m_IsGrounded && !inputDetected)
            {
                m_IdleTimer += Time.deltaTime;

                if (m_IdleTimer >= idleTimeout)
                {
                    m_IdleTimer = 0f;
                    m_Animator.SetTrigger(m_HashTimeoutToIdle);
                }
            }
            else
            {
                m_IdleTimer = 0f;
                m_Animator.ResetTrigger(m_HashTimeoutToIdle);
            }
            // ?  是否检测到 输入
            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }

        void FixedUpdate()
        {
            CacheAnimatorState();

            UpdateInputBlocking();

            m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat((m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime), 1f));

            MyResetTrigger();
            MySetTrigger();

            CalculateForwardMovement();
            CalculateVerticalMovement();

            SetTargetRotation();
            if (IsOrientationUpdated() && IsMoveInput) UpdateOrientation();

            TimeoutToIdle();

            m_PreviouslyGrounded = m_IsGrounded;
        }

        void MyResetTrigger()
        {
           m_Animator.ResetTrigger(m_HashMeleeAttack);

        }

        void MySetTrigger()
        {
            if (m_Input.Fire1AttackInput && canAttack)
            {
                //Debug.Log("attack  really");
                m_Animator.SetTrigger(m_HashMeleeAttack);
            }



            if (m_Input.EInput && canAttack)
            {
                Debug.Log("Eyu : E");
            }
            if (m_Input.FInput && canAttack)
            {
                Debug.Log("Eyu : F");
            }
        }


    }

}
