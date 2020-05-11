using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnitStates : MonoBehaviour
    {
        public float health;
        public float mana;
        public float attack;
        public float magic;
        public float defense;
        public float speed;
        public float normalDis;
        public float skillDis;
        private AnimatorStateInfo animStateInfo;
        
        public void Select(string str)
        {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("");
            List<GameObject> tempList = new List<GameObject>();

            //选怪放入tempLIst中
            for( int i = 0; i < enemy.Length; i++)
            {
                float diss = Vector3.Distance(this.transform.position, enemy[i].transform.position - this.transform.position);

                if(str == "")
                {
                    float angle = Vector3.Angle(this.transform.forward, enemy[i].transform.position - this.transform.position);

                    if(diss < normalDis && angle < 50)
                    {
                        tempList.Add(enemy[i]);
                    }
                }
                else if(str == "")
                {
                    if(diss < skillDis)tempList.Add(enemy[i]);

                }
            }

            //对tempList中的怪进行处理
            foreach (var objects in tempList)
            {
                if(objects.GetComponent<Rigidbody>() != null && str == "BigScale")
                {
                    objects.GetComponent<Rigidbody>().freezeRotation = true;
                    objects.GetComponent<Rigidbody>().AddExplosionForce(200, this.transform.position, 5);
                }
                //掉血函数
                //objects.GetComponent<jiaoben>().Damage(100);
                //StartCoroutine(objects);
            }
        }

    }

