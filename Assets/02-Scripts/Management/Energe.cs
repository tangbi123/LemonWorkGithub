using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemon.Character;

namespace ns
{
    ///<summary>
    ///
    ///<summary>
	public class Energe : MonoBehaviour
    {
        private  CharacterDataTB cData;
        void Start()
        {
            cData = GameObject.FindWithTag("Player").GetComponent<CharacterDataTB>();
            DestroyMyself();
        }

        void OnTriggerEnter(Collider coll)
        {
            if(coll.gameObject.tag == "Player")
            {
                cData.blueEnerge += 5;
                if (cData.blueEnerge >= 100) cData.blueEnerge = 100;

                Destroy(this.gameObject);
            }
        }

        void DestroyMyself()
        {
            float time = 0;
            while(true)
            {
                time += Time.deltaTime;
                if(time >= 10)
                {
                    Destroy(this.gameObject, 2f);
                }
            }
        }
    }
}

