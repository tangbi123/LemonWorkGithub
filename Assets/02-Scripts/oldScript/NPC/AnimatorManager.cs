using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace ns
{
    ///<summary>
    ///控制动画
    ///<summary>
	public class AnimatorManager : MonoBehaviour
    {
        public enum NPCState { idle, trace, attack, die, max};
        public NPCState npcState = NPCState.idle;

        public float traceDist = 20.0f;
        public float attackDist = 4.0f;

        private Transform npcTr;
        private NavMeshAgent nvAgent;
        private Animator anim;
        private bool isDie = false;

        private GameObject player;

        private void Start()
        {
            npcTr = this.gameObject.transform;
            nvAgent = this.GetComponent<NavMeshAgent>();
            anim = this.GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");

            nvAgent.destination = player.transform.position;

            StartCoroutine(CheckNPCState());
            StartCoroutine(NPCAction());
        }

      
        public void NPCDie()
        {
            
            nvAgent.Stop();
            //InstanceEnerge();//生成能量
            StopAllCoroutines();
            isDie = true;
            npcState = NPCState.die;
            anim.SetTrigger("isDie");
            Destroy(this.gameObject, 1);
        }

        /// <summary>
        /// 根据npc与 Player的距离选择npc的状态
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckNPCState()
        {
            while (!isDie)
            {
                ////更新player 的状态
                //playerState = GameObject.FindWithTag("Player").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

                yield return new WaitForSeconds(0.2f);
                float dist = Vector3.Distance(player.transform.position, npcTr.position);

                if (dist <= attackDist) npcState = NPCState.attack;
                else if (dist <= traceDist) npcState = NPCState.trace;
                else npcState = NPCState.idle;
            }
        }

        /// <summary>
        /// 根据npc 的状态 做出不同的动作
        /// </summary>
        /// <returns></returns>
        IEnumerator NPCAction()
        {
            while (!isDie)
            {
                switch (npcState)
                {
                    case NPCState.idle:
                        nvAgent.Stop();
                        anim.SetBool("isTrace", false);
                        break;
                    case NPCState.trace:
                        nvAgent.destination = player.transform.position;
                        nvAgent.Resume();//继续追击
                        LookToPlayer();
                        anim.SetBool("isAttack", false);
                        anim.SetBool("isTrace", true);
                        break;
                    case NPCState.attack:
                        nvAgent.Stop();
                        anim.SetBool("isAttack", true);
                        break;

                }
                yield return null;
            }
        }

        public void LookToPlayer()
        {
            Vector3 v = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

            Quaternion rotation = Quaternion.LookRotation(v - transform.position);

            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2.0f);
        }

    }
}

