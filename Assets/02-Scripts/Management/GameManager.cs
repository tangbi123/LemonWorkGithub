// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Lemon.audio;
// using Lemon.AI;
// using Lemon.Character;
// /// <summary>
// /// 
// /// </summary>
// public class GameManager : MonoBehaviour
// {
//     public GameObject play;//  player  预制体
//     public Transform InitPos;//Player 出生地点
//     public Transform[] NPCInitPos;//NPC  出身地点
//     public GameObject npcs;//NPC 预制体


//     //变身  公共量
//     public GameObject donghua;
//     public GameObject boss;
//     public Camera c;
//     //变身

//     private CharacterDataTB cData;
//     private GameObject player;//player;
//     private GameObject eyu;//yeyu
//     float time = 0;
//     bool isBianshen = false;//是否变身
//     public bool bsScu = false;//是否成功
//     bool bsToLemon = false;//是否 能量耗尽

//     //变身

//     // Start is called before the first frame update
//     void Awake()
//     {
//         player = Instantiate(play, InitPos.position, InitPos.rotation);
        
//         InitNPC();
//     }
    
//     void Start()
//     {
//         cData = GameObject.Find("GameManager").GetComponent<CharacterDataTB>();
//         eyu = Instantiate(boss, InitPos.position, InitPos.rotation);
//         donghua.active = false;
//         eyu.active = false;
//          bsScu = false;

//         StartCoroutine(BSCost());
//     }
//     public void InitNPC()
//     {
//         GameObject temp;
//         foreach (Transform npc in NPCInitPos)
//         {
//             temp = Instantiate(npcs, npc.position, npc.transform.rotation);
//             temp.GetComponent<NPC>().InitPos = npc;
//         }
//     }

//     void Update()
//     {
//         Bianshen();
//     }


//     public void Bianshen()
//     {
//         if(Input.GetKeyDown(KeyCode.O) && cData.blueEnerge > 0)
//         {

//             //Time.timeScale = 0;
//             time = 0;
//             //cData.blueEnerge -= 0;
//             isBianshen = true;

//             donghua.transform.position = player.transform.position;
//            // player.active = !player.active;
//             c.enabled = !c.enabled;
//             donghua.active = !donghua.active;
//         }

//         //播放动画
//         if(isBianshen)
//         {
//             if(time >= 13)
//             {
//                 //Time.timeScale = 1;
//                 player.active = !player.active;
//                 GameObject.Find("Camera").GetComponent<FollowCam>().enabled =
//                     !GameObject.Find("Camera").GetComponent<FollowCam>().enabled;

//                 donghua.active = !donghua.active;

//                 c.enabled = !c.enabled;

//                 eyu.active = !eyu.active;
//                 eyu.transform.position = player.transform.position;

//                 BSDeal();
//                 //player.GetComponent<CharacterMotorTB>().IsVisible();
//                 //player.GetComponent<CharacterMotorTB>().DesTX();
//                 //player.transform.parent = eyu.transform;

//                 isBianshen = false;
//                 bsScu = true;
//             }
//             time += Time.deltaTime;
//         } 
//     }


//     IEnumerator BSCost()
//     {
//         while (true) { 
//         if (bsScu)
//         {
//             while (true)
//             {
//                 cData.blueEnerge -= 0.25f;
//                 yield return new WaitForSeconds(0.5f);

//                 if (cData.blueEnerge <=0 || Input.GetKeyDown(KeyCode.P))
//                 {
//                     //cData.blueEnerge = 0;
//                     bsScu = false;
//                     bsToLemon = true;
//                        print("bs  to lemon");
//                         break;
//                 }
//             }
//         }

//         if (bsToLemon )
//         {
//            // print("bs  to lemon");
//                 player.active = !player.active;
//                 player.GetComponent<CharacterMotorTB>().Start();

//                 // player.GetComponent<CharacterMotorTB>().IsVisible();
//                 //player.GetComponent<CharacterMotorTB>().InitTX();
//                 //player.transform.parent = null;

//                 player.transform.position = eyu.transform.position;
//                 eyu.active = !eyu.active;

//                 GameObject.Find("Camera").GetComponent<FollowCam>().enabled =
//                 !GameObject.Find("Camera").GetComponent<FollowCam>().enabled;
//                 BSDeal();
                
//             bsToLemon = false;
//         }
//         yield return null;
//         }
//     }

//     public void BSDeal()
//     {
//         GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
//         foreach(var npc in npcs)
//         {
//             npc.GetComponent<NPC>().BSDeal();
//         }
//         //GameObject.FindWithTag("Boss").GetComponent<NPC1>().BSDeal();
//     }
    
// }
