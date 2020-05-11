 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// 缓存池 对象 
    /// 
    /// </summary>
    /// 

    public class PoolData
    {
        //  01-抽屉 对象挂载的父节点  文件夹/抽屉
        public GameObject fatherObj;
        //  02-存储容器  存放obj
        public List<GameObject> poolList;

        /// <summary>
        /// 构造函数， 将 obj 装进 poolobj里面
        /// </summary>
        /// <param name="obj">小物体</param >
        /// <param name="poolObj">大池子</param>
        public PoolData(GameObject obj,GameObject poolObj)
        {
            //给抽屉 创建一个 父对象， 并且把他 作为我们 pool对象的子物体
            fatherObj = new GameObject(obj.name);
            fatherObj.transform.parent = poolObj.transform;

            poolList = new List<GameObject>() { };
            PushObj(obj);
        }

        /// <summary>
        /// 将一个 active 的obj  disActive 放进抽屉（fatherOBj）
        /// </summary>
        /// <param name="obj"></param>
        public void PushObj(GameObject obj)
        {
            poolList.Add(obj);
            obj.transform.parent = fatherObj.transform;

            //首先失活 隐藏
            obj.SetActive(false);
        }

        /// <summary>
        /// 从 抽屉里面 取东西， 再激活
        /// </summary>
        /// <returns></returns>
        public GameObject GetObj()
        {
            GameObject obj = poolList[0];

            poolList.RemoveAt(0);

            obj.SetActive(true);

            obj.transform.parent = null;
            return obj;
        }
    }

    ///<summary>
    ///缓存池模块
    ///<summary>
	public class ObjPool: BaseManager<ObjPool>
    {
        //衣柜
        public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
        private GameObject poolObj;//存放所有缓存物体 的 大文件夹

        /// <summary>
        /// 获取物体
        /// </summary>
        /// <param name="name">resources 里的路径</param>
        /// <returns></returns>
        public GameObject GetObj
            (string name, UnityEngine.Events.UnityAction<GameObject> callback)
        {
            GameObject obj = null;
            if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                //obj = poolDic[name].GetObj();
                callback(poolDic[name].GetObj());
            }
            else
            {
                //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
                //obj.name = name;


                ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) =>
                {
                    o.name = name;
                    callback(o);
                    //return o;
                }
                );
            }
            return obj;
        }

        public GameObject GetObj(string name)
        {
            GameObject obj = null;
            if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            {
                obj = poolDic[name].GetObj();
            }
            else
            {
                obj = ResMgr.GetInstance().Load<GameObject>(name);
                obj.name = name;
            }
            return obj;
        }
        public void PushObj(string name,GameObject obj)
        {
            if (poolObj == null)
                poolObj = new GameObject("Pool");

            if (poolDic.ContainsKey(name))
            {
                poolDic[name].PushObj(obj);
            }
            else
            {
                poolDic.Add(name, new PoolData(obj,poolObj));
            }
        }


        /// <summary>
        /// 清空 缓存池
        /// </summary>
        public void Clear()
        {
            poolDic.Clear();
            poolObj = null;
        }
    }
}

