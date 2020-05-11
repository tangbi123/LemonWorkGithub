using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{

    public class TT
    {
        public void Update()
        {
            Debug.Log("lksdjfds");
        }
    }
    ///<summary>
    ///
    ///<summary>
	public class test : MonoBehaviour
    {
        //void Start()
        //{
        //    //TT t = new TT();
        //    //MonoManager.GetInstance().AddUpdateListener(t.Update);
        //    InputMgr.GetInstance().StartOrEndCheck(true);
        //    EventCenter.GetInstance().AddEventListener<KeyCode>("某建按下", CheckInputDown);
        //     EventCenter.GetInstance().AddEventListener<KeyCode>("某键抬起", CheckInputUp);
        //}

        private void CheckInputDown(KeyCode key)
        {
            KeyCode code = (KeyCode)key;
            switch (code)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    break;
            }

        }

        private void CheckInputUp(KeyCode key)
        {
            Debug.Log(key.ToString() + "456");
        }
        //void Update()
        //{
        //    //if (Input.GetMouseButtonDown(0))
        //    //{
        //    //    ObjPool.GetInstance().GetObj("TXPrefabs/Cube", (o) =>
        //    //    { o.transform.localScale *= 2; });
        //    //    //ResMgr.GetInstance().Load<GameObject>("TXPrefabs/Cube");
        //    //}

        //    //if(Input.GetMouseButtonDown(1))
        //    //    {
        //    //    //ObjPool.GetInstance().GetObj("TXPrefabs/Sphere");
        //    //    //ResMgr.GetInstance().LoadAsync<GameObject>("TXPrefabs/Sphere", (obj) => {
        //    //        ///obj.transform.localScale *= 2;

        //    //    //});
        //    //}

        //}

        //private void DoSomething(GameObject obj)
        //{

        //}
    }
}

