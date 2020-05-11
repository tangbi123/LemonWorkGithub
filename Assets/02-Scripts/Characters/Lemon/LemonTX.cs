using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ns;
using DG.Tweening;
using GameManager;

namespace Lemon
{
    /// <summary>
    /// 管理特效的生成 销毁，  人物属性等
    /// </summary>
    public class LemonTX : MonoBehaviour
    {
        public BaseShuxing myShuxing;

        private SkinnedMeshRenderer m_Skin;

        //TX
        private GameObject huan;
        private GameObject jiqi;
        private GameObject bigA;

        void Start()
        {
            myShuxing = GameObject.Find("GameManager").GetComponent<GameMgr>().shuxing;
            InitHuan();
            m_Skin = GetComponentInChildren<SkinnedMeshRenderer>();

            //InitShuxing();
        }

        // public void InitShuxing()
        // {
        //     myShuxing.maxHP = 200;
        //     myShuxing.HP = 200;
        //     myShuxing.maxMP = 100;
        //     myShuxing.MP = 50;
        // }

        // private void OnEnable()
        // {
            
        // }

        
        public void ChangeVisible()
        {
            m_Skin.enabled = !m_Skin.enabled;
        }

        public void InitBig()
        {
            bigA = ObjPool.GetInstance().GetObj("TX/BigAttack");
            bigA.transform.position = this.transform.position;
            bigA.transform.parent = this.transform;
        }
        public void DesBig()
        {
            ObjPool.GetInstance().PushObj(bigA.name, bigA);
        }
        /// <summary>
        /// 生成  环特效
        /// </summary>
        public void InitHuan()
        {
            huan = ObjPool.GetInstance().GetObj("TX/Huan");
            //print(huan.name);
            huan.transform.position = this.transform.position;
            huan.transform.parent = this.transform;
        }

        /// <summary>
        /// 生成集气特效
        /// </summary>
        public void InitJiqi()
        {
            jiqi = ObjPool.GetInstance().GetObj("TX/Jiqi");
            //print(huan.name);
            jiqi.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
            jiqi.transform.parent = this.transform;
        }



        public void DesJiqi()
        {
            ObjPool.GetInstance().PushObj(jiqi.name, jiqi);
        }
        public void DesHuan()
        {
                ObjPool.GetInstance().PushObj(huan.name, huan);
        }

        public void BigAttack()
        {

        }


        /// <summary>
        /// 生成 能量球， 减少能量
        /// </summary>
        public void Yuangong()
        {
            myShuxing.MP -= 10;
            GameObject qiu = ObjPool.GetInstance().GetObj("TX/Nengliangqiu");
            qiu.transform.position = this.transform.position;
        }


        /// <summary>
        /// 查找能量石
        /// </summary>
        /// <returns></returns>
        public GameObject FindNengliangTarget()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("energe");

            if (objs.Length == 0) return null;
            //mindist 作为初值 作比较， tempDist作为范围
            float mindist = 100, tempDist;
            GameObject targetObj = null;

            foreach(GameObject obj in objs)
            {
                tempDist = Vector3.Distance(this.transform.position,
                    obj.transform.position);
                if(tempDist < mindist && tempDist  < 50)
                {
                    targetObj = obj;
                    mindist = tempDist;
                }
            }
            return targetObj;
        }

        /// <summary>
        /// 实现附体能量石
        /// 1、隐身， 去掉环 特效
        /// 2、0.5f 移动到目标位置
        /// 3、设置不能被攻击
        /// </summary>
        public GameObject FutiNengliangshi()
        {
            GameObject targetobj = FindNengliangTarget();

            if (targetobj != null)
            {
                ChangeVisible();
                DesHuan();

                //this.transform.DOMove(targetobj.transform.position, 0.5f);

                //变量 不能攻击
                this.transform.position = targetobj.transform.position;
            }
            else Debug.Log("can not find nenglinagshi");

            return targetobj;
        }

        public void JietiNengliangshi(GameObject obj)
        {
            if(obj != null)
            Destroy(obj);
            DesJiqi();
            InitHuan();
            //huan.transform.localScale /= 2;
            // 变量 可以攻击
        }

        
    }
}
