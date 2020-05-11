using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using Lemon.audio;

namespace UI
{
    ///<summary>
    ///按钮事件管理
    ///<summary>
	public class BtnsManagement : MonoBehaviour
    {

        
        private Canvas start;
        private Canvas help;

        private Button startBtn;
        private Button helpBtn;
        private Button returnBtn;
        private Button exitBtn;
        void Start()
        {
            start = GetComponent<Canvas>();
            help = GameObject.Find("HelpUI").GetComponent<Canvas>();

            startBtn = GameObject.Find("startBtn").GetComponent<Button>();
            helpBtn = GameObject.Find("helpBtn").GetComponent<Button>();
            returnBtn = GameObject.Find("returnBtn").GetComponent<Button>();

            exitBtn = GameObject.Find("exitBtn").GetComponent<Button>();

            exitBtn.onClick.AddListener(OnClickExitGameBtn);
            startBtn.onClick.AddListener(OnClickStartGameBtn);
            helpBtn.onClick.AddListener(OnClickHelpBtn);
            returnBtn.onClick.AddListener(OnClickHelpBtn);

            

            help.enabled = !help.enabled;
        }
        

        public void OnClickExitGameBtn()
        {
            //GameObject.Find("UIManager").GetComponent<UIManager>().OnClickExitGameBtn();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
        /// <summary>
        /// 按开始游戏按钮，加载 “02-haidaoGame”场景
        /// </summary>
        public void OnClickStartGameBtn()
        {
            AudioManager.Instance.Play("button");
            print("start game");
            Globe.nextSceneName = "03-haidaoGame";
            SceneManager.LoadScene("Loading");
        }

        public void OnClickHelpBtn()
        {
            AudioManager.Instance.Play("button");
            //print("show or close help");
            help.enabled = !help.enabled;
        }

        
    }
}

