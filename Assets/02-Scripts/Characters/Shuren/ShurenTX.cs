using GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    public class ShurenTX : MonoBehaviour
    {
        public ParticleSystem fuzi;

        public ParticleSystem hurt;

        private GameObject daoguang1;

        private GameObject daoguang3;

        public Transform daoguangPos;

        public CameraSettings cameraSettings;

        void Reset()
        {
            fuzi = GameObject.Find("斧子特效").GetComponent<ParticleSystem>();
            hurt = GameObject.Find("Hurt").GetComponent<ParticleSystem>();

            daoguangPos = GameObject.Find("daoguangPos").GetComponent<Transform>();

            cameraSettings = GameObject.Find("CameraRig").GetComponent<CameraSettings>();

        }


        void Start()
        {
            fuzi.Stop();
            hurt.Stop();
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) Attack();
        }
        public void FuziTX()
        {
            fuzi.Play();
        }
        public void HurtTX()
        {
            hurt.Play();
        }

        public void Daoguang1()
        {
            daoguang1 = ObjPool.GetInstance().GetObj("TX/daoguang1");
            daoguang1.transform.position = daoguangPos.transform.position;
            daoguang1.transform.forward = this.gameObject.transform.forward;

            cameraSettings.ShakeCamera(0.5f,10,10,50,true);
        }

        public void Daoguang3()
        {
            daoguang3 = ObjPool.GetInstance().GetObj("TX/daoguang3");
            daoguang3.transform.position = daoguangPos.transform.position;
            daoguang3.transform.forward = this.gameObject.transform.forward;

            cameraSettings.ShakeCamera(0.5f, 10, 10, 50, true);
        }

        public void Attack()
        {
            EventCenter.GetInstance().EventTrigger("BAttack");
        }
    }
}
