// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Lemon.Character;
// using DG.Tweening;
// using Lemon.AI;
// using Lemon.audio;

// namespace Lemon.Character
// {
//     ///<summary>
//     ///负责有关于Player 位移运动的函数
//     ///<summary>
// 	public class CharacterMotorTB : MonoBehaviour
//     {
//         //特效
//         public Transform attackPos;
//         public GameObject attackEnerge;
//         public GameObject jq;
//         public GameObject fangyu;
//         //特效

//         public int speed = 15;
//         public bool isActive;
//         public GameObject tx1;//圈圈特效  
//         //public GameObject tx2;
//         public GameObject bigTX;


//         private Animator anim;
//         private AnimatorStateInfo animatorStateInfo;
//         //private int attackCount = 0;//控制动画
//         private CharacterDataTB cData;
        
//         private GameObject targetNPC;//

//         //特效的临时变量
//         private GameObject jiqiTemp;
//         private GameObject fangyuTemp;
//         private GameObject t1;//圈圈特效 临时变量
//         private GameObject t1Target;//附体环境的  临时特效
//         private GameObject big;

    
//         //特效的临时变量

//         //private GameManager gm;
//         public void Start()
//         {
//             cData = GameObject.Find("GameManager").GetComponent<CharacterDataTB>();
//             InitTX();
//             isActive = true;
            
//             if (!(anim = this.GetComponent<Animator>())) print("can not find Animator");

//             StartXiecheng();

//            // gm = GameObject.Find("GameManager").GetComponent<GameManager>();
//         }

//         public void StartXiecheng()
//         {
//             StartCoroutine(CheckAnimState());
//             StartCoroutine(FutiHuanjing());
//         }
//         private void Update()
//         {
//             //if (Input.GetKeyDown(KeyCode.P))
//             //{
//             //    Debug.Log("TX");
//             //    InitTX();
//             //}
//             if (animatorStateInfo.IsName("idle") /*&& !gm.bsScu*/ )
//             {
//                 //print("I can move");
//                 Move();
//             }
//             animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
//             AllInputManage();
            
//         }

//         void OnTriggerEnter(Collider coll)
//         {
//             //收集能量
//             if (coll.gameObject.tag == "blueEnerge")
//             {
//                 Destroy(coll.gameObject);
//                 cData.blueEnerge += 3;
//                 if (cData.blueEnerge >= 100) cData.blueEnerge = 100;
//             }

//         }
//         ////旋转
//         //public void LookAtTarget(Transform target)
//         //{
//         //    //transform.rotation = Quaternion.LookRotation(dir);
//         //    Quaternion.Lerp(this.transform.rotation, target.rotation, 1);
//         //}


//         /// <summary>
//         /// 简单移动
//         /// </summary>
//         public void Move()
//         {
//             float horizontal = Input.GetAxis("Horizontal");
//             float vertical = Input.GetAxis("Vertical");
//             Vector3 moveDir = new Vector3(horizontal, 0, vertical);
//             //print(moveDir);

//             //人物移动 和 转向
//             this.transform.Translate(moveDir.normalized* speed * Time.deltaTime);

//             Quaternion quaDir = Quaternion.LookRotation(moveDir, Vector3.up);

//             //float rotationY = 0;
//             //if(horizontal == 0)
//             //{
//             //    if (vertical != 0) rotationY = this.transform.position.y;
//             //}
//             //else
//             //{
//             //    rotationY = horizontal * 45 * (2 - vertical);
//             //}
//             //Quaternion target = Quaternion.Euler(0, rotationY, 0);

//             this.transform.rotation = Quaternion.Slerp(transform.rotation, quaDir, Time.deltaTime * 0.5f);
//             //this.transform.eulerAngles = new Vector3(0, Quaternion.FromToRotation(Vector3.forward, 
//               // moveDir).eulerAngles.y, 0);
            

//             //Debug.Log(this.transform.childCount);

//             if ( targetNPC != null && targetNPC.gameObject.tag == "NPC" && this.transform.childCount == 5)
//             {
//                 if (moveDir.magnitude >= 0.005)
//                 {
//                     //print("futi  zou" + this.transform.childCount);
//                     targetNPC.GetComponent<NPC>().anim.SetBool("isTrace", true);
//                 }
//                 else
//                 {
//                     targetNPC.GetComponent<NPC>().anim.SetBool("isTrace", false);
//                 }
//             }
//         }


//         /// <summary>
//         /// 连招test
//         /// </summary>
//         public void Attack1()
//         {

//             if (animatorStateInfo.IsName("idle") || animatorStateInfo.IsName("move"))
//             {
//                 anim.SetTrigger("attack1");
//                 //attackCount++;
//                 //print("1");
//             }
//             else if (animatorStateInfo.IsName("attack1") && animatorStateInfo.normalizedTime > 0.5f)
//             {
//                 anim.SetTrigger("attack1");
//                 //print("2");
//             }

//         }

//         //防止动作 卡死，变回idle
//         IEnumerator CheckAnimState()
//         {
//             while (true)
//             {
//                 //animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
//                 if (!animatorStateInfo.IsName("idle") && animatorStateInfo.normalizedTime >= 1f)
//                 {
//                     anim.SetTrigger("isIdle");
//                     //attackCount = 0;
//                 }
//                 yield return new WaitForSeconds(0.5f);
//             }
//         }

//         #region  动作的函数
//         public void BigAttack()
//         {
//              StartCoroutine(BAttack());
//             anim.SetTrigger("bigAttack");
//         }

//         IEnumerator BAttack()
//         {
//             DesTX();
//             isActive = false;
//             yield return new WaitForSeconds(1.8f);
//             isActive = true;
//             big = Instantiate(bigTX, this.transform.position, bigTX.transform.rotation);
//             BigDamage(this.GetComponent<Collider>(), 25);
//             InitTX();
//             AudioManager.Instance.Play("explosion");
//             yield return new WaitForSeconds(2f);
//             Destroy(big);
//             StopCoroutine("BAttack");
//         }
//         public void BigDamage(Collider coll, float damage)
//         {
//             Collider[] objs = Physics.OverlapSphere(coll.transform.position, 10f);

//             if (objs.Length >= 0)
//             {
//                 foreach (Collider obj in objs)
//                 {
//                     if (obj.gameObject.tag == "NPC")
//                     {
//                         obj.gameObject.GetComponent<NPC>().IsBigDamage(damage);
//                     }
//                 }
//             }
//         }

//         #region//集齐代码，还要加特效

//         public void Jiqi1()
//         {
//             //t1.transform.localScale *= 2;
//             anim.SetTrigger("jiqi1");
//             jiqiTemp = Instantiate(jq, this.transform.position + new Vector3(0,0.5f,0), Quaternion.identity); 
//         }
//         public void Jiqi2()
//         {

//             anim.SetTrigger("jiqi2");
//         }
//         public void Jiqi3()
//         {
//            // t1.transform.localScale /= 2;
//             anim.SetTrigger("jiqi3");
//             Destroy(jiqiTemp);
//         }

//         #endregion

//         #region//防御代码，还要加特效,还要加效果
//         public void Fangyu1()
//         {
//             anim.SetTrigger("fangyu1");
//             fangyuTemp = Instantiate(fangyu, this.transform.position, Quaternion.identity);
            
//         }
//         public void Fangyu2()
//         {

//             anim.SetTrigger("fangyu2");
//         }
//         public void Fangyu3()
//         {
//             Destroy(fangyuTemp);
//             anim.SetTrigger("fangyu3");
//         }

//         #endregion

//         public void Yuangong()
//         {
//             AudioManager.Instance.Play("CastSkill");
//             anim.SetTrigger("yuangong");
//         }

// #endregion

//         /// <summary>
//         /// K 附体 i 解体  L集气  Q防御  U远攻  E大招
//         /// </summary>
//         public void AllInputManage()
//         {
//             if (Input.GetKeyDown(KeyCode.I) && cData.blueEnerge >= 10)
//             {
//                 cData.blueEnerge -= 10;
//                 BigAttack();
//             }

//             //if (Input.GetKeyDown(KeyCode.J))
//             //{
//             //    Attack1();
//             //}

//             if (isActive == true && Input.GetKeyDown(KeyCode.H))
//             {
                
//                 //附体
//                 FutiNPC();
//             }
//             if (isActive == false && Input.GetKeyDown(KeyCode.Y))
//             {
//                 //解体
//                 JietiNPC();
//             }

//             //按L集气
//             if (animatorStateInfo.IsName("idle") && Input.GetKeyDown(KeyCode.T))
//             {
//                 Jiqi1();
//             }
//             else if (animatorStateInfo.IsName("jiqi1") && Input.GetKey(KeyCode.T))
//             {
//                 Jiqi2();

//             }
//             else if (animatorStateInfo.IsName("jiqi2") && Input.GetKeyUp(KeyCode.T))
//             {
//                 Jiqi3();
//             }


//             //按Q防御
//             if (animatorStateInfo.IsName("idle") && Input.GetKeyDown(KeyCode.Q))
//             {
//                 Fangyu1();
//             }
//             else if (animatorStateInfo.IsName("fangyu1") && Input.GetKey(KeyCode.Q))
//             {
//                 Fangyu2();
//             }
//             else if (animatorStateInfo.IsName("fangyu2") && Input.GetKeyUp(KeyCode.Q))
//             {
//                 Fangyu3();
//             }


//             //按U远攻
//             if (Input.GetKeyDown(KeyCode.U) && cData.blueEnerge >= 5)
//             {
//                 cData.blueEnerge -= 5;
//                 Instantiate(attackEnerge, attackPos.position, attackEnerge.transform.rotation);
//                 Yuangong();
//             }
//             if (Input.GetKeyDown(KeyCode.U))
//             {
//                 anim.SetTrigger("isIdle");
//             }
//         }

//         public GameObject FindNPCTarget()
//         {
//             GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
//             if (npcs.Length == 0) return null;

//             float mindist = 100, tempDist;
//             GameObject targetChongci = null;
//             //找到最近的npc
//             foreach (GameObject npc in npcs)
//             {
//                 tempDist = Vector3.Distance(this.transform.position, npc.transform.position);
//                 //冲刺最远距离为50
//                 if (tempDist < mindist && tempDist < 50)
//                 {
//                     targetChongci = npc;
//                     mindist = tempDist;
//                 }
//             }
//             return targetChongci;
//         }

//         /// <summary>
//         /// 附体的细节：在5米范围内，先选定目标，然后用DoMove()
//         /// </summary>
//         public void FutiNPC()
//         {
//             targetNPC = FindNPCTarget();
//             //
//             if (targetNPC != null && Vector3.Distance(this.transform.position,targetNPC.transform.position) <= 5)
//             {
//                 AudioManager.Instance.Play("buff");
//                 IsVisible();
//                 DesTX();

//                 this.transform.DOMove(targetNPC.transform.position, 0.8f);
//                 isActive = !isActive;

//                 //targetNPC.GetComponent<NPC>().enabled = false;
//                 targetNPC.GetComponent<NPC>().InitTX();
//                 targetNPC.transform.parent = this.transform;
//                 targetNPC.transform.forward = this.transform.forward;

//                 print("futi" + this.transform.childCount);
//             }
//             else Debug.Log("can not find targetNPC");
//         }


//         public void JietiNPC()
//         {
//             InitTX();
//             this.transform.position += new Vector3(2, 0, 2);
//             IsVisible();
//             isActive = !isActive;
//             targetNPC.transform.parent = null;
//             targetNPC.GetComponent<NPC>().DesTX();
//         }



//         public GameObject FindEnvironmentTarget()
//         {
//             GameObject[] npcs = GameObject.FindGameObjectsWithTag("energe");
//             if (npcs.Length == 0) return null;
//             //print("environment" + npcs.Length);

//             float mindist = 100, tempDist;
//             GameObject targetChongci = null;
//             //找到最近的npc
//             foreach (GameObject npc in npcs)
//             {
//                 tempDist = Vector3.Distance(this.transform.position, npc.transform.position);
//                 //冲刺最远距离为50
//                 if (tempDist < mindist && tempDist < 50)
//                 {
//                     targetChongci = npc;
//                     mindist = tempDist;
//                 }
//             }
//             return targetChongci;
//         }

//         /// <summary>
//         /// 附体的细节：在5米范围内，先选定目标，然后用DoMove()
//         /// </summary>
//         public void FutiEnvironment()
//         {
//             targetNPC = FindEnvironmentTarget();
//             print(targetNPC);
//             //
//             if (targetNPC != null /* && Vector3.Distance(this.transform.position, targetNPC.transform.position) <= 20*/)
//             {
//                 AudioManager.Instance.Play("buff");
//                 IsVisible();
//                 DesTX();

//                 this.transform.DOMove(targetNPC.transform.position, 0.5f);
//                 isActive = !isActive;

                
//                 t1Target = Instantiate(tx1, targetNPC.transform.position, tx1.transform.rotation);
//                 t1Target.transform.localScale *= 2;

//                // print("futi" + this.transform.childCount);
//             }
//             else Debug.Log("can not find targetNPC");
//         }


//         public void JietiEnvironment()
//         {
//             Destroy(targetNPC);
//             Destroy(t1Target);
//             //tx1.transform.localScale /= 2;
//             InitTX();
//             //this.transform.position += new Vector3(2, 0, 2);
            
//             isActive = !isActive;
//             //targetNPC.transform.parent = null;
//            // targetNPC.GetComponent<NPC>().DesTX();
//         }

//         public IEnumerator  FutiHuanjing()
//         {
//             while (true)
//             {
//                 if (Input.GetKeyDown(KeyCode.L))
//                 {
//                     FutiEnvironment();
//                     if (targetNPC != null)
//                     {
//                         yield return new WaitForSeconds(1f);
//                         IsVisible();
//                         Jiqi1();
//                         Jiqi2();
//                         yield return new WaitForSeconds(2f);

//                         JietiEnvironment();
//                         Jiqi3();
//                         cData.blueEnerge += 10;
//                         if (cData.blueEnerge >= 100) cData.blueEnerge = 100;
//                     }
                    
//                     anim.SetTrigger("isIdle");
//                 }

//                 yield return null; ;
//             }
//         }
//         /// <summary>
//         /// 根据名字查找子物体并返回
//         /// </summary>
//         /// <param name="name"></param>
//         /// <returns></returns>
//         public GameObject FindChild(string name)
//         {
//             Transform[] children = this.GetComponentsInChildren<Transform>();
//             foreach (var child in children)
//             {
//                 if (child.gameObject.name == name)
//                 {
//                     return child.gameObject;
//                 }
//             }
//             return null;
//         }
//         public void IsVisible()
//         {
//             //this.GetComponent<BoxCollider>().enabled = !this.GetComponent<BoxCollider>().enabled;
//             this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = 
//                 !this.GetComponentInChildren<SkinnedMeshRenderer>().enabled;
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
//            //Destroy(t2);
//         }

//     }

// }

