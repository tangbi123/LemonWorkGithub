using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace ns
{
    ///<summary>
    ///提供外部 update
    ///IEnumerator
    ///<summary>
	public class MonoManager : BaseManager<MonoManager>
    {
        public MonoController controller;

        public MonoManager()
        {
            GameObject obj = new GameObject("MonoController");
            controller = obj.AddComponent<MonoController>();
        }

        /// <summary>
        /// 给外部提供添加 帧 更新 事件函数
        /// </summary>
        /// <param name="fun"></param>
        public void AddUpdateListener(UnityAction fun)
        {
            controller.AddUpdateListener(fun);
        }

        public void RemoveUpdateListener(UnityAction fun)
        {
            controller.RemoveUpdateListener(fun);
            
        }

        #region 携程包装
        public Coroutine StartCoroutine(string methodName)
        {
            return controller.StartCoroutine(methodName);
        }
        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return controller.StartCoroutine(methodName, value);
        }
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            controller.StopCoroutine(routine);
        }
        public void StopCoroutine(Coroutine routine)
        {
            controller.StopCoroutine(routine);
        }
        public void StopCoroutine(string methodName)
        {
            controller.StopCoroutine(methodName);
        }
        #endregion
    }
}

