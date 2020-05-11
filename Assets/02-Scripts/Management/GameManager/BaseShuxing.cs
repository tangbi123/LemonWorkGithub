using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    ///<summary>
    ///基本属性
    ///<summary>
	public class BaseShuxing
    {
        public static float maxEnerge = 100;

        public float HP ;
        public float maxHP ;
        public float MP;
        public float maxMP;
        
        public BaseShuxing lemonshuxing;
        public BaseShuxing()
        {
            InitShuxing();
        }

        public void InitShuxing()
        {
            HP = 200;
            maxHP = 200;
            MP = 40;
            maxMP = 100;
        }

        public void ChangeToShuren()
        {
            lemonshuxing = this;
            HP = 800;
            maxHP = 800;
            MP = 400;
            maxMP = 400;
        }

        public void ChangeToEyu()
        {
            lemonshuxing = this;
            HP = 1200;
            maxHP = 1200;
            MP = 300;
            maxMP = 300;
        }

        public void ChangeToLemon()
        {
            this.HP = lemonshuxing.HP;
            this.MP = lemonshuxing.MP;
            this.maxHP = lemonshuxing.maxHP;
            this.maxMP = lemonshuxing.maxMP;
        }
        public virtual void Damage()
        {

        }

    }
}

