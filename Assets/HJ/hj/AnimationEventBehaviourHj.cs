//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Lemon.AI;
//using Lemon.Character;
//using Lemon.audio;
//public class AnimationEventBehaviourHj : MonoBehaviour
//{
//    //public GameObject mixamorig_HeadTop_End;
//    //public GameObject mixamorig_LeftHand;
//    //public GameObject mixamorig_RightHand;
//    public ParticleSystem fire_par;
//    public ParticleSystem left;
//    public ParticleSystem right;
//    public ParticleSystem down;
//    //ParticleSystem

//    private CharacterDataTB cData;
//    public void Start()
//    {
//        //fire_par = mixamorig_HeadTop_End.GetComponent<ParticleSystem>();
//        cData = GameObject.Find("GameManager").GetComponent<CharacterDataTB>();
//    }
//    /// <summary>
//    /// 右键 喷火
//    /// </summary>
//    public void boss_fire()
//    {

//        AudioManager.Instance.Play("fire");
//        ConstDamage(this.GetComponent<Collider>());

//        print("boss fire");
//        fire_par.Play();
//    }

//    public void boss_left()
//    {
//        AudioManager.Instance.Play("left");
//        BoomBigDamage(this.GetComponent<Collider>(), 10f);
//        //BoomDamage(this.GetComponent<Collider>());
//        left.Play();
//    }
//    public void boss_right()
//    {
//        AudioManager.Instance.Play("right");
//        //BoomBigDamage(this.GetComponent<Collider>());
//        BoomDamage(this.GetComponent<Collider>(), 10f);
//        right.Play();
//    }

//    /// <summary>
//    /// Q  跳下的攻击
//    /// </summary>
//    public void boss_down()
//    {
//        AudioManager.Instance.Play("explosion");
//        print("boss jump and down");
//        down.Play();

//        BoomBigDamage(this.GetComponent<Collider>(), 50f);
//    }

//    public void BoomBigDamage(Collider coll, float damage)
//    {
//        Collider[] objs = Physics.OverlapSphere(coll.transform.position, 5f);

//        if (objs.Length >= 0)
//        {
//            foreach (Collider obj in objs)
//            {
//                if (obj.gameObject.tag == "NPC")
//                {
//                    obj.gameObject.GetComponent<NPC>().IsBigDamage(damage);
//                }
//            }
//        }
//    }
//    public void BoomDamage(Collider coll, float damage)
//    {
//        Collider[] objs = Physics.OverlapSphere(coll.transform.position, 5f);

//        if (objs.Length >= 0)
//        {
//            foreach (Collider obj in objs)
//            {
//                if (obj.gameObject.tag == "NPC")
//                {
//                    obj.gameObject.GetComponent<NPC>().IsDamage(damage);
//                }
//            }
//        }
//    }

//    public void ConstDamage(Collider coll)
//    {
//        Collider[] objs = Physics.OverlapSphere(coll.transform.position, 5f);

//        if (objs.Length >= 0)
//        {
//            foreach (Collider obj in objs)
//            {
//                if (obj.gameObject.tag == "NPC")
//                {
//                    obj.gameObject.GetComponent<NPC>().ConstDamage();
//                }
//            }
//        }
//    }


//    void OnTriggerEnter(Collider coll)
//    {
//        //收集能量
//        if (coll.gameObject.tag == "blueEnerge")
//        {
//            Destroy(coll.gameObject);
//            cData.blueEnerge += 3;
//            if (cData.blueEnerge >= 100) cData.blueEnerge = 100;
//        }

//    }
//}
