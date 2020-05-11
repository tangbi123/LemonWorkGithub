using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Character;
using Lemon.audio;
using GameManager;

namespace UI
{
    //找ui脚本
    ///<summary>
    ///
    ///<summary>
	public class ConllectMana : MonoBehaviour
    {
        private Text blueText;
        private Image blueFill;
        private Button SettingBtn;
        private Text bloodText;
        private Image bloodFill;
        private Button ShuxingBtn;

        private Button helpBtn;
        private Canvas helpUI;
        private Button returnBtn;

        private BaseShuxing cData;


        private void Start()
        {
            blueFill = GameObject.Find("blueFill").GetComponent<Image>();
            blueText = GameObject.Find("blueText").GetComponent<Text>();
        
            bloodText = GameObject.Find("bloodText").GetComponent<Text>();
            bloodFill = GameObject.Find("bloodFill").GetComponent<Image>();

            SettingBtn = GameObject.Find("SettingBtn").GetComponent<Button>();
            ShuxingBtn = GameObject.Find("ShuxingBtn").GetComponent<Button>();
            cData = GameObject.Find("GameManager").GetComponent<GameMgr>().shuxing;



            returnBtn = GameObject.Find("returnBtn").GetComponent<Button>();
            helpBtn = GameObject.Find("helpBtn").GetComponent<Button>();
            helpUI = GameObject.Find("HelpUI").GetComponent<Canvas>();
            helpBtn.onClick.AddListener(OnClickHelpBtn);
            returnBtn.onClick.AddListener(OnClickReturnBtn);

            helpUI.enabled = false;

            if (SettingBtn != null) SettingBtn.onClick.AddListener(OnClickSettingBtn);
            else print("can not find SettingBtn");
            if (ShuxingBtn != null) ShuxingBtn.onClick.AddListener(OnClickShuxingBtn);
            else print("can not find ShuxingBtn");

            
            StartCoroutine(UpdateEnergeData());
        }

        void OnClickReturnBtn()
        {
            AudioManager.Instance.Play("button");
            Time.timeScale = 1;
            helpUI.enabled = !helpUI.enabled;
        }
        public void OnClickHelpBtn()
        {
            AudioManager.Instance.Play("button");
            Time.timeScale = 0;
            helpUI.enabled = !helpUI.enabled;
        }

        /// <summary>
        /// 更新能量条
        /// </summary>
        IEnumerator UpdateEnergeData()
        {
            while (true)
            {
                if (blueFill != null && blueText != null)
                {
                    float mptemp = cData.MP;
                    blueText.text = mptemp + "/" + cData.maxMP;
                    blueFill.fillAmount = ((float)mptemp / cData.maxMP);
                }
                else
                {
                    print("can not find component");
                }

                if (bloodText != null && bloodFill != null)
                {
                    float bloodTemp = cData.HP;
                    bloodText.text = "" + bloodTemp + "/" + cData.maxHP;
                    bloodFill.fillAmount = (bloodTemp / cData.maxHP);
                }
                yield return new WaitForSeconds(1f);
            }
        }

        #region//按键事件管理
        /// <summary>
        /// 按键的事件
        /// </summary>
        public void OnClickSettingBtn()
        {
            AudioManager.Instance.Play("button");
            GameObject.Find("UIManager").GetComponent<UIManager>().OnClickSettingBtn();
        }


        public void OnClickShuxingBtn()
        {
            AudioManager.Instance.Play("button");
            GameObject.Find("UIManager").GetComponent<UIManager>().OnClickShuxingBtn();
        }
        #endregion
    }
}

