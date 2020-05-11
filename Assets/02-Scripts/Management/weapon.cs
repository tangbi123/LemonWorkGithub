// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
// using Lemon.AI;
// using Lemon.audio;

// namespace Lemon.Character
// {
//     ///<summary>
//     ///炸弹的逻辑
//     ///<summary>
// 	public class weapon : MonoBehaviour
//     {
//         private CharacterMotorTB cMotor;
//         public GameObject boomTX;
//         void Start()
//         {
//             cMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMotorTB>();
//             Keep();
//         }

//         /// <summary>
//         /// 找一个最近的NPC，DOMove 向他，碰撞就爆炸，对范围内的NPC造成伤害
//         /// </summary>
//         public void Keep()
//         {
//             GameObject target = cMotor.FindNPCTarget();
//             if(target!=null)
//             {
//                 this.transform.DOMove(target.transform.position, 1f);
//             }
//         }

//         void OnTriggerEnter(Collider coll)
//         {
//             if(coll.gameObject.tag == "NPC")
//             {
//                 AudioManager.Instance.Play("explosion");
//                 GameObject boom = Instantiate(boomTX, coll.transform.position + new Vector3(0, 1, 0), boomTX.transform.rotation);
//                 Destroy(boom, 2f);
//                 BoomBigDamage(coll, 20);
//                 this.GetComponent<SphereCollider>().enabled = !this.GetComponent<SphereCollider>().enabled;
//             }
//             Destroy(this.gameObject, 2f);
//         }

//         public void BoomBigDamage(Collider coll, float damage)
//         {
//             Collider[] objs = Physics.OverlapSphere(coll.transform.position, 5f);

//             if(objs.Length >= 0)
//             {
//                 foreach(Collider obj in objs)
//                 {
//                     if(obj.gameObject.tag == "NPC")
//                     {
//                         obj.gameObject.GetComponent<NPC>().IsBigDamage(damage);
//                     }
//                 }
//             }

//         }
//     }
// }

