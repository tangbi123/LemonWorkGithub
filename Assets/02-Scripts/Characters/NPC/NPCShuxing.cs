using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    public class NPCShuxing : MonoBehaviour
    {
        public bool hit;
        public float hittime = 0;

        public GameObject hurtTX;
        void Start()
        {
            EventCenter.GetInstance().AddEventListener("BAttack", GetHurt);
        }
        void Update()
        {
            if (hit)
                this.transform.Rotate(Vector3.left,Time.deltaTime * 10000f);
            ChixuKongzhi();
        }
        public void GetHurt()
        {
            //TX
           // hurtTX = ObjPool.GetInstance().GetObj("TX/Hurt");
            //hurtTX.transform.position = this.transform.position;

            if (hittime <= 0)
                hit = true;
            //if(hittime == 0)settrigger;
            hittime += 1.5f;
            if (hittime > 2) hittime = 2;
        }

        public void ChixuKongzhi()
        {

            if (hittime >= 0) hittime -= Time.deltaTime;
            if(hittime<= 0)
            {
                hit = false;
            }
        }

        public void SetHit()
        {
            this.hit = false;
        }
    }
}
