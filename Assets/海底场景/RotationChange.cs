using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ns
{
    public class RotationChange : MonoBehaviour
    {
        public Volume volume;

        public HDRISky h;

        void Start()
        {
            volume = GetComponent<Volume>();
           // h = GetComponent<HDRISky>();
           // h = VolumeComponent<HDRISky>();
            
           volume.profile.TryGet<HDRISky>(out h);
            if (h == null) Debug.Log("can not find hdri");

            h.rotation.max = 360;
            h.rotation.min = 0;
            StartCoroutine(ChangeRotation());
        }

        /// <summary>
        /// 变动 rotatoin
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeRotation()
        {
            while (true)
            {
                if (h.rotation.value == 360) h.rotation.value = 0;
                h.rotation.value++;
                //一帧 加1
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
}
