using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ns
{
    ///<summary>
    ///
    ///<summary>
	public class MonoController : MonoBehaviour
    {
        public event UnityAction updateEvent;

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (updateEvent != null)
                updateEvent();
        }

        /// <summary>
        /// 给外部提供添加 帧 更新 事件函数
        /// </summary>
        /// <param name="fun"></param>
        public void AddUpdateListener(UnityAction fun)
        {
            updateEvent += fun;
        }

        public void RemoveUpdateListener(UnityAction fun)
        {
            updateEvent -= fun;
        }

        
        
    }
}

