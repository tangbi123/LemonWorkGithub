using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Character;
using Lemon.audio;
using GameManager;

namespace UI
{
    ///<summary>
    ///属性界面的UI,点开属性面板才更新数据，减少开销
    ///<summary>
	public class ShuxingUI : MonoBehaviour
    {
        private Text HPText;
        private Text BEnergeText;
        //private Text ATKText;
        private Image BEnergeFill;
        private Image HPFill;
        private BaseShuxing cData;
        private Button ShuxingReturn;

        private void Start()
        {
            

            HPText = GameObject.Find("HPText").GetComponent<Text>();
            BEnergeText = GameObject.Find("BEnergeText").GetComponent<Text>();
            //ATKText = GameObject.Find("ATKText").GetComponent<Text>();
            BEnergeFill = GameObject.Find("BEnergeFill").GetComponent<Image>();
            HPFill = GameObject.Find("HPFill").GetComponent<Image>();
            
            cData = GameObject.Find("GameManager").GetComponent<GameMgr>().shuxing;

            ShuxingReturn = GameObject.Find("ShuxingReturn").GetComponent<Button>();
            ShuxingReturn.onClick.AddListener(OnClickShuxingReturnBtn);
            

        }

        /// <summary>
        /// 更行属性里的数据
        /// 
        /// </summary>
        public void UpdateShuxing()
        {
            if (GameObject.FindWithTag("Player").active == true)
            {
                HPText.text = "" + cData.HP + "/" + cData.maxHP;
                BEnergeText.text = "" + cData.MP + "/" + cData.maxMP;

                HPFill.fillAmount = ((float)cData.HP / cData.maxHP);
                BEnergeFill.fillAmount = ((float)cData.MP / cData.maxMP);
            }
        }
        
        public void OnClickShuxingReturnBtn()
        {
            AudioManager.Instance.Play("button");
            GameObject.Find("UIManager").GetComponent<UIManager>().OnClickShuxingReturnBtn();
        }
    }
}

