using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ns
{
    ///<summary>
    ///
    ///<summary>
	public class SceneMgr : BaseManager<SceneMgr>
    {

        public void LoadScene(string name, UnityAction fun)
        {
            SceneManager.LoadScene(name);
            fun();
        }

        public void LoadSceneAsyn(string name, UnityAction fun)
        {
        }

        private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun)
        {
            yield return null;
        }

    }
}

