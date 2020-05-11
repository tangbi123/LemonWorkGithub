using ns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1\Input
/// 2\事件中心模块
/// 3、公共Mono
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart = false;
    /// <summary>
    /// 构造函数，添加 Update（） 函数
    /// </summary>
   public InputMgr()
    {
        MonoManager.GetInstance().AddUpdateListener(MyUpdate);
    }

    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //事件中心模块、 分发按下抬起事件
            EventCenter.GetInstance().EventTrigger("某建按下", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("某键抬起", key);
        }
    }

    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    /// <summary>
    /// 是否开启或关闭输入检测
    /// </summary>
    private void MyUpdate()
    {
        //打开监听
        if (!isStart) return;
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);

    }

}
