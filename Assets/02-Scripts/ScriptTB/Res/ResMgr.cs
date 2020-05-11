using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ns
{
    ///<summary>
    ///
    ///<summary>
	public class ResMgr :BaseManager<ResMgr>
    {

        /// <summary>
        /// 同步加载物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">resources 里的路径</param>
        /// <returns></returns>
        public T Load<T>(string name) where T:Object
        {
            T res = Resources.Load<T>(name);
            if(res is GameObject)
            {
                return GameObject.Instantiate(res);
            }
            else return res;
        }

        public void LoadAsync<T>(string name, UnityAction<T> callback)where T : Object
        {
            MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name, callback));
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator ReallyLoadAsync<T> 
            (string name, UnityAction<T> callback)where T : Object
        {
            ResourceRequest r = Resources.LoadAsync<T>(name);
            yield return r;

            if (r.asset is GameObject)
            {
                callback(GameObject.Instantiate(r.asset) as T);
            }
            else callback(r.asset as T);
        }
    }
}

