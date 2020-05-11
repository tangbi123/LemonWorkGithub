using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///<summary>
	public class rotate : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 8;
        }
    }
}

