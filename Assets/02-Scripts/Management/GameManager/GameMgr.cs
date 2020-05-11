using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager

{
    public class GameMgr : MonoBehaviour
    {
        public Transform startPos;
        public CameraSettings cs;
        public AllInputMgr m_Input;
        public GameObject lemon;
        public GameObject shuren;
        public GameObject eyu;

        public GameObject nowPlayer;

        public BaseShuxing shuxing;
        void Reset()
        {
            lemon = GameObject.Find("Lemon");
            shuren = GameObject.Find("Shuren");
            eyu = GameObject.Find("Eyu");

        }
        void Awake()
        {
            cs = GameObject.Find("CameraRig").GetComponent<CameraSettings>();
            m_Input = GetComponent<AllInputMgr>();

            if(lemon == null)
            lemon = GameObject.Find("Lemon");
            if(shuren == null)
            shuren = GameObject.Find("Shuren");
            if(eyu == null)
            eyu = GameObject.Find("Eyu");

            nowPlayer = lemon;
            shuren.active = false;
            eyu.active = false;

            shuxing = new BaseShuxing();
            lemon.transform.position = new Vector3
            (startPos.position.x, 5f, startPos.position.z);
        }
        void Update()
        {
            if (m_Input.QInput) ChangeLemon();
            if (Input.GetKeyDown(KeyCode.P)) ChangeShuren();
            if (Input.GetKeyDown(KeyCode.L)) ChangeEyu();
        }

        public void ChangeShuren()
        {
           
            shuren.transform.position = lemon.transform.position;
            lemon.active = false; ;
            shuren.active = true;
            cs.ChangeShuren();

            nowPlayer = shuren;
            shuxing.ChangeToShuren();
        }

        public void ChangeLemon()
        {
            
            lemon.transform.position = eyu.active == true?eyu.transform.position : shuren.transform.position;
            lemon.active = true;
            shuren.active = false;
            eyu.active = false;
            cs.ChangeLemon();

            nowPlayer = lemon;
            shuxing.ChangeToLemon();
        }
        public void ChangeEyu()
        {
            eyu.transform.position = lemon.transform.position;
            lemon.active = false; ;
            eyu.active = true;
            cs.ChangeEyu();

            nowPlayer = eyu;
            shuxing.ChangeToEyu();
        }
    }
}
