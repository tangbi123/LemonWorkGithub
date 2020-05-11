using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///<summary>
	public class DelayPush : MonoBehaviour
    {
        //对象激活时的函数
        void OnEnable()
        {
            Invoke("Push", 1);
        }
        void Push()
        {
            ObjPool.GetInstance().PushObj(this.gameObject.name, this.gameObject);
        }
    }
}

