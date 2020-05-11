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
	public class LostUI : MonoBehaviour
    {
        private Button lostBtn;
        private Scene scene;

        void Start()
        {
            lostBtn = GameObject.Find("lostBtn").GetComponent<Button>();
            lostBtn.onClick.AddListener(OnClickLostBtn);
            scene = SceneManager.GetActiveScene();
        }

        public void OnClickLostBtn()
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}

