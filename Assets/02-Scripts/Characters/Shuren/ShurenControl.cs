using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using GameManager;
using UnityEngine.Events;

namespace ns
{
    public class ShurenControl : MonoBehaviour
    {
        protected static ShurenControl s_Instance;
        public static ShurenControl Instance { get { return s_Instance; } }


        //人物移动属性
        public float maxForwardSpeed = 8f;
        public float gravity = 20f;
        public float jumpSpeed = 10f;
        public float minTurnSpeed = 400f;
        public float maxTurnSpeed = 1200f;
        public float idleTimeout = 5f;
        public bool canAttack = true;

        

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

        private AllInputMgr m_Input;
        private Animator m_Animator;
        private CharacterController m_CharCtrl;
        public CameraSettings cameraSettings;

        protected Quaternion m_TargetRotation;
        protected float m_AngleDiff;
        protected Collider[] m_OverlapResult = new Collider[8];

        protected bool m_InAttack = false;
        protected bool m_InCombo = false;

        protected float m_IdleTimer;//随机数 选取随机待机状态

        //Constants are used to behaves properly;
        const float k_AirborneTurnSpeedProportion = 5.4f;
        const float k_GroundedRayDistance = 1f;
        const float k_JumpAbortSpeed = 10f;
        const float k_MinEnemyDotCoeff = 0.2f;
        const float k_InverseOneEighty = 1f / 180f;
        const float k_StickingGravityProportion = 0.3f;
        const float k_GroundAcceleration = 20f;
        const float k_GroundDeceleration = 25f;

        //Animator  parameters
        readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");
        readonly int m_HashTimeoutToIdle = Animator.StringToHash("TimeoutToIdle");
        readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");
        readonly int m_HashStateTime = Animator.StringToHash("StateTime");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashBAttack1 = Animator.StringToHash("BAttack1");

        //Animator States
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborne = Animator.StringToHash("Airborne");
        readonly int m_HashShuCombo1 = Animator.StringToHash("ShuCombo1");
        readonly int m_HashShuCombo2 = Animator.StringToHash("ShuCombo2");
        readonly int m_HashShuCombo3 = Animator.StringToHash("ShuCombo3");

        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");

        //是否正在移动
        protected bool IsMoveInput
        { 
            get { return !Mathf.Approximately(m_Input.MoveInput.sqrMagnitude, 0f); }
        }

        //对 canAttack 赋值
        public void SetCanAttack(bool canAttack)
        {
            this.canAttack = canAttack;
        }

        //对 音效等 赋初值，  这里主要是相机
        void Reset()
        {
            m_Input = GameObject.Find("GameManager").GetComponent<AllInputMgr>();
            cameraSettings = GameObject.Find("CameraRig").GetComponent<CameraSettings>();
        }

        //对引用脚本赋值
        void Awake()
        {
            
            m_Input = GameObject.Find("GameManager").GetComponent<AllInputMgr>();
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();
            cameraSettings = GameObject.Find("CameraRig").GetComponent<CameraSettings>();
            s_Instance = this;

            //SceneLinkedSMB<PlayerController>.Initialise(m_Animator, this);
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

        //更新 玩家输入是否 阻塞  
        //1.状态的标签  为 BlockInput 的时候 就不能输入别的
        //
        void UpdateInputBlocking()
        {
            bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransitioning;
            inputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;
            m_Input.playerControllerInputBlocked = inputBlocked;
            //Debug.Log(inputBlocked);
        }

        //计算 前行 移动  以及状态
        void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.MoveInput;
            if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
            //方向 * 速度
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;
            //在移动就加速  否则减速
            float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

            //前行的 速度
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

           // Debug.Log("forwardspeed" + m_ForwardSpeed);
            //控制移动 blendtree

            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed); 
        }

        void CalculateVerticalMovement()
        {
            if (!m_Input.SpaceInput && m_IsGrounded)
                m_ReadyToJump = true;

            if(m_IsGrounded)
            {
                //Debug.Log("isGrounded");
                //落地重力
                m_VerticalSpeed = -gravity * k_StickingGravityProportion;

                if(m_Input.SpaceInput && m_ReadyToJump && !m_InCombo)
                {
                    m_VerticalSpeed = jumpSpeed;
                    m_IsGrounded = false;
                    m_ReadyToJump = false;
                }

            }
            else
            {
                //正在跳
                if(!m_Input.SpaceInput && m_VerticalSpeed > 0.0f)
                {
                    //降低 条约的速度
                    m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;
                }

                if(Mathf.Approximately(m_VerticalSpeed, 0f))
                {
                    m_VerticalSpeed = 0f;
                }

                m_VerticalSpeed -= gravity * Time.deltaTime;
            }
        }

        void SetTargetRotation()
        {
            // player移动
            Vector2 moveInput = m_Input.MoveInput;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            Vector3 forward = Quaternion.Euler(0f, cameraSettings.Current.m_XAxis.Value, 0f) * Vector3.forward;
            forward.y = 0;
            forward.Normalize();

            Quaternion targetRotation;

            //判断 相机forward 和 palyer的forward 方向是否 相反
            if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-forward);
            }
            // 不是的话就让 rotaion 转向 相机的方向
            else
            {
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
                targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            Vector3 resultingForward = targetRotation * Vector3.forward;


            //攻击的时候 试着转向 npc
            //if (m_InAttack)
            //{
            //    // Find all the enemies in the local area.
            //    Vector3 centre = transform.position + transform.forward * 2.0f + transform.up;

            //    Vector3 halfExtents = new Vector3(3.0f, 1.0f, 2.0f);
            //    int layerMask = 1 << LayerMask.NameToLayer("Enemy");
            //    int count = Physics.OverlapBoxNonAlloc(centre, halfExtents, m_OverlapResult, targetRotation, layerMask);

            //    // Go through all the enemies in the local area...
            //    float closestDot = 0.0f;
            //    Vector3 closestForward = Vector3.zero;
            //    int closest = -1;

            //    for (int i = 0; i < count; ++i)
            //    {
            //        // ... and for each get a vector from the player to the enemy.
            //        Vector3 playerToEnemy = m_OverlapResult[i].transform.position - transform.position;
            //        playerToEnemy.y = 0;
            //        playerToEnemy.Normalize();

            //        // Find the dot product between the direction the player wants to go and the direction to the enemy.
            //        // This will be larger the closer to Ellen's desired direction the direction to the enemy is.
            //        float d = Vector3.Dot(resultingForward, playerToEnemy);

            //        // Store the closest enemy.
            //        if (d > k_MinEnemyDotCoeff && d > closestDot)
            //        {
            //            closestForward = playerToEnemy;
            //            closestDot = d;
            //            closest = i;
            //        }
            //    }

            //    // If there is a close enemy...
            //    if (closest != -1)
            //    {
            //        // The desired forward is the direction to the closest enemy.
            //        resultingForward = closestForward;

            //        // We also directly set the rotation, as we want snappy fight and orientation isn't updated in the UpdateOrientation function during an atatck.
            //        transform.rotation = Quaternion.LookRotation(resultingForward);
            //    }
            //}

            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
            m_TargetRotation = targetRotation;
        }

        // Called each physics step to count up to the point where Ellen considers a random idle.
        void TimeoutToIdle()
        {
            bool inputDetected = IsMoveInput || m_Input.Fire1AttackInput || m_Input.SpaceInput;

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

        //控制人物移动
        void OnAnimatorMove()
        {
            Vector3 movement;
            //计算 movement
            if(m_IsGrounded)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f,
                    -Vector3.up);

                if (Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);

                    //Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();
                    //m_CurrnetWalkingSurface = groundRenderer ? groundRenderer.sharedMaterial : null;
                }
                else
                {
                    movement = m_Animator.deltaPosition;
                    //m_CurrentWalkingSurface = null;
                }
            }
            else
            {
                movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
            }


            m_CharCtrl.transform.rotation *= m_Animator.deltaRotation;
            //竖直方向的 vertical speed
            movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;
            //用来移动
            m_CharCtrl.Move(movement);
            

            m_IsGrounded = m_CharCtrl.isGrounded;

            if (!m_IsGrounded) 
                m_Animator.SetFloat(m_HashAirborneVerticalSpeed, m_VerticalSpeed);

            m_Animator.SetBool(m_HashGrounded, m_IsGrounded);
            
        }

        // Called each physics step to help determine whether Ellen can turn under player input.
        bool IsOrientationUpdated()
        {
            bool updateOrientationForLocomotion = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLocomotion || m_NextStateInfo.shortNameHash == m_HashLocomotion;
            bool updateOrientationForAirborne = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashAirborne || m_NextStateInfo.shortNameHash == m_HashAirborne;
            //bool updateOrientationForLanding = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLanding || m_NextStateInfo.shortNameHash == m_HashLanding;

            return updateOrientationForLocomotion || updateOrientationForAirborne /*|| updateOrientationForLanding*/ || m_InCombo && !m_InAttack;
        }

        // Called each physics step after SetTargetRotation if there is move input and Ellen is in the correct animator state according to IsOrientationUpdated.
        //改变转向
        void UpdateOrientation()
        {
            m_Animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

            Vector3 localInput = new Vector3(m_Input.MoveInput.x, 0f, m_Input.MoveInput.y);
            float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            float actualTurnSpeed = m_IsGrounded ? groundedTurnSpeed : Vector3.Angle(transform.forward, localInput) * k_InverseOneEighty * k_AirborneTurnSpeedProportion * groundedTurnSpeed;
            m_TargetRotation = Quaternion.RotateTowards
                (transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);
            //物理程度 改变 转向
            //transform.rotation = m_TargetRotation;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);
            //m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, 360f * Time.deltaTime);

            transform.rotation = m_TargetRotation;
        }


        void MyResetTrigger()
        {
            m_Animator.ResetTrigger(m_HashMeleeAttack);

            m_Animator.ResetTrigger(m_HashBAttack1);
        }

        void MySetTrigger()
        {
            if (m_Input.Fire1AttackInput && canAttack)
            {
                //AtkCheck(50,90);
                //Debug.Log("attack  really");
                m_Animator.SetTrigger(m_HashMeleeAttack);
            }

            if(m_Input.EInput && canAttack)
            {
                //AtkCheck(50, 90);
                m_Animator.SetTrigger(m_HashBAttack1);
            }
        }
        void FixedUpdate()
        {
            CacheAnimatorState();

            UpdateInputBlocking();

            //OnAnimatorMove();
            //UpdateWalk();
            //计算当前动作的 时间
            m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat((m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime), 1f));

            MyResetTrigger();
            MySetTrigger();
            
            

            CalculateForwardMovement();
            CalculateVerticalMovement();


            //转向
            SetTargetRotation();
            if (IsOrientationUpdated() && IsMoveInput)
                UpdateOrientation();

            //PlayAudio();

            TimeoutToIdle();

            m_PreviouslyGrounded = m_IsGrounded;
        }

        public float range = 30;
        public float angle = 80;
        private GameObject hurtTX;

        public void InitHurtTX()
        {
            hurtTX = ObjPool.GetInstance().GetObj("TX/HurtTX");
        }
        public void StartAttack()
        {
            Invoke("AttackCount", 0.2f);
            m_InAttack = true;
        }
        public void EndAttack()
        {
            m_InAttack = false;
        }
        public void AttackCount()
        {
            AtkCheck(range, angle);
        }
        public void AtkCheck(float range, float angle)
        {
            Collider[] colliders = Physics.OverlapSphere
                (transform.position, range, LayerMask.GetMask("NPC"));
            foreach (var coll in colliders)
            {
                float ang = Vector3.Angle
                    (coll.transform.position - transform.position, transform.forward);
                if(ang < angle)
                {
                    //手上函数
                    coll.GetComponent<NPCShuxing>().GetHurt();
                }
            }

        }
    }


    //// Wait for the screen to fade out.
    //yield return StartCoroutine(ScreenFader.FadeSceneOut());
    //        while (ScreenFader.IsFading)
    //        {
    //            yield return null;
    //        }
}
