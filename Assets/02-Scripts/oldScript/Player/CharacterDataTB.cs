using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.Character
{
    ///<summary>
    ///人物基本属性
    ///<summary>
	public class CharacterDataTB:MonoBehaviour
    {
       
        public float HP;//当前生命值
        public float maxHP;//最大生命值
        public float baseATK;//基础攻击力
        public float defence;//减伤比例  0 - 1 
        public float baoji;//暴击  0 - 1 
        public float baojiMul;//暴击效果 
        public float blueEnerge;//蓝色能量
        public float redEnerge;//红色能量

        
        private void Start()
        {
            InitPlayer();
        }


        /// <summary>
        /// 初始化属性
        /// </summary>
        public void InitPlayer()
        {
            this.HP = 100;
            this.maxHP = 100;
            this.baseATK = 20;
            this.defence = 0;
            this.baoji = 0;
            this.baojiMul = 0;
            this.blueEnerge = 0;
            this.redEnerge = 0;
        }

        /// <summary>
        /// 数据的复制
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public CharacterDataTB PlayerCpy(CharacterDataTB player)
        {
            CharacterDataTB newplayer = new CharacterDataTB();
            newplayer.HP = player.HP;
            newplayer.maxHP = player.maxHP;
            newplayer.baseATK = player.baseATK;
            newplayer.defence = player.defence;
            newplayer.baoji = player.baoji;
            newplayer.baojiMul = player.baojiMul;
            newplayer.blueEnerge = player.blueEnerge;
            newplayer.redEnerge = player.redEnerge;
            return newplayer;
        }

        /// <summary>
        /// 碰撞函数
        /// </summary>
        /// <param name="coll"></param>
        //void OnColliderEnter(Collision coll)
        //{
        //    if(coll.gameObject.tag == "attack")
        //    {
        //        HP -= Random.Range(1,5);
        //    }
        //}

        /// <summary>
        /// 与碰撞有关的参数
        /// </summary>
        /// <param name="coll"></param>
        void OnTriggerEnter(Collider coll)
        {
            //收集能量
            if (coll.gameObject.tag == "blueEnerge")
            {
                Destroy(coll.gameObject);
                blueEnerge += 2;
                if (blueEnerge >= 100) blueEnerge = 100;
                //print("blueEnerge" + blueEnerge);
            }

        }

        public void Damage(float damage)
        {
            this.HP -= damage;
        }
    }
}

