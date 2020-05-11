using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    ///<summary>
    ///
    ///<summary>
	public class WinAll : MonoBehaviour
    {
        private Button nextGame;

        void Start()
        {
            nextGame = GameObject.Find("nextGame").GetComponent<Button>();
            nextGame.onClick.AddListener(OnClickNextgameBtn);
        }

        public void OnClickNextgameBtn()
        {
            Globe.nextSceneName = "01-Start";
            SceneManager.LoadScene("04-Loading");
        }
    }
}

