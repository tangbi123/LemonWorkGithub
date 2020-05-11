using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    public class TXInPool : MonoBehaviour
    {
        public float time;

        void OnEnable()
        {
            StartCoroutine(Push(time));
        }

        public IEnumerator Push(float time)
        {
            yield return new WaitForSeconds(time);
            ObjPool.GetInstance().PushObj(this.name, this.gameObject);
        }
    }
}
