using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lemon.Character;
using UI;
using GameManager;

/// <summary>
/// 各种按钮的函数
/// 各种界面的管理
/// 
/// </summary>
public class UIManager : MonoBehaviour
{
    
    
    private Canvas GameUI;
    private Canvas SettingUI;
    private Canvas ShuxingUI;
    private Canvas WinUI;
    private Canvas LostUI;

    //private Canvas HelpUI;

    private BaseShuxing cData;
    private bool isWin = false;
    private bool isLost = false;
    /// <summary>
    /// 准备各种工具
    /// </summary>
    void Start()
    {
        if (!(GameUI = GameObject.Find("GameUI").GetComponent<Canvas>()))
            print("can not find GameUI");
        if (!(SettingUI = GameObject.Find("SettingUI").GetComponent<Canvas>()))
            print("can not find SettingUI");
        if (!(ShuxingUI = GameObject.Find("ShuxingUI").GetComponent<Canvas>()))
            print("can not find ShuxingUI");

        if (!(WinUI = GameObject.Find("WinUI").GetComponent<Canvas>()))
            print("can not find WinUI");
        if (!(LostUI = GameObject.Find("LostUI").GetComponent<Canvas>()))
            print("can not find LostUI");

        //HelpUI = GameObject.Find("HelpUI").GetComponent<Canvas>();
        cData = GameObject.Find("GameManager").GetComponent<GameMgr>().shuxing;
        isWin = false;
        isLost = false;

        if (GameUI && SettingUI && ShuxingUI && WinUI && LostUI)
        {
            GameUI.enabled = true;
            SettingUI.enabled = false;
            ShuxingUI.enabled = false;

            //HelpUI.enabled = false;

            WinUI.enabled = false;
            LostUI.enabled = false;

            //Debug.Log("UI succeess"  + HelpUI.enabled);
        }
        else Debug.Log("UI error");
        //StartCoroutine(WinOrLost());
    }
    

    IEnumerator WinOrLost()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if (IsEnerge("NPC") && IsEnerge("energe"))
            {
                IsWin();
                StopAllCoroutines();
            }

            if(cData.HP <= 0)
            {
                IsLost();
                StopAllCoroutines();
            }

            
        }
    }

    public void OnClickExitGameBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    /// <summary>
    /// 按设置按钮，会暂停游戏
    /// </summary>
    public void OnClickSettingBtn()
    {
        Time.timeScale = 0;
        GameUI.enabled = !GameUI.enabled;
        SettingUI.enabled = !SettingUI.enabled;
    }



    /// <summary>
    ///按返回游戏按钮，会继续游戏
    /// </summary>
    public void OnClickReturngameBtn()
    {
        Time.timeScale = 1;
        GameUI.enabled = !GameUI.enabled;
        SettingUI.enabled = !SettingUI.enabled;
    }

    /// <summary>
    /// 人物属性界面
    /// </summary>
    public void OnClickShuxingBtn()
    {
        Time.timeScale = 0;
        GameUI.enabled = !GameUI.enabled;
        ShuxingUI.enabled = !ShuxingUI.enabled;
        GameObject.Find("ShuxingUI").GetComponent<ShuxingUI>().UpdateShuxing();
    }

    public void OnClickShuxingReturnBtn()
    {
        Time.timeScale = 1;
        GameUI.enabled = !GameUI.enabled;
        ShuxingUI.enabled = !ShuxingUI.enabled;
    }

   

    public void IsWin()
    {
        GameUI.enabled = !GameUI.enabled;
        WinUI.enabled = !WinUI.enabled;
 
    }
    public void IsLost()
    {
        GameUI.enabled = !GameUI.enabled;
        LostUI.enabled = !LostUI.enabled;
    }

    /// <summary>
    /// 按名字查找  物体
    /// 要是没有 则返回true， 否则返回false
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsEnerge(string name)
    {
        GameObject[] energes = GameObject.FindGameObjectsWithTag(name);
        if (energes.Length == 0) return true;
        else return false;
    }

    public void OnClickNextgameBtn()
    {
        Globe.nextSceneName = "02-haidiGame";
        SceneManager.LoadScene("Loading");
    }
}


