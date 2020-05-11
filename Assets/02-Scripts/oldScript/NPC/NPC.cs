// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
// using Lemon.Character;

// namespace Lemon.AI
// {
//     ///<summary>
//     ///NPC 的控制逻辑：根据距离选择不同的状态，做出不同的动作
//     ///<summary>
// 	public class NPC :MonoBehaviour
//     {
//         public Transform InitPos;//野怪恢复到 idle 状态 会回到固定位置
//         public enum NPCState { idle, trace, attack, die };
//         public NPCState npcState = NPCState.idle;
//         //追击的参数
//         public float traceDist = 20.0f;
//         public float attackDist = 4.0f;

//         public GameObject[] energePrefabs;//死后生成能量
//         public Transform[] energeTrs;//能量的生成地点
//         public GameObject tx1;

//         //组件和目标
//         private Transform npcTr;
//         private NavMeshAgent nvAgent;
//         public Animator anim;
//         private bool isDie = false;
//         //伤害计算的参数
//         private ShuxingManager shuxing;
//         private AnimatorStateInfo playerState;


//         private CharacterDataTB cData;
//         private GameObject player;
//         private bool isActive = true;
//         private GameObject t1;//圈圈特效 临时变量

//         private bool isBiansheng = false;
//         private void Start()
//         {
//             //自身组件
//             npcTr = this.GetComponent<Transform>();
//             player = GameObject.FindWithTag("Player");

//             nvAgent = this.GetComponent<NavMeshAgent>();
//             anim = this.GetComponent<Animator>();
//             //脚本
            
//             cData = GameObject.Find("GameManager").GetComponent<CharacterDataTB>();

//             shuxing = new ShuxingManager();
//             //nvAgent.destination = player.transform.position;
            
//             StartCoroutine(CheckNPCState());
//             StartCoroutine(NPCAction());
//             //StartCoroutine(IsHit());
            
//         }

        
//         public void OnTriggerEnter(Collider coll)
//         {
//             if(coll.gameObject.tag == "Player" && npcState == NPCState.attack)
//             {
//                 if (this.gameObject.name == "BossNPC1")
//                 {
//                     //print(shuxing.HP);
//                     print(cData.HP);
//                     cData.Damage(Random.Range(4, 8));
//                 }
//                 else
//                 {
//                     print(cData.HP);
//                     cData.Damage(Random.Range(3, 5));
//                 }
//             }
//         }

//         //IEnumerator IsHit()
//         //{
//         //    while(true)
//         //    {
//         //        if(isHit)
//         //        {

//         //            isHit = !isHit;
//         //        }
//         //        yield return new WaitForSeconds(0.2f);
//         //    }
//         //}


//         #region  伤害函数
//         /// <summary>
//         /// 普通damage  没有控制
//         /// </summary>
//         public void IsDamage(float damage)
//         {
//             if (this.gameObject.name == "BossNPC1")
//             {
//                 print(shuxing.HP);
//                 damage /= 10f;
//             }
//             shuxing.Damage(damage);

//             if (shuxing.HP <= 0)
//             {
//                 NPCDie();
//             }

//         }
//         /// <summary>
//         /// bigDamage 需要添加控制
//         /// </summary>
//         public void IsBigDamage(float damage)
//         {
//             if (this.gameObject.name == "BossNPC1")
//             {
//                 print(shuxing.HP);
//                 damage /= 10f;
//             }
//             shuxing.Damage(damage * 2);
//             //print(baseShuxing.HP);
//             if (shuxing.HP <= 0)
//             {
//                 NPCDie();
//             }

//             //制造特效
//             anim.SetTrigger("isHit");
//         }
//         public void ConstDamage()
//         {
//             StartCoroutine(CDamage());

//         }
//         IEnumerator CDamage()
//         {
//             float time = 0;
//             while(true)
//             {
//                 float damage = cData.baseATK * 0.8f;
//                 if (this.gameObject.name == "BossNPC1")
//                 {
//                     print(shuxing.HP);
//                     damage /= 10f;
//                 }
//                 shuxing.Damage(damage);
//                 anim.SetTrigger("isHit");
//                 print(shuxing.HP);
//                 if (shuxing.HP <= 0)
//                 {
//                     NPCDie();
//                 }
//                 yield return new WaitForSeconds(0.5f);
//                 time += 0.5f;
//                 if(time >= 2.0f)
//                 {
//                     break;
//                 }
//             }
//             StopCoroutine("CDamage");

//         }

//         #endregion
//         public void NPCDie()
//         {
//             nvAgent.Stop();
//             InstanceEnerge();//生成能量
//             StopAllCoroutines();
//             isDie = true;
//             npcState = NPCState.die;
//             anim.SetTrigger("isDie");
//             Destroy(this.gameObject, 1);
//         }

//         /// <summary>
//         /// 根据npc与 Player的距离选择npc的状态
//         /// </summary>
//         /// <returns></returns>
//         IEnumerator CheckNPCState()
//         {
//             while (!isDie)
//             {
//                 yield return new WaitForSeconds(0.2f);
//                 if (isActive == false)
//                 {
//                     npcState = NPCState.idle;
//                 }
//                 else
//                 {
//                     //更新player 的状态
//                     // playerState = GameObject.FindWithTag("Player").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);


//                     float dist = Vector3.Distance(player.transform.position, npcTr.position);

//                     if (dist <= attackDist) npcState = NPCState.attack;
//                     else if (dist <= traceDist) npcState = NPCState.trace;
//                     else npcState = NPCState.idle;
//                 }
//                 player = GameObject.FindWithTag("Player");
//             }
            
//         }

//         /// <summary>
//         /// 根据npc 的状态 做出不同的动作
//         /// </summary>
//         /// <returns></returns>
//         IEnumerator NPCAction()
//         {
//             while (!isDie)
//             {
//                 switch (npcState)
//                 {
//                     case NPCState.idle:
//                         Idle();
//                         break;
//                     case NPCState.trace:

//                         nvAgent.destination = player.transform.position;
//                         nvAgent.Resume();//继续追击
//                         //LookToPlayer();
//                         anim.SetBool("isAttack", false);
//                         anim.SetBool("isTrace", true);
//                         break;
//                     case NPCState.attack:
//                         nvAgent.Stop();
//                         anim.SetBool("isAttack", true);
//                         break;

//                 }
//                 if (isBiansheng == false)
//                 {
//                     isActive = GameObject.FindWithTag("Player").GetComponent<CharacterMotorTB>().isActive;
//                 }
                
//                 yield return null;
//             }
//         }

//         public void BSDeal()
//         {
//             isBiansheng = !isBiansheng;
//             player = GameObject.FindWithTag("Player");
//         }

//         public void Idle()
//         {
//             //if (this.transform.parent == null)
//             //{
//             //    nvAgent.SetDestination(InitPos.position);
//             //    anim.SetBool("isTrace", true);
//             //    if(this.transform.position == InitPos.position)
//             //    {
//             //        nvAgent.Stop();
//             //        anim.SetBool("isTrace", false);
//             //        print("" + gameObject + this.nvAgent.destination);
//             //    }
//             //}
//             //else
//             //{
//             //    nvAgent.Stop();
//             //}
//             //nvAgent.Stop();
//             //LookToPlayer();
//             if (this.transform.parent != null)
//             {
//                // print("npc  is bei kong zhi");

//             }
//             else if (this.transform.parent == null && this.transform.position != InitPos.position)
//             {
//                 //print("go to InitPos");
//                 nvAgent.destination = InitPos.position;
//                 anim.SetBool("isTrace", true);
//             }
//             else
//             {
//                // print("npc  is idle");
//                 anim.SetBool("isTrace", false);
//             }
//             anim.SetBool("isAttack", false);

//         }
//         /// <summary>
//         /// 死后生成能量
//         /// </summary>
//         public void InstanceEnerge()
//         {
//             //int index = 0;
//             foreach (Transform tr in energeTrs)
//             {
//                 //index = Random.Range(0,2);
//                 Instantiate(energePrefabs[0], tr.position, tr.rotation);
//             }
//         }
        
//         public bool IsAttack()
//         {
//             if (playerState.IsName("attack1") || playerState.IsName("attack2")
//                 || playerState.IsName("attack3"))
//                 return true;
//             else return false;
//         }

//         /// <summary>
//         /// 使NPC面向 player
//         /// 
//         /// </summary>
//         public void LookToPlayer()
//         {
//             Vector3 v = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

//             Quaternion rotation = Quaternion.LookRotation(v - transform.position);

//             this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2.0f);
//         }


//         public void InitTX()
//         {
//             t1 = Instantiate(tx1, this.transform.position, tx1.transform.rotation);
//             t1.transform.parent = this.transform;
//             //t2 = Instantiate(tx2, this.transform.position, Quaternion.identity);
//             //t2.transform.parent = this.transform;
//         }

//         public void DesTX()
//         {
//             Destroy(t1);
//             //Destroy(t2);
//         }

//     }
// }

