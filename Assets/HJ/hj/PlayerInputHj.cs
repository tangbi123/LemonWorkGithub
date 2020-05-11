using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHj : MonoBehaviour
{   
    //脚本静态变量
    #region 
    //静态变量，Instance  使外界可以访问protected
    public static PlayerInputHj Instance
    {
        get { return s_Instance; }
    }
    //静态变量，s_Instance
    protected static PlayerInputHj s_Instance;
    #endregion
    
    //输入是否受阻
    public bool playerControllerInputBlocked;
    
    //外部输入阻塞
    protected bool m_ExternalInputBlocked;

    //二维向量，存储鼠标移动及wasd输入
    protected Vector2 m_Movement;
    protected Vector2 m_Camera;

    //按键指令，存储按键
    #region 
    protected bool m_jump;
    protected bool m_Pause;
    protected bool m_attack;
    protected bool m_attack_jump;
    protected bool m_attack_fire;
    protected bool m_attack_two;
    protected bool m_run;
    #endregion

    //get set
    #region 
     public Vector2 MoveInput
    {
        get
        {
            if(playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return m_Movement;
        }
    }

    public Vector2 CameraInput
    {
        get
        {
            if(playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return m_Camera;
        }
    }
    public bool RunInput
    {
            get { return m_run && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
    public bool JumpInput
    {
        get { return m_jump && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
    public bool AttackInput
    {
        get { return m_attack && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
    public bool AttackInputFire
    {
        get { return m_attack_fire && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
    public bool AttackInputJump
    {
        get { return m_attack_jump && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
    public bool AttackInputTwo
    {
        get { return m_attack_two && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
    public bool Pause
    {
        get { return m_Pause; }
    }
    #endregion

    #region 
    //等待
    WaitForSeconds m_AttackInputWait;
    //协成
    Coroutine m_AttackWaitCoroutine;
    //常量
    const float k_AttackInputDuration = 0.03f;
    #endregion

    void Awake()
    {
        //0.03f
        m_AttackInputWait = new WaitForSeconds(k_AttackInputDuration);
        //
        if (s_Instance == null)
            s_Instance = this;
        //不能有多个input脚本
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void Update()
    {   
        //默认左键
        if (Input.GetButtonDown("Fire1"))
        {
            //如果  不为空  停止  
            if (m_AttackWaitCoroutine != null)
                StopCoroutine(m_AttackWaitCoroutine);
            //开启协成
            m_AttackWaitCoroutine = StartCoroutine(AttackWait());
        }
        //更新键位
        m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_Camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        m_Pause = Input.GetButtonDown ("Cancel");
        m_jump = Input.GetKey(KeyCode.Space);

        //m_attack = Input.GetKey(KeyCode.Mouse0);
        m_attack_fire = Input.GetKey(KeyCode.Mouse1);
        m_attack_jump = Input.GetKey(KeyCode.Q);
        m_attack_two = Input.GetKey(KeyCode.E);
        m_run = Input.GetKey(KeyCode.LeftShift);
        
    }


    IEnumerator AttackWait()
    {
        m_attack = true;

        yield return m_AttackInputWait;

        m_attack = false;
    }
    //输入阻塞
    //电脑 是否拥有、释放、获得 控制
    //拥有控制
    public bool HaveControl()
    {
        return !m_ExternalInputBlocked;
    }
    //释放控制
    public void ReleaseControl()
    {
        m_ExternalInputBlocked = true;
    }
    //获得控制
    public void GainControl()
    {
        m_ExternalInputBlocked = false;
    }
}