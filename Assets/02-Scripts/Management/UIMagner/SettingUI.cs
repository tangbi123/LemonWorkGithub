using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.audio;
using UnityEngine.SceneManagement;


namespace UI
{
    ///<summary>
    ///设置按钮里面的设置
    ///<summary>
	public class SettingUI : MonoBehaviour
    {
        private Button returnBtn;
        private Slider radio;
        private AudioSource audioSource;
        private Button exitBtn;
        //private Button mainBtn;
        private void Start()
        {
            audioSource = AudioManager.Instance.asBgm;
            radio = GameObject.Find("Radio").GetComponent<Slider>();
            returnBtn = GameObject.Find("returnGame").GetComponent<Button>();
            exitBtn = GameObject.Find("exitBtn").GetComponent<Button>();
            //mainBtn = GameObject.Find("mainBtn").GetComponent<Button>();

            if (returnBtn != null) returnBtn.onClick.AddListener(OnClickReturnBtn);
            if (exitBtn != null) exitBtn.onClick.AddListener(OnClickExitGameBtn);
            //if (mainBtn != null) mainBtn.onClick.AddListener(OnClickMainBtn);

            radio.value = 0.01f;
        }

        void Update()
        {
            RadioChange();
        }
        public void OnClickReturnBtn()
        {
            AudioManager.Instance.Play("button");
            GameObject.Find("UIManager").GetComponent<UIManager>().OnClickReturngameBtn();
        }

        public void RadioChange()
        {
            audioSource.volume = radio.value;
        }

        public void OnClickExitGameBtn()
        {
            AudioManager.Instance.Play("button");
            GameObject.Find("UIManager").GetComponent<UIManager>().OnClickExitGameBtn();
        }

        public void OnClickMainBtn()
        {
            Globe.nextSceneName = "01-Start";
            SceneManager.LoadScene("Loading");
        }
    }
}

