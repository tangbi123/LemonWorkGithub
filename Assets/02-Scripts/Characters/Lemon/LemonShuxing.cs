using GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public class LemonShuxing : BaseShuxing
    {
        public LemonShuxing()
        {
            this.HP = 200;
            this.maxHP = 200;
            this.MP = 50;
            this.maxHP = 200;
        }

        public void Damage(float damage)
        {
            this.HP -= damage;
        }
    }
}
